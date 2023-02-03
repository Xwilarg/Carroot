using UnityEngine;

namespace GlobalGameJam2023.Player
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}
