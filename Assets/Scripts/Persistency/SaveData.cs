using System.Collections.Generic;

namespace GlobalGameJam2023.Persistency
{
    public class SaveData
    {
        public string Language { set; get; } = "english";
        public Dictionary<int, LevelData> LevelData { set; get; } = new();
        public float SoundVolume { set; get; } = 1f;
        public float MusicVolume { set; get; } = 1f;
    }
}
