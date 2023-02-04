using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace GlobalGameJam2023.Persistency
{
    public class DataManager
    {
        private readonly string _key = "凛と「戦え」 胸に掲げ　燐と「魂」燦け「愛」の弾丸鳴らし「歌え」ば今日が「始まる」笑っていこうよ！ね☆";

        private string Encrypt(string s)
        {
            StringBuilder str = new();
            for (var i = 0; i < s.Length; i++)
            {
                str.Append((char)(s[i] ^ _key[i % _key.Length]));
            }
            return str.ToString();
        }

        private static DataManager _instance;
        public static DataManager Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }

        private SaveData _saveData;
        public SaveData SaveData
        {
            get
            {
                if (_saveData == null)
                {
                    if (File.Exists($"{Application.persistentDataPath}/save.bin"))
                    {
                        _saveData = JsonConvert.DeserializeObject<SaveData>(Encrypt(File.ReadAllText($"{Application.persistentDataPath}/save.bin")));
                    }
                    else
                    {
                        _saveData = new();
                    }
                }
                return _saveData;
            }
        }

        public void Save()
        {
            File.WriteAllText($"{Application.persistentDataPath}/save.bin", Encrypt(JsonConvert.SerializeObject(_saveData)));
        }

        public void DeleteSaveFolder()
        {
            File.Delete("save.bin");
            // TODO: popup or smth
        }

        public void OpenSaveFolder() // Won't work in WebGL!
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.persistentDataPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
