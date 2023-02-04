using GlobalGameJam2023.Persistency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023.Translation
{
    public class Translate
    {
        private readonly string[] _languages =
        {
            "english",
            "french"
        };

        private Translate()
        {
            foreach (var lang in _languages)
            {
                _translationData.Add(lang, JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>(lang).text));
            }
        }

        private static Translate _instance;
        public static Translate Instance
        {
            private set => _instance = value;
            get
            {
                _instance ??= new Translate();
                return _instance;
            }
        }

        public string Tr(string key, params string[] arguments)
        {
            var langData = _translationData[CurrentLanguage];
            string sentence;
            if (langData.ContainsKey(key))
            {
                sentence = langData[key];
            }
            else
            {
                sentence = _translationData["english"][key];
            }
            for (int i = 0; i < arguments.Length; i++)
            {
                sentence = sentence.Replace("{" + i + "}", arguments[i]);
            }
            return sentence;
        }

        private string _currentLanguage = null;
        public string CurrentLanguage
        {
            set
            {
                if (!_translationData.ContainsKey(value))
                {
                    throw new ArgumentException($"Invalid translation key {value}", nameof(value));
                }
                _currentLanguage = value;
                foreach (var tt in UnityEngine.Object.FindObjectsOfType<TMP_TextTranslate>())
                {
                    tt.UpdateText();
                }
                DataManager.Instance.Save();
            }
            get
            {
                return _currentLanguage ??= DataManager.Instance.SaveData.Language;
            }
        }

        private readonly Dictionary<string, Dictionary<string, string>> _translationData = new Dictionary<string, Dictionary<string, string>>();
    }
}