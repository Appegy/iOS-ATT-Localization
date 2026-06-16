using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using static Appegy.Att.Localization.TransparencyDescriptions;
using static Appegy.Att.Localization.TransparencyDescriptionsAPI;

namespace Appegy.Att.Localization
{
    internal class TransparencyDescriptionsPostProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => PostprocessorOrder;

        public void OnPostprocessBuild(BuildReport report)
        {
            Log($"Started work. Target platform is {report.summary.platform}. Required: {EnabledAutoXcodeUpdate}; Has default description: {!string.IsNullOrEmpty(DefaultDescription)}");
            if (report.summary.platform == BuildTarget.iOS && EnabledAutoXcodeUpdate)
            {
                if (string.IsNullOrEmpty(DefaultDescription))
                {
                    LogError($"You need to specify '{nameof(DefaultDescription).AddSpacesToSentence()}' to use ATT translations.");
                    return;
                }
                var buildPath = report.summary.outputPath;

                AddAppTrackingTransparencyFramework(buildPath);
                SetAppTransparencyDefaultDescription(buildPath, DefaultDescription);

                foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                {
                    var translation = GetAttDescription(language);
                    if (string.IsNullOrEmpty(translation))
                    {
                        continue;
                    }
                    if (translation == DefaultDescription && language != Default)
                    {
                        continue;
                    }
                    SetAppTransparencyDescription(buildPath, language, translation);
                }
            }

            Log($"Finished work.");
        }
    }
}