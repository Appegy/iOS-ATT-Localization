using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Appegy.Att.Localization.TransparencyDescriptions;

namespace Appegy.Att.Localization
{
    internal static class TransparencyDescriptionsSettingsProvider
    {
        private static readonly HashSet<SystemLanguage> _checkedLanguages = new HashSet<SystemLanguage>();
        private static Texture2D _linkedinIcon;
        private static Texture2D _githubIcon;
        private static GUIContent _clearButtonStyle;
        private static GUIContent _resetButtonStyle;
        private static GUIContent _linkedinContent;
        private static GUIContent _githubContent;

        [SettingsProvider]
        public static SettingsProvider CreateAttSettingsProvider()
        {
            _linkedinIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.appegy.ios-att-localization/Images/LinkedIn.png");
            _githubIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.appegy.ios-att-localization/Images/Github.png");

            _clearButtonStyle = new GUIContent("X", $"Clear translation and use {Default} instead");
            _resetButtonStyle = new GUIContent("R", "Revert translation to Default value");
            _linkedinContent = new GUIContent(_linkedinIcon);
            _githubContent = new GUIContent(_githubIcon);

            var provider = new SettingsProvider($"Project/iOS ATT Localization", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    EditorGUIUtility.labelWidth = 200;
                    EditorGUI.indentLevel++;

                    EnabledAutoXcodeUpdate = EditorGUILayout.Toggle(nameof(EnabledAutoXcodeUpdate).AddSpacesToSentence(), EnabledAutoXcodeUpdate);
                    var guiEnabled = GUI.enabled;
                    GUI.enabled = EnabledAutoXcodeUpdate;

                    PostprocessorOrder = EditorGUILayout.IntField(nameof(PostprocessorOrder).AddSpacesToSentence(), PostprocessorOrder);
                    DefaultDescription = EditorGUILayout.TextField($"{Default} [{Default.GetLocalCodeIOS()}] - Default", DefaultDescription);
                    if (string.IsNullOrEmpty(DefaultDescription))
                    {
                        EditorGUILayout.HelpBox($"You need to specify '{nameof(DefaultDescription).AddSpacesToSentence()}' to use translations. Otherwise it won't work", MessageType.Error);
                    }
                    GUI.enabled = EnabledAutoXcodeUpdate && !string.IsNullOrEmpty(DefaultDescription);
                    EditorGUILayout.Space();

                    _checkedLanguages.Clear();
                    foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                    {
                        if (_checkedLanguages.Contains(language))
                        {
                            continue;
                        }
                        _checkedLanguages.Add(language);
                        switch (language)
                        {
                            case Default:
                            case SystemLanguage.Unknown:
                                continue;
                        }
                        var translation = GetAttDescription(language);
                        EditorGUILayout.BeginHorizontal();
                        var newValue = EditorGUILayout.TextField($"{language} [{language.GetLocalCodeIOS()}]", translation);
                        if (translation != newValue)
                        {
                            SetAttDescription(language, newValue);
                        }
                        if (GUILayout.Button(_clearButtonStyle, GUILayout.Width(18)))
                        {
                            SetAttDescription(language, string.Empty);
                        }
                        if (GUILayout.Button(_resetButtonStyle, GUILayout.Width(18)))
                        {
                            ResetToDefault(language);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    GUI.enabled = guiEnabled;
                    EditorGUI.indentLevel--;
                },
                footerBarGuiHandler = () =>
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Clear All", EditorStyles.miniButtonLeft, GUILayout.Width(200)))
                    {
                        foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
                        {
                            switch (language)
                            {
                                case Default:
                                case SystemLanguage.Unknown:
                                    continue;
                            }
                            SetAttDescription(language, string.Empty);
                        }
                    }
                    if (GUILayout.Button("Reset To Default", EditorStyles.miniButtonRight, GUILayout.Width(200)))
                    {
                        ResetToDefault();
                    }
                    GUILayout.Label("");
                    GUILayout.Label("Created by Ivan Murashka", EditorStyles.toolbarButton, GUILayout.Width(155));
                    if (GUILayout.Button(_linkedinContent, EditorStyles.toolbarButton, GUILayout.Width(23), GUILayout.Height(18)))
                    {
                        Application.OpenURL("https://www.linkedin.com/in/imurashka/");
                    }
                    if (GUILayout.Button(_githubContent, EditorStyles.toolbarButton, GUILayout.Width(23), GUILayout.Height(18)))
                    {
                        Application.OpenURL("https://github.com/imurashka/");
                    }
                    EditorGUILayout.EndHorizontal();
                },
                keywords = new HashSet<string>(new[] { "att", "ios" }),
            };

            return provider;
        }
    }
}