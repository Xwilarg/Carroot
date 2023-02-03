using System;
using UnityEngine;

namespace GlobalGameJam2023.Ability
{
    public class Projectile : MonoBehaviour
    {
        public event EventHandler<CollisionEventArgs> OnCollision;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollision?.Invoke(this, new(collision.contacts[0].point));
            Destroy(gameObject);
        }
    }
}
