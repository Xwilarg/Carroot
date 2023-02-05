using System;
using UnityEngine;

namespace GlobalGameJam2023.Ability
{
    public class Projectile : MonoBehaviour
    {
        public event EventHandler<CollisionEventArgs> OnCollision;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled) // Platforms still call this methods even when you shoot from below
            {
                OnCollision?.Invoke(this, new(collision.contacts[0].point, transform.position));
                Destroy(gameObject);
            }
        }
    }
}
