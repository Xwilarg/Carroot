using UnityEngine;
using GlobalGameJam2023.Player;
using UnityEngine.UI;

namespace Assets.Scripts.Boss
{
    public class BossNPC : MonoBehaviour
    {
        private int _life = 3;
        private int _maxLife = 3;

        [SerializeField]
        private Image _health;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("ProjectileBoss"))
            {
                _life--;
                _health.rectTransform.anchorMax = new(_life / _maxLife, 1f);
                if (_life == 0)
                {
                    PlayerController.Instance.Win();
                }
            }
        }
    }
}
