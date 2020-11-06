using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace JWLibrary.Utils {

    public class LanguageHandler<T>
        where T : class, new() {

        private static readonly Lazy<LanguageHandler<T>> _instance =
            new Lazy<LanguageHandler<T>>(() => new LanguageHandler<T>());

        private readonly Dictionary<string, string> _keyValues = new Dictionary<string, string>
        {
            {"ko-KR", "./Resource/Lang/ko-KR.json"},
            {"en-US", "./Resource/Lang/en-US.json"}
        };

        private T _languageResource;

        private readonly Dictionary<string, T> _languageResources = new Dictionary<string, T>();

        private LanguageHandler() {
            if (_languageResources.Count <= 0) {
                _languageResources.Add("en-US", LoadLanguageSetting("en-US"));
                _languageResources.Add("ko-KR", LoadLanguageSetting("ko-KR"));
            }
        }

        public static LanguageHandler<T> Instance => _instance.Value;

        public T LanguageResource {
            get {
                if (_languageResource == null)
                    _languageResource = _languageResources[Thread.CurrentThread.CurrentCulture.Name];

                return _languageResource;
            }
        }

        public T this[string lang] {
            get {
                _languageResource = _languageResources[lang];
                return _languageResource;
            }
        }

        private T LoadLanguageSetting(string language) {
            var resourceJson = string.Empty;
            var keyValue = _keyValues.Where(m => m.Key == language).FirstOrDefault();
            T langRes = null;

            if (string.IsNullOrEmpty(keyValue.Key)) keyValue = _keyValues.Where(m => m.Key == "en-US").First();

            resourceJson = File.ReadAllText(keyValue.Value);

            langRes = JsonConvert.DeserializeObject<T>(resourceJson);

            var numberFormatInfo = CultureInfo.CreateSpecificCulture(language).NumberFormat;
            var cultureInfo = new CultureInfo(language);
            cultureInfo.NumberFormat = numberFormatInfo;

            if (language == "ko-KR") {
                cultureInfo.DateTimeFormat.DateSeparator = "-";
                cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            } else {
                cultureInfo.DateTimeFormat.DateSeparator = "/";
                cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            }

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            return langRes;
        }
    }
}