using UnityEngine;

namespace GlobalGameJam2023.Ability
{
    public class CollisionEventArgs
    {
        public CollisionEventArgs(Vector2 position, Vector2 goPosition)
        {
            Position = position;
            GameObjectPosition = goPosition;
        }

        public Vector2 Position { get; set; }
        public Vector2 GameObjectPosition { get; set; }
    }
}
