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
            {(int) SystemLanguage.Afrikaans, "Hierdie data sal slegs gebruik word om relevante advertensies aan u te verskaf en sal nie aan derde partye verkoop of oorgedra word nie."},
            {(int) SystemLanguage.Arabic, "سيتم استخدام هذه البيانات لتزويدك بالإعلانات ذات الصلة فقط ولن يتم بيعها أو نقلها إلى أي طرف ثالث."},
            {(int) SystemLanguage.Basque, "Datu hauek iragarki garrantzitsuak eskaintzeko erabiliko dira eta ez zaizkie hirugarrenei salduko edo transferituko."},
            {(int) SystemLanguage.Belarusian, "Гэтыя дадзеныя будуць выкарыстоўвацца толькі для прадастаўлення вам адпаведнай рэкламы і не будуць прададзеныя або перададзеныя трэцім бакам."},
            {(int) SystemLanguage.Bulgarian, "Тези данни ще се използват само за предоставяне на подходящи реклами и няма да бъдат продавани или прехвърляни на трети страни."},
            {(int) SystemLanguage.Catalan, "Aquestes dades s’utilitzaran només per proporcionar-vos anuncis rellevants i no es vendran ni transferiran a tercers."},
            {(int) SystemLanguage.Chinese, "这些数据将仅用于向您提供相关广告，不会出售或转让给任何第三方。"},
            {(int) SystemLanguage.ChineseSimplified, "这些数据将仅用于向您提供相关广告，不会出售或转让给任何第三方。"},
            {(int) SystemLanguage.ChineseTraditional, "這些數據將僅用於向您提供相關廣告，不會出售或轉讓給任何第三方。"},
            {(int) SystemLanguage.Czech, "Tato data budou použita pouze k poskytnutí relevantních reklam a nebudou prodána ani převedena žádným třetím stranám."},
            {(int) SystemLanguage.Danish, "Disse data vil kun blive brugt til at give dig relevante annoncer og vil ikke blive solgt eller overført til tredjemand."},
            {(int) SystemLanguage.Dutch, "Deze gegevens worden alleen gebruikt om u relevante advertenties te bieden en worden niet verkocht of overgedragen aan derden."},
            {(int) SystemLanguage.English, "This data will be used to provide you with relevant ads only and will not be sold or transferred to any third parties."},
            {(int) SystemLanguage.Estonian, "Neid andmeid kasutatakse ainult asjakohaste reklaamide esitamiseks ning neid ei müüda ega edastata kolmandatele osapooltele."},
            {(int) SystemLanguage.Finnish, "Näitä tietoja käytetään vain osuvien mainosten tarjoamiseen, eikä niitä myydä tai siirretä kolmansille osapuolille."},
            {(int) SystemLanguage.French, "Ces données seront utilisées uniquement pour vous fournir des publicités pertinentes et ne seront ni vendues ni transférées à des tiers."},
            {(int) SystemLanguage.German, "Diese Daten werden nur verwendet, um Ihnen relevante Anzeigen bereitzustellen und werden nicht an Dritte verkauft oder weitergegeben."},
            {(int) SystemLanguage.Greek, "Αυτά τα δεδομένα θα χρησιμοποιηθούν για να σας παρέχουν μόνο σχετικές διαφημίσεις και δεν θα πωληθούν ή θα μεταφερθούν σε τρίτους."},
            {(int) SystemLanguage.Hebrew, "נתונים אלה ישמשו כדי לספק לך מודעות רלוונטיות בלבד ולא יימכרו או יועברו לצדדים שלישיים כלשהם."},
            {(int) SystemLanguage.Hungarian, "Ezeket az adatokat arra használjuk fel, hogy csak releváns hirdetéseket jelenítsünk meg Önnek, és nem adjuk el vagy adjuk át harmadik félnek."},
            {(int) SystemLanguage.Icelandic, "Þessi gögn verða aðeins notuð til að veita þér viðeigandi auglýsingar og verða ekki seldar eða fluttar til þriðja aðila."},
            {(int) SystemLanguage.Indonesian, "Data ini akan digunakan untuk menyediakan Anda dengan iklan yang relevan saja dan tidak akan dijual atau ditransfer ke pihak ketiga mana pun."},
            {(int) SystemLanguage.Italian, "Questi dati verranno utilizzati solo per fornire annunci pertinenti e non saranno venduti o trasferiti a terzi."},
            {(int) SystemLanguage.Japanese, "このデータは、関連する広告を提供するためにのみ使用され、第三者に販売または転送されることはありません。"},
            {(int) SystemLanguage.Korean, "이 데이터는 관련 광고를 제공하기 위해서만 사용되며 제3자에게 판매되거나 양도되지 않습니다."},
            {(int) SystemLanguage.Latvian, "Šie dati tiks izmantoti, lai sniegtu jums tikai atbilstošas reklāmas, un tie netiks pārdoti vai nodoti trešajām pusēm."},
            {(int) SystemLanguage.Lithuanian, "Šie duomenys bus naudojami tik norint pateikti jums atitinkamus skelbimus ir nebus parduodami ar perduodami trečiosioms šalims."},
            {(int) SystemLanguage.Norwegian, "Disse dataene vil bare bli brukt til å gi deg relevante annonser og vil ikke bli solgt eller overført til tredjeparter."},
            {(int) SystemLanguage.Polish, "Dane te będą wykorzystywane wyłącznie do dostarczania odpowiednich reklam i nie będą sprzedawane ani przekazywane stronom trzecim."},
            {(int) SystemLanguage.Portuguese, "Esses dados serão usados apenas para fornecer anúncios relevantes e não serão vendidos ou transferidos a terceiros."},
            {(int) SystemLanguage.Romanian, "Aceste date vor fi utilizate doar pentru a vă oferi reclame relevante și nu vor fi vândute sau transferate către terți."},
            {(int) SystemLanguage.Russian, "Эти данные будут использоваться только для предоставления вам релевантной рекламы и не будут проданы или переданы третьим лицам."},
            {(int) SystemLanguage.SerboCroatian, "Ови подаци ће се користити само за пружање релевантних огласа и неће се продавати нити преносити трећим странама."},
            {(int) SystemLanguage.Slovak, "Tieto údaje budú použité len k tomu, aby vám poskytli relevantné reklamy a nebudú predané ani prevedené na žiadne tretie strany."},
            {(int) SystemLanguage.Slovenian, "Ti podatki bodo uporabljeni samo za zagotavljanje ustreznih oglasov in ne bodo prodani ali preneseni tretjim osebam."},
            {(int) SystemLanguage.Spanish, "Estos datos se utilizarán para proporcionarle anuncios relevantes únicamente y no se venderán ni transferirán a terceros."},
            {(int) SystemLanguage.Swedish, "Dessa uppgifter kommer endast att användas för att ge dig relevanta annonser och kommer inte att säljas eller överföras till tredje part."},
            {(int) SystemLanguage.Thai, "ข้อมูลนี้จะถูกนำมาใช้เพื่อให้คุณมีโฆษณาที่เกี่ยวข้องเท่านั้น และจะไม่ขายหรือโอนไปยังบุคคลที่สาม"},
            {(int) SystemLanguage.Turkish, "Bu veriler yalnızca size ilgili reklamları sağlamak için kullanılacaktır ve herhangi bir üçüncü tarafa satılmayacak veya aktarılmayacaktır."},
            {(int) SystemLanguage.Ukrainian, "Ці дані будуть використовуватися лише для надання вам релевантної реклами та не будуть продаватися або передаватися третім сторонам."},
            {(int) SystemLanguage.Vietnamese, "Dữ liệu này sẽ chỉ được sử dụng để cung cấp cho bạn các quảng cáo có liên quan và sẽ không được bán hoặc chuyển giao cho bất kỳ bên thứ ba nào."},
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