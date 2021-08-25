#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

namespace Appegy.Att.Localization
{
    public static class TransparencyDescriptionsAPI
    {
        public const string UserTrackingUsageDescription = "NSUserTrackingUsageDescription";
        public const string AppTrackingTransparencyFramework = "AppTrackingTransparency.framework";

        public static void AddAppTrackingTransparencyFramework(string buildPath)
        {
            #if UNITY_IOS
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            Log($"{nameof(AddAppTrackingTransparencyFramework)}{Environment.NewLine}" +
                $"{nameof(buildPath)}: {buildPath}{Environment.NewLine}" +
                $"{nameof(projectPath)}: {projectPath}");
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            var xcodeTarget = project.GetUnityFrameworkTargetGuid();
            project.AddFrameworkToProject(xcodeTarget, AppTrackingTransparencyFramework, true);
            project.WriteToFile(projectPath);
            #endif
        }

        public static void SetAppTransparencyDefaultDescription(string buildPath, string translation)
        {
            #if UNITY_IOS
            var plistPath = buildPath + "/Info.plist";
            Log($"{nameof(SetAppTransparencyDefaultDescription)}{Environment.NewLine}" +
                $"{nameof(buildPath)}: {buildPath}{Environment.NewLine}" +
                $"{nameof(plistPath)}: {plistPath}{Environment.NewLine}" +
                $"{nameof(translation)}: {translation}");
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            plist.root.SetString(UserTrackingUsageDescription, translation);
            File.WriteAllText(plistPath, plist.WriteToString());
            #endif
        }

        public static void SetAppTransparencyDescription(string buildPath, SystemLanguage language, string translation)
        {
            #if UNITY_IOS
            var localeCode = language.GetLocalCodeIOS();
            if (string.IsNullOrEmpty(localeCode))
            {
                LogError($"Failed to set app transparency description for {language} - can't convert to country code");
                return;
            }
            if (string.IsNullOrEmpty(translation))
            {
                LogError($"Failed to set app transparency description for {language} - empty translation");
                return;
            }

            const string resourcesDirectoryName = "TransparencyResources";
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            var unityMainTargetGuid = project.GetUnityMainTargetGuid();

            var resourcesDirectoryPath = Path.Combine(buildPath, resourcesDirectoryName);
            var localeSpecificDirectoryName = localeCode + ".lproj";
            var localeSpecificDirectoryPath = Path.Combine(resourcesDirectoryPath, localeSpecificDirectoryName);
            var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPath, "InfoPlist.strings");
            var folderReferencePath = Path.Combine("./" + resourcesDirectoryName, localeSpecificDirectoryName);
            var translationLine = $"\"{UserTrackingUsageDescription}\" = \"{translation}\";\n";

            Log($"Translate ATT for {localeCode} with {translation}{Environment.NewLine}" +
                $"{nameof(resourcesDirectoryName)}: {resourcesDirectoryName}{Environment.NewLine}" +
                $"{nameof(resourcesDirectoryPath)}: {resourcesDirectoryPath}{Environment.NewLine}" +
                $"{nameof(localeSpecificDirectoryName)}: {localeSpecificDirectoryName}{Environment.NewLine}" +
                $"{nameof(localeSpecificDirectoryPath)}: {localeSpecificDirectoryPath}{Environment.NewLine}" +
                $"{nameof(infoPlistStringsFilePath)}: {infoPlistStringsFilePath}" +
                $"{nameof(folderReferencePath)}: {folderReferencePath}");

            if (!Directory.Exists(localeSpecificDirectoryPath))
            {
                Log($"Create directory: {localeSpecificDirectoryPath}");
                Directory.CreateDirectory(localeSpecificDirectoryPath);
            }

            if (File.Exists(infoPlistStringsFilePath))
            {
                Log($"{infoPlistStringsFilePath} already exists - append or change {UserTrackingUsageDescription}");
                var output = new List<string>();
                var lines = File.ReadAllLines(infoPlistStringsFilePath);
                var keyUpdated = false;
                foreach (var line in lines)
                {
                    if (line.Contains(UserTrackingUsageDescription))
                    {
                        output.Add(translationLine);
                        keyUpdated = true;
                    }
                    else
                    {
                        output.Add(line);
                    }
                }

                if (!keyUpdated)
                {
                    output.Add(translationLine);
                }

                File.WriteAllText(infoPlistStringsFilePath, string.Join(Environment.NewLine, output.ToArray()) + Environment.NewLine);
            }
            else
            {
                Log($"Create {infoPlistStringsFilePath} and append {UserTrackingUsageDescription}");
                File.WriteAllText(infoPlistStringsFilePath, translationLine);
            }

            var guid = project.AddFolderReference(folderReferencePath, Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName));
            project.AddFileToBuild(unityMainTargetGuid, guid);
            project.WriteToFile(projectPath);
            #endif
        }

        internal static void Log(string message)
        {
            Debug.Log($"[{nameof(TransparencyDescriptionsAPI)}]: {message}");
        }

        internal static void LogError(string message)
        {
            Debug.LogError($"[{nameof(TransparencyDescriptionsAPI)}]: {message}");
        }
    }
}