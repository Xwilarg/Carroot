using GlobalGameJam2023.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2023.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Vector2 _mov;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_mov.x * _info.Speed, _rb.velocity.y);
        }

        public void Move(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
        }
    }
}
