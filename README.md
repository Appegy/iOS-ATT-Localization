# 📓 iOS App Tracking Transparency Localization for Unity
[![openupm](https://img.shields.io/npm/v/org.appegy.att-loc?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/org.appegy.att-loc/)

## Description
![alt](Images/Preview.png)

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

Or just use [OpenUPM](https://openupm.com/packages/org.appegy.att-loc/)
```
openupm add org.appegy.att-loc
```

## Project Settings
The easiest way to add translations is just set them up in `Project Settings`.
```
Edit ➜ Project Settings ➜ iOS ATT Localization
```
Xcode project will be automatically updated after build if `Enabled Auto Xcode Update` is checked. You also must specify `English [EN] - Default` description. This description will be applied to `NSUserTrackingUsageDescription` property in main Info.plist. Now  you are ready to set descriptions for any language you need (**leave description empty if you want to use `English [EN] - Default`**).

## API
You also can set translation for any language by code. In your postprocessor script you can use next methods
```C#
public class YourPostProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.iOS)
        {
            var buildPath = report.summary.outputPath;
            
            // Add AppTrackingTransparency.framework to generated xcode project 
            TransparencyDescriptionsAPI.AddAppTrackingTransparencyFramework(buildPath);
            
            // Override NSUserTrackingUsageDescription in main Info.plist
            TransparencyDescriptionsAPI.SetAppTransparencyDefaultDescription(buildPath, "Default translation");
            
            // Set description for specific language
            TransparencyDescriptionsAPI.SetAppTransparencyDescription(buildPath, SystemLanguage.Russian, "Some russian text");
        }
    }
}
```