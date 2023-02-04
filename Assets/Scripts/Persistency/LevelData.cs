using System.Collections.Generic;

namespace GlobalGameJam2023.Persistency
{
    public class LevelData
    {
        public int Time { set; get; }
        public IEnumerable<Coordinate> Replay { set; get; }
    }
}
