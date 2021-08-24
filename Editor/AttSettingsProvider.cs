using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Appegy.Att.Localization.TransparencyDescriptions;

namespace Appegy.Att.Localization
{
    internal static class AttSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateDeviceSimulatorSettingsProvider()
        {
            var provider = new SettingsProvider($"Project/iOS 14 ATT Descriptions", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    EditorGUIUtility.labelWidth = 200;
                    EditorGUI.indentLevel++;

                    EnabledAutoXcodeUpdate = EditorGUILayout.Toggle(nameof(EnabledAutoXcodeUpdate).AddSpacesToSentence(), EnabledAutoXcodeUpdate);
                    var guiEnabled = GUI.enabled;
                    GUI.enabled = EnabledAutoXcodeUpdate;

                    PostprocessorOrder = EditorGUILayout.IntField(nameof(PostprocessorOrder).AddSpacesToSentence(), PostprocessorOrder);
                    DefaultDescription = EditorGUILayout.TextField(nameof(DefaultDescription).AddSpacesToSentence(), DefaultDescription);

                    foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                    {
                        if (language == SystemLanguage.Unknown)
                        {
                            continue;
                        }
                        var translation = GetAttDescription(language);
                        var newValue = EditorGUILayout.TextField($"{language} [{language.GetLocalCodeIOS()}]", translation);
                        if (translation != newValue)
                        {
                            SetAttDescription(language, newValue);
                        }
                    }

                    GUI.enabled = guiEnabled;
                    EditorGUI.indentLevel--;
                },
                footerBarGuiHandler = () =>
                {
                    if (GUILayout.Button("Reset To Default", GUILayout.Width(200)))
                    {
                        ResetToDefault();
                    }
                },

                keywords = new HashSet<string>(new[] {"att", "ios"}),
            };

            return provider;
        }
    }
}