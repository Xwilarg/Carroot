using GlobalGameJam2023.Player;

namespace GlobalGameJam2023
{
    public class Coordinate
    {
        public CleanVector2 Position { set; get; }
        public float TimeSinceStart { set; get; }
        public CleanVector2 Velocity { set; get; }
    }
}