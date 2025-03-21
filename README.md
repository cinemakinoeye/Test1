
# Test 1 Unity Project

## Dependencies

Unity Editor Version 2022.3.55f

XR Interaction Toolkit

OpenBrush SDK

## Scenes

Basic Swim 01 - demo basic swimming with controller

Swim World 01 - demo basic swimming with controller and OpenBrush objects

## Bugs and things to fix

installing OpenBrush SDK Error

At some point in Unity’s recent release cycles you need to add this line to your Packages/manifest.json file:
"com.unity.nuget.newtonsoft-json": "3.0.2",

ERROR - Importer(NativeFormatImporter) generated inconsistent result for asset(guid:a9a6963505ddf7f4d886008c6dc86122) "Assets/XR/Settings/Open XR Package Settings.asset"

BUG - Navigation problem, joystick change sends you flying away