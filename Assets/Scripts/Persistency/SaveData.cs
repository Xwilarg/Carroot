using System.Collections.Generic;

namespace GlobalGameJam2023.Persistency
{
    public class SaveData
    {
        public string Language { set; get; } = "english";
        public Dictionary<string, LevelData> LevelData { set; get; }
    }
}
