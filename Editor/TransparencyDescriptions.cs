using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Appegy.Att.Localization
{
    public class TransparencyDescriptions : ScriptableObject
    {
        internal const SystemLanguage Default = SystemLanguage.English;

        [Serializable]
        private class LanguagesDictionary : SerializableDictionary<int, string>
        {
        }

        [SerializeField]
        private bool _enabledAutoXcodeUpdate = true;
        [SerializeField]
        private int _postprocessorOrder;

        [SerializeField, HideInInspector]
        private LanguagesDictionary _attDescriptions = new LanguagesDictionary
        {
            {(int) SystemLanguage.Catalan, "Prement 'Permetre', s'utilitza la informació del dispositiu per a obtindre contingut publicitari més rellevant"},
            {(int) SystemLanguage.Chinese, "此数据将仅用于向您提供相关广告。它不会被出售或转让给任何第三方。"},
            {(int) SystemLanguage.English, "This data will be used to provide you with relevant ads only. It will not be sold or transferred to any third parties."},
            {(int) SystemLanguage.French, "Ces données serviront uniquement à vous fournir des annonces pertinentes. Elles ne seront ni vendues ni transférées à des tiers."},
            {(int) SystemLanguage.German, "Diese Daten werden dafür gebraucht, Ihnen nur relevante Werbung zu zeigen. Sie werden nicht an Dritte verkauft oder weitergegeben."},
            {(int) SystemLanguage.Indonesian, "Data ini akan digunakan untuk menyediakan hanya iklan yang relevan untuk Anda. Data tidak akan dijual atau ditransfer ke pihak ketiga mana pun."},
            {(int) SystemLanguage.Italian, "Questi dati verranno utilizzati solo per inviarvi pubblicità rilevanti. Non saranno venduti  o trasferiti ad alcuna terza parte."},
            {(int) SystemLanguage.Japanese, "本データは、お客様に関連性のある広告を提供する目的に限り使用されます。第三者に販売または譲渡されることはありません。"},
            {(int) SystemLanguage.Korean, "이 데이터는 고객님과 연관이 있는 광고만을 제공하는데 사용됩니다. 제3자에게 매각되거나 양도되지 않습니다."},
            {(int) SystemLanguage.Portuguese, "Esses dados serão usados para fornecer a você apenas anúncios relevantes. Não serão vendidos ou transferidos para terceiros."},
            {(int) SystemLanguage.Russian, "Эта информация используется только для предоставления релевантной рекламы. Она не будет передана третьим лицам."},
            {(int) SystemLanguage.Spanish, "Esta información se utilizará para ofrecerte solamente anuncios relevantes. No se venderá ni se transferirá a terceros."},
            {(int) SystemLanguage.Thai, "ข้อมูลนี้จะใช้เพื่อแสดงโฆษณาที่เกี่ยวข้องแก่คุณเท่านั้น จะไม่ขายหรือโอนให้กับบุคคลภายนอกใด ๆ"},
            {(int) SystemLanguage.Turkish, "Bu veriler yalnızca size ilgili reklamlar sunmak üzere kullanılacak. Herhangi bir üçüncü kişiye satılmayacak veya aktarılmayacak."},
            {(int) SystemLanguage.Vietnamese, "Dữ liệu này sẽ chỉ được sử dụng để cung cấp cho bạn những quảng cáo phù hợp. Nó sẽ không được bán hay chuyển giao cho bất kì bên thứ ba nào."},
            {(int) SystemLanguage.ChineseSimplified, "点击\"允许\"以使用设备信息获得更加相关的广告内容"},
            {(int) SystemLanguage.ChineseTraditional, "點擊\"允許\"以使用設備信息獲得更加相關的廣告內容"},
        };

        public static bool EnabledAutoXcodeUpdate
        {
            get => GetInstance()._enabledAutoXcodeUpdate;
            set
            {
                if (GetInstance()._enabledAutoXcodeUpdate != value)
                {
                    Undo.RecordObject(GetInstance(), $"Changed {EnabledAutoXcodeUpdate}");
                    GetInstance()._enabledAutoXcodeUpdate = value;
                    Save();
                }
            }
        }

        public static int PostprocessorOrder
        {
            get => GetInstance()._postprocessorOrder;
            set
            {
                if (GetInstance()._postprocessorOrder != value)
                {
                    Undo.RecordObject(GetInstance(), $"Changed {PostprocessorOrder}");
                    GetInstance()._postprocessorOrder = value;
                    Save();
                }
            }
        }

        public static string DefaultDescription
        {
            get => GetAttDescription(Default);
            set => SetAttDescription(Default, value);
        }

        public static string GetAttDescription(SystemLanguage language)
        {
            var inst = GetInstance();
            var result = inst._attDescriptions.TryGetValue((int) language, out var translation) ? translation : string.Empty;
            if (language == Default)
            {
                return result;
            }
            return result == DefaultDescription ? string.Empty : result;
        }

        public static void SetAttDescription(SystemLanguage language, string translation)
        {
            if (string.IsNullOrEmpty(translation))
            {
                RemoveAttDescription(language);
            }
            else if (language != Default && translation == DefaultDescription)
            {
                RemoveAttDescription(language);
            }
            else
            {
                Undo.RecordObject(GetInstance(), $"Changed Description for {language}");
                var inst = GetInstance();
                inst._attDescriptions[(int) language] = translation;
                Save();
            }
        }

        private static void RemoveAttDescription(SystemLanguage language)
        {
            Undo.RecordObject(GetInstance(), $"Removed Description for {language}");
            var dict = GetInstance()._attDescriptions;
            if (dict.Remove((int) language))
            {
                Save();
            }
        }

        #region Interal Save/Load

        private static TransparencyDescriptions _instance;
        private const string FailedToSaveError = "Failed to save" + nameof(TransparencyDescriptions) + " to " + PathToSettings + " Make sure the settings file is writable.";
        private const string PathToSettings = "ProjectSettings/" + nameof(TransparencyDescriptions) + ".asset";

        internal static TransparencyDescriptions GetInstance()
        {
            if (_instance != null) return _instance;
            var settings = CreateInstance<TransparencyDescriptions>();
            if (File.Exists(PathToSettings))
            {
                var settingsJson = File.ReadAllText(PathToSettings);
                JsonUtility.FromJsonOverwrite(settingsJson, settings);
            }

            _instance = settings;
            return settings;
        }

        public static void ResetToDefault()
        {
            if (File.Exists(PathToSettings))
            {
                File.Delete(PathToSettings);
                _instance = null;
            }
            Save();
        }

        internal static void Save()
        {
            if (_instance == null) return;
            var fileInfo = new FileInfo(PathToSettings);
            var settingsJson = JsonUtility.ToJson(_instance, true);
            if (fileInfo.Exists && settingsJson == File.ReadAllText(PathToSettings)) return;

            if (!AssetDatabase.IsOpenForEdit(PathToSettings) && !AssetDatabase.MakeEditable(PathToSettings))
            {
                Debug.LogWarning(FailedToSaveError);
                return;
            }

            try
            {
                if (fileInfo.Exists && fileInfo.IsReadOnly)
                {
                    fileInfo.IsReadOnly = false;
                }
                File.WriteAllText(PathToSettings, settingsJson);
            }
            catch (Exception)
            {
                Debug.LogWarning(FailedToSaveError);
            }
        }

        #endregion
    }
}