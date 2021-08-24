#if UNITY_EDITOR && UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using static Appegy.Att.Localization.TransparencyDescriptions;

namespace Appegy.Att.Localization
{
    internal class PostProcessorAtt : IPostprocessBuildWithReport
    {
        public int callbackOrder => PostprocessorOrder;

        public void OnPostprocessBuild(BuildReport report)
        {
            Log($"Started work. Target platform is {report.summary.platform}. Required: {EnabledAutoXcodeUpdate}");
            if (report.summary.platform == BuildTarget.iOS && EnabledAutoXcodeUpdate)
            {
                var plistPath = report.summary.outputPath + "/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
                plist.root.SetString("NSUserTrackingUsageDescription", DefaultDescription);
                File.WriteAllText(plistPath, plist.WriteToString());

                var buildPath = report.summary.outputPath;
                var projectPath = PBXProject.GetPBXProjectPath(buildPath);
                var project = new PBXProject();
                project.ReadFromFile(projectPath);
                var unityMainTargetGuid = project.GetUnityMainTargetGuid();
                var xcodeTarget = project.GetUnityFrameworkTargetGuid();
                project.AddFrameworkToProject(xcodeTarget, "AppTrackingTransparency.framework", true);

                const string resources = "Resources";
                const string attResources = "AppegyResources";

                foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                {
                    var translation = GetAttDescription(language);
                    var localeCode = language.GetLocalCodeIOS();
                    if (string.IsNullOrEmpty(localeCode) || string.IsNullOrEmpty(translation) || translation == DefaultDescription)
                    {
                        continue;
                    }
                    var resourcesDirectoryName = Directory.Exists(Path.Combine(buildPath, resources)) ? resources : attResources;
                    var resourcesDirectoryPath = Path.Combine(buildPath, resourcesDirectoryName);
                    var localeSpecificDirectoryName = localeCode + ".lproj";
                    var localeSpecificDirectoryPath = Path.Combine(resourcesDirectoryPath, localeSpecificDirectoryName);
                    var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPath, "InfoPlist.strings");

                    Log($"Translate ATT for {localeCode} with {translation}{Environment.NewLine}" +
                        $"{nameof(resourcesDirectoryName)}: {resourcesDirectoryName}{Environment.NewLine}" +
                        $"{nameof(resourcesDirectoryPath)}: {resourcesDirectoryPath}{Environment.NewLine}" +
                        $"{nameof(localeSpecificDirectoryName)}: {localeSpecificDirectoryName}{Environment.NewLine}" +
                        $"{nameof(localeSpecificDirectoryPath)}: {localeSpecificDirectoryPath}{Environment.NewLine}" +
                        $"{nameof(infoPlistStringsFilePath)}: {infoPlistStringsFilePath}");

                    if (!Directory.Exists(resourcesDirectoryPath))
                    {
                        Directory.CreateDirectory(resourcesDirectoryPath);
                    }
                    if (!Directory.Exists(localeSpecificDirectoryPath))
                    {
                        Directory.CreateDirectory(localeSpecificDirectoryPath);
                    }

                    var translationLine = "\"NSUserTrackingUsageDescription\" = \"" + translation + "\";\n";
                    if (File.Exists(infoPlistStringsFilePath))
                    {
                        var output = new List<string>();
                        var lines = File.ReadAllLines(infoPlistStringsFilePath);
                        var keyUpdated = false;
                        foreach (var line in lines)
                        {
                            if (line.Contains("NSUserTrackingUsageDescription"))
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

                        File.WriteAllText(infoPlistStringsFilePath, string.Join("\n", output.ToArray()) + "\n");
                    }
                    else
                    {
                        File.WriteAllText(infoPlistStringsFilePath, "/* Localized versions of Info.plist keys - Generated by Appegy */\n" + translationLine);
                    }

                    var guid = project.AddFolderReference(localeSpecificDirectoryPath, Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName));
                    project.AddFileToBuild(unityMainTargetGuid, guid);
                }
                project.WriteToFile(projectPath);
            }

            Log($"Finished work.");
        }

        private static void Log(string message)
        {
            Debug.Log($"[{nameof(PostProcessorAtt)}]: {message}");
        }
    }
}
#endif