using UnityEngine;

namespace GlobalGameJam2023.NPC
{
    public class RabbitRadar : MonoBehaviour
    {
        private Rabbit _rabbit;

        private void Awake()
        {
            _rabbit = transform.parent.GetComponent<Rabbit>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _rabbit.Target = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _rabbit.Target = null;
            }
        }
    }
}
