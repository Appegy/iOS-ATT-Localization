# 📓 iOS App Tracking Transparency Localization for Unity
[![package](https://img.shields.io/badge/version-1.0.0-green)](https://github.com/appegy/att-loc)

## Description
Provides localization of iOS App Tracking Transparency (ATT) descriptions. Perfectly works with oficial Unity's [iOS 14 Advertising Support](https://docs.unity3d.com/Packages/com.unity.ads.ios-support@1.0/manual/index.html) package.

## Installation
Manual add package to the ```manifest.json```.
```
"dependencies": {
  "org.appegy.att-loc": "https://github.com/appegy/att-loc.git",
  ...
},
```
Or you can specify version you need

```
"dependencies": {
  "org.appegy.att-loc": "https://github.com/appegy/att-loc.git#1.0.0",
  ...
},
```
Use OpenUPM to add package
```
Soon™
```

## Project Settings
The easiest way to add translations is just set them up in `Project Settings`.
```
Edit ➜ Project Settings ➜ iOS ATT Localization
```
Xcode project will be automatically updated after build if `Enabled Auto Xcode Update` is checked. You also must specify `English [EN] - Default` description. This description will be applied to `NSUserTrackingUsageDescription` property in main Info.plist. Now  you are ready to set descriptions for any language you need (**leave description empty if you want to use `English [EN] - Default`**).