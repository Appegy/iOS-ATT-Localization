using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Appegy.Att.Localization
{
    public class TransparencyDescriptions : ScriptableObject
    {
        [Serializable]
        private class LanguagesDictionary : SerializableDictionary<int, string>
        {
        }

        internal const SystemLanguage Default = SystemLanguage.English;
        private static readonly LanguagesDictionary _defaults = CreateDefault();

        [SerializeField]
        private bool _enabledAutoXcodeUpdate = true;
        [SerializeField]
        private int _postprocessorOrder;

        [SerializeField, HideInInspector]
        private LanguagesDictionary _attDescriptions = CreateDefault();

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

        public static string GetDefaultAttDescription(SystemLanguage language)
        {
            return _defaults.TryGetValue((int)language, out var translation) ? translation : string.Empty;
        }

        public static string GetAttDescription(SystemLanguage language)
        {
            var inst = GetInstance();
            var result = inst._attDescriptions.TryGetValue((int)language, out var translation) ? translation : string.Empty;
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
                inst._attDescriptions[(int)language] = translation;
                Save();
            }
        }

        private static void RemoveAttDescription(SystemLanguage language)
        {
            Undo.RecordObject(GetInstance(), $"Removed Description for {language}");
            var dict = GetInstance()._attDescriptions;
            if (dict.Remove((int)language))
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

        public static void ResetToDefault(SystemLanguage language)
        {
            SetAttDescription(language, GetDefaultAttDescription(language));
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

        private static LanguagesDictionary CreateDefault()
        {
            var dictionary = new LanguagesDictionary();

            foreach (SystemLanguage language in Enum.GetValues(typeof(SystemLanguage)))
            {
                if (language == SystemLanguage.Unknown) continue;

                var description = GetDescriptionForLanguage(language);

                if (!string.IsNullOrEmpty(description))
                {
                    dictionary[(int)language] = description;
                }
                else
                {
                    Debug.LogWarning($"[TransparencyDescriptions] Missing default translation for language: {language}");
                }
            }

            return dictionary;
        }

        private static string GetDescriptionForLanguage(SystemLanguage language)
        {
            return language switch
            {
                SystemLanguage.Afrikaans => "Hierdie data sal slegs gebruik word om relevante advertensies te wys en sal nie aan derde partye verkoop of oorgedra word nie.",
                SystemLanguage.Arabic => "سيتم استخدام هذه البيانات فقط لتقديم إعلانات تناسب اهتماماتك، ولن يتم بيعها أو نقلها إلى أي طرف ثالث.",
                SystemLanguage.Basque => "Datu hauek iragarki egokiak erakusteko soilik erabiliko dira, eta ez zaizkie hirugarrenei saldu edo lagako.",
                SystemLanguage.Belarusian => "Гэтыя даныя будуць выкарыстоўвацца толькі для паказу актуальнай рэкламы і не будуць прададзены або перададзены трэцім асобам.",
                SystemLanguage.Bulgarian => "Тези данни ще се използват само за предоставяне на подходящи реклами и няма да бъдат продавани или предоставяни на трети страни.",
                SystemLanguage.Catalan => "Aquestes dades només s'utilitzaran per mostrar anuncis rellevants i no es vendran ni es transferiran a tercers.",
                SystemLanguage.Chinese => "此数据仅用于向您提供相关广告，不会出售或转让给任何第三方。",
                SystemLanguage.ChineseSimplified => "此数据仅用于向您提供相关广告，不会出售或转让给任何第三方。",
                SystemLanguage.ChineseTraditional => "此資料僅用於向您提供相關廣告，不會出售或轉讓給任何第三方。",
                SystemLanguage.Czech => "Tato data budou použita pouze k zobrazování relevantních reklam a nebudou prodána ani předána třetím stranám.",
                SystemLanguage.Danish => "Disse data vil kun blive brugt til at vise dig relevante annoncer og vil ikke blive solgt eller videregivet til tredjeparter.",
                SystemLanguage.Dutch => "Deze gegevens worden alleen gebruikt om relevante advertenties te tonen en worden niet verkocht of overgedragen aan derden.",
                SystemLanguage.English => "This data will be used to provide you with relevant ads only and will not be sold or transferred to any third parties.",
                SystemLanguage.Estonian => "Neid andmeid kasutatakse ainult asjakohaste reklaamide kuvamiseks ning neid ei müüda ega edastata kolmandatele isikutele.",
                SystemLanguage.Faroese => "Hesar dáta verða bert brúktar til at geva tær viðkomandi lýsingar og verða ikki seldar ella latið víðari til triðja part.",
                SystemLanguage.Finnish => "Näitä tietoja käytetään vain osuvien mainosten näyttämiseen, eikä niitä myydä tai luovuteta kolmansille osapuolille.",
                SystemLanguage.French => "Ces données seront utilisées uniquement pour vous présenter des publicités pertinentes et ne seront ni vendues ni transférées à des tiers.",
                SystemLanguage.German => "Diese Daten werden nur verwendet, um Ihnen relevante Werbung anzuzeigen, und werden nicht an Dritte verkauft oder weitergegeben.",
                SystemLanguage.Greek => "Αυτά τα δεδομένα θα χρησιμοποιηθούν μόνο για την παροχή σχετικών διαφημίσεων και δεν θα πωληθούν ή μεταβιβαστούν σε τρίτους.",
                SystemLanguage.Hebrew => "נתונים אלה ישמשו להצגת פרסומות רלוונטיות בלבד ולא יימכרו או יועברו לצד שלישי כלשהו.",
                SystemLanguage.Hindi => "इस डेटा का उपयोग केवल आपको प्रासंगिक विज्ञापन दिखाने के लिए किया जाएगा और इसे किसी भी तीसरे पक्ष को बेचा या हस्तांतरित नहीं किया जाएगा।",
                SystemLanguage.Hungarian => "Ezeket az adatokat kizárólag releváns hirdetések megjelenítésére használjuk, és nem adjuk el vagy adjuk át harmadik félnek.",
                SystemLanguage.Icelandic => "Þessi gögn verða eingöngu notuð til að birta þér viðeigandi auglýsingar og verða hvorki seld né afhent þriðja aðila.",
                SystemLanguage.Indonesian => "Data ini hanya akan digunakan untuk menyediakan iklan yang relevan bagi Anda dan tidak akan dijual atau dialihkan kepada pihak ketiga mana pun.",
                SystemLanguage.Italian => "Questi dati saranno utilizzati solo per mostrarti annunci pertinenti e non verranno venduti o ceduti a terzi.",
                SystemLanguage.Japanese => "このデータは、関連性の高い広告を提供するためにのみ使用され、第三者に販売または提供されることはありません。",
                SystemLanguage.Korean => "이 데이터는 관련 광고를 제공하는 데에만 사용되며 제3자에게 판매되거나 양도되지 않습니다.",
                SystemLanguage.Latvian => "Šie dati tiks izmantoti tikai, lai nodrošinātu jums atbilstošas reklāmas, un tie netiks pārdoti vai nodoti trešajām personām.",
                SystemLanguage.Lithuanian => "Šie duomenys bus naudojami tik siekiant pateikti jums aktualius skelbimus ir nebus parduodami ar perduodami trečiosioms šalims.",
                SystemLanguage.Norwegian => "Disse dataene vil kun bli brukt til å gi deg relevante annonser, og vil ikke bli solgt eller overført til tredjeparter.",
                SystemLanguage.Polish => "Dane te będą wykorzystywane wyłącznie do wyświetlania dopasowanych reklam i nie będą sprzedawane ani udostępniane stronom trzecim.",
                SystemLanguage.Portuguese => "Estes dados serão utilizados apenas para lhe fornecer anúncios relevantes e não serão vendidos ou transferidos a terceiros.",
                SystemLanguage.Romanian => "Aceste date vor fi utilizate doar pentru a vă afișa reclame relevante și nu vor fi vândute sau transferate către terți.",
                SystemLanguage.Russian => "Эти данные будут использоваться только для показа вам релевантной рекламы и не будут проданы или переданы третьим лицам.",
                SystemLanguage.SerboCroatian => "Ovi podaci će se koristiti samo za prikazivanje relevantnih oglasa i neće se prodavati niti ustupati trećim stranama.",
                SystemLanguage.Slovak => "Tieto údaje budú použité len na zobrazovanie relevantných reklám a nebudú predané ani postúpené tretím stranám.",
                SystemLanguage.Slovenian => "Ti podatki bodo uporabljeni le za prikazovanje ustreznih oglasov in ne bodo prodani ali posredovani tretjim osebam.",
                SystemLanguage.Spanish => "Estos datos se utilizarán únicamente para mostrarle anuncios relevantes y no se venderán ni transferirán a terceros.",
                SystemLanguage.Swedish => "Dessa data kommer endast att användas för att visa relevanta annonser och kommer inte att säljas eller överföras till tredje part.",
                SystemLanguage.Thai => "ข้อมูลนี้จะใช้เพื่อนำเสนอโฆษณาที่เกี่ยวข้องกับคุณเท่านั้น และจะไม่ถูกขายหรือส่งต่อให้แก่บุคคลที่สาม",
                SystemLanguage.Turkish => "Bu veriler yalnızca size ilgili reklamları göstermek için kullanılacak olup, üçüncü taraflara satılmayacak veya devredilmeyecektir.",
                SystemLanguage.Ukrainian => "Ці дані будуть використовуватися лише для показу релевантної реклами і не будуть продані чи передані третім сторонам.",
                SystemLanguage.Vietnamese => "Dữ liệu này sẽ chỉ được sử dụng để cung cấp quảng cáo phù hợp và sẽ không được bán hoặc chuyển giao cho bất kỳ bên thứ ba nào.",
                _ => string.Empty
            };
        }
    }
}