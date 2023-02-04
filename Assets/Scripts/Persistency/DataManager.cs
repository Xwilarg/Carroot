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
                    if (File.Exists("save.bin"))
                    {
                        _saveData = JsonConvert.DeserializeObject<SaveData>(Encoding.UTF8.GetString(File.ReadAllBytes("save.bin")));
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
            File.WriteAllBytes("save.bin", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_saveData)));
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
