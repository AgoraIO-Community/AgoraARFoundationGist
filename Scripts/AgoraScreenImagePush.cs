using System.Collections;
using UnityEngine;
using Agora.Rtc;

public class AgoraScreenImagePush : MonoBehaviour
{
    [SerializeField] string AppID;
    [SerializeField] string Token;
    [SerializeField] string ChannelName;

    IRtcEngine RtcEngine;

    bool JoinedChannel { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        InitAgora();
        SetExternalVideoSource();
        JoinChannel();
    }

    void InitAgora()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngineContext context = new RtcEngineContext(AppID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        RtcEngine.Initialize(context);
        RtcEngine.InitEventHandler(handler);

        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            bitrate = 0,
            minBitrate = 1,
            dimensions = new VideoDimensions { width = Screen.width, height = Screen.height },
            orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_ENABLED
        };
        RtcEngine.SetVideoEncoderConfiguration(config);
        // For video testing only
        RtcEngine.DisableAudio();

        RtcEngine.EnableVideo();
    }


    private void SetExternalVideoSource()
    {
        int rc = RtcEngine.SetExternalVideoSource(true, false, EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME, new SenderOptions());
        Debug.Log("SetExternalVideoSource returns " + rc);
    }

    private void JoinChannel()
    {
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        RtcEngine.JoinChannel(Token, ChannelName);
    }

    private void OnDestroy()
    {
        Debug.LogWarning("OnDestroy");
        JoinedChannel = false;
        if (RtcEngine != null)
        {
            RtcEngine.InitEventHandler(null);
            RtcEngine.LeaveChannel();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    int timestamp = 0;
    IEnumerator PushFrame()
    {
        // yield return new WaitWhile(() => mTexture == null);
        Debug.Log("PushFrame");
        while (JoinedChannel)
        {
            yield return new WaitForEndOfFrame();
            var mTexture = ScreenCapture.CaptureScreenshotAsTexture();
            // Get the Raw Texture data from the the from the texture and apply it to an array of bytes
            byte[] bytes = mTexture.GetRawTextureData();
            // int size = Marshal.SizeOf(bytes[0]) * bytes.Length;
            // Check to see if there is an engine instance already created
            //if the engine is present
            if (RtcEngine != null)
            {
                //Create a new external video frame
                ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
                //Set the buffer type of the video frame
                externalVideoFrame.type = VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
                // Set the video pixel format
                externalVideoFrame.format = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA;
                //apply raw data you are pulling from the rectangle you created earlier to the video frame
                externalVideoFrame.buffer = bytes;
                //Set the width of the video frame (in pixels)
                externalVideoFrame.stride = mTexture.width;
                //Set the height of the video frame
                externalVideoFrame.height = mTexture.height;
                //Rotate the video frame (0, 90, 180, or 270)
                externalVideoFrame.rotation = 180;
                externalVideoFrame.timestamp = System.DateTime.Now.Ticks / 10000;
                //Push the external video frame with the frame we just created
                int ret = RtcEngine.PushVideoFrame(externalVideoFrame);
                if (timestamp % 10 == 0)
                {
                    Debug.Log($"Pushed frame {ret} timestamp = " + timestamp);
                }

            }
            Object.Destroy(mTexture);
        }
    }

    #region -- Agora Event ---

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        AgoraScreenImagePush _app;
        public UserEventHandler(AgoraScreenImagePush app)
        {
            _app = app;
        }

        public override void OnError(int err, string msg)
        {
            msg = _app.RtcEngine.GetErrorDescription(err);
            Debug.LogError(string.Format("OnError err: {0}, msg: {1}", err, msg));
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            int build = 0;
            Debug.Log(string.Format("sdk version: ${0}",
                _app.RtcEngine.GetVersion(ref build)));
            Debug.Log(
                string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                    connection.channelId, connection.localUid, elapsed));
            _app.JoinedChannel = true;
            _app.StartCoroutine(_app.PushFrame());
        }

    }
    #endregion
}