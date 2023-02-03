using UnityEngine;

namespace GlobalGameJam2023.Ability
{
    public class CollisionEventArgs
    {
        public CollisionEventArgs(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; set; }
    }
}
