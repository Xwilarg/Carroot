using UnityEngine;

namespace GlobalGameJam2023.Player
{
    public class CleanVector2
    {
        public float X { set; get; }
        public float Y { set; get; }

        public Vector2 ToVector2() => new(X, Y);
    }
}
