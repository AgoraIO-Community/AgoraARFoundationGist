# Unity AR Foundation - Agora Code Gist

This repo contains scripts/assets only to help Unity project that tries to use Agora with Unity ARFoundation framework.  You should be able to just download the code and add to the Unity project. 

## Developer Environment Prerequisites
- Unity Editor, version 2021 LTS or above is recommended
- Project using Unity ARFoundation
- [Agora Developer Account](https://console.agora.io/)
- [Agora Unity Video SDK](https://docs.agora.io/en/sdks?platform=unity) 

## Quick Start

This section shows you how to prepare, build, and run a sample application.  We use [the official Unity AR Foundation Samples project](https://github.com/Unity-Technologies/arfoundation-samples) as an example.
 
### Obtain an App ID
 

To build and run with the Agora Video SDK, get an App ID:

1. Create a developer account at [agora.io](https://dashboard.agora.io/signin/). Once you finish the signup process, you will be redirected to the Dashboard.

2. Navigate in Agora Console on the left to **Projects** > **More** > **Create** > **Create New Project**.

3. Save the **App ID** from the Dashboard for later use.
### Import the Agora SDK
Obtain the Video SDK from the link above and import the unity package into the project.  Run a quick test in the Editor with your AppID first to verify your set up using the include API-Examples.
![api examples](https://github.com/icywind/AgoraARFoundationGist/assets/1261195/7dcfb9a1-2fff-4550-9d56-15aafcf4ee19)
### Add the Agora script
1. Clone this repo or simply download the script that you need to your project's Assets folder where appropriate.  Open the sample scene.
2. Create an empty game object in the scene and attach the script to it
3. Fill in your AppID, token and Channel name information.  For AppID that were created for Test mode only, you don't need to supply a token.  Otherwise, obtain a temporary token from your Agora developer console. 
![WorldCameraWithUserFacingFaceTracking_-_arfoundation](https://github.com/icywind/AgoraARFoundationGist/assets/1261195/ea13f8b7-f48a-48c6-bb75-322279479e13)
### Run the Application
Build the application to mobile device for testing.  In this example, an Android device was used.  We cast its screen to the computer for capturing.  While your Unity Editor is still open, run the *Agora API-Example/Basic/JoinChannelVideo* demo to view the stream.

![ar face](https://github.com/icywind/AgoraARFoundationGist/assets/1261195/f1e60b51-3e99-4851-be17-9d147815ce05)


## License

The MIT License (MIT).
