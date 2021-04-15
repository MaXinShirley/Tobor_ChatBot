# Tobor Chatbot app
Eon Reality - Unity project for Chatbot application

## Important Settings
- Unity 2020.1.4f1

- Android:
	- Minimum API Level: Android 7.0 'Nougat' (API Level 24)
	- ARCore required apps require a minimum SDK version of 24.
	
- iOS:
	- ARKit required apps require a minimum SDK version of 11.0.
	- Refer here [https://developer.apple.com/support/required-device-capabilities] for ARKit's hardware compatibility.

## Dependent Software(s)
- Android Target Support for Unity
- iOS Target Support for Unity
- Android SDK
- JDK, NDK

## Build Settings
- Common Settings
	- Resolution and Presenstation
		- Orientation
			- Default Orientation: Portrait
	- Other Settings
		- Bundke Identifier: com.EonReality.Q2916_Tobor
- Android
	- Other Settings
		- Rendering
			- Color Space: Gamma
			- Auto Graphics API: Unchecked
			- Graphics APIs:
				- OpenGLES3
			- Color Gamut:
				- sRGB
			- Multithreaded Rendering: checked
			- Static Batching: checked
			- Graphics Jobs (Experimental): Unchecked
		- Identification:
			- Minimum API Level: Android 7.0 'Nougat' (API Level 24)
		- Configuration
			- Api Compatibility Level: .NET Standard 2.0
		- Legacy
			- Clamp BlendShapes (Deprecated): Unchecked
- iOS
	- Other Settings
		- Configuration
			- Api Compatibility Level: .NET Standard 2.0
			- Camera Usage Description: Required for augmented reality support.
			- Microphone Usage Description: Required for chatbot support.
			- Target Device :iPhone Only
			- Target SDK: Device SDK
			- Target minimum iOS Version: 11.0
			- Architecture: ARM64

- This project has been tested on both Android and iOS. 

## Build Instruction
- iOS
	- Requires Xcode8 or higher. Target iOS 10.0
	- Add framework:
		- speech.framework
		- AVFoundation.framework
		- AVKit.framework
	- Add permission
		- Privacy – Microphone Usage Description      
		- Privacy – Speech Recognition Usage Description
	- Under Signing & Capabilities, tick Automatically manage signing
	- Change Team to Eon Reality Pte Ltd
	- Build as per normal.

## Usage Guide
None at the moment

## Unity Package Manager
| Package              	  | Version 			| Purpose 	|
|--------------------	  |-------			    |---------	|
| AR Foundation           | 3.1.10	            | AR Subsytems	|
| ARCore XR Plugin        | 3.1.8   	        | Native Google ARCore |
| ARKit XR Plugin         | 3.1.8	            | Native Apple ARKit |
| 2D Sprite               | 1.0.0	            | Create and edit Sprite assets |
| XR Plugin Management    | 3.2.16              | Management for XR plug-ins |

## Plugins
- SpeechToTextPlugin for both iOS and Android
- Refer here [https://github.com/PingAK9/Speech-And-Text-Unity-iOS-Android]


## 3rd party libraries
- Chatbot is loading JSON files for all 6 phrases to get the dialogs.
- JSON and WEST files are stored under Syn.bot Resources.
- Unity only read .json, it’s unable to read the .west file. 
- To create the .json file, simply replace the extension .west to .json
- If edit is required, open the .west files using Syn Oryzer Studio.


To Upgrade from a unitypackage or the Asset Store:
1. Delete the affected folder(s)
2. Import the new unitypackage or from the Asset Store
3. If the original author failed to maintain the script guid, links in prefabs will be lost, there will be a need to change guid manually


| Package/Library     | Installation Method           | Version      | Purpose                                     | Default Location            | Comments  |
|-----------------------|------------------------------------------------------------|----------------|------------------------------------|---------------------------------------------------|----------------------------------------------------|
| Syn.Bot             | Assets Store                  | 1.0      | Build chatbot dialogs                           | `Assets\Syn.Bot`             |           |
| Unity-Logs-Viewer   | Asset Store                   | 1.8          | View console log in built                   | `Assets\Unity-Logs-Viewer`   |           |
