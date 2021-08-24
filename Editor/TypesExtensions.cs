using System.Text;
using UnityEngine;

namespace Appegy.Att.Localization
{
    internal static class TypesExtensions
    {
        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                {
                    newText.Append(' ');
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static string GetLocalCodeIOS(this SystemLanguage lang)
        {
            string res = lang switch
            {
                SystemLanguage.Afrikaans => "AF",
                SystemLanguage.Arabic => "AR",
                SystemLanguage.Basque => "EU",
                SystemLanguage.Belarusian => "BY",
                SystemLanguage.Bulgarian => "BG",
                SystemLanguage.Catalan => "CA",
                SystemLanguage.Chinese => "ZH",
                SystemLanguage.Czech => "CS",
                SystemLanguage.Danish => "DA",
                SystemLanguage.Dutch => "NL",
                SystemLanguage.English => "EN",
                SystemLanguage.Estonian => "ET",
                SystemLanguage.Faroese => "FO",
                SystemLanguage.Finnish => "FI",
                SystemLanguage.French => "FR",
                SystemLanguage.German => "DE",
                SystemLanguage.Greek => "EL",
                SystemLanguage.Hebrew => "IW",
                SystemLanguage.Hungarian => "HU",
                SystemLanguage.Icelandic => "IS",
                SystemLanguage.Indonesian => "IN",
                SystemLanguage.Italian => "IT",
                SystemLanguage.Japanese => "JA",
                SystemLanguage.Korean => "KO",
                SystemLanguage.Latvian => "LV",
                SystemLanguage.Lithuanian => "LT",
                SystemLanguage.Norwegian => "NO",
                SystemLanguage.Polish => "PL",
                SystemLanguage.Portuguese => "PT",
                SystemLanguage.Romanian => "RO",
                SystemLanguage.Russian => "RU",
                SystemLanguage.SerboCroatian => "SH",
                SystemLanguage.Slovak => "SK",
                SystemLanguage.Slovenian => "SL",
                SystemLanguage.Spanish => "ES",
                SystemLanguage.Swedish => "SV",
                SystemLanguage.Thai => "TH",
                SystemLanguage.Turkish => "TR",
                SystemLanguage.Ukrainian => "UK",
                SystemLanguage.Vietnamese => "VI",
                SystemLanguage.ChineseSimplified => "ZH-Hans",
                SystemLanguage.ChineseTraditional => "ZH-Hant",
                _ => ""
            };
            return res;
        }
    }
}