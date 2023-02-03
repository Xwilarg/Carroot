using GlobalGameJam2023.Ability;
using GlobalGameJam2023.SO;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2023.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private float _movX;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        private bool[] _canUseAbility = new bool[2] { true, true };

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_movX * _info.Speed * Time.fixedDeltaTime, _rb.velocity.y);
        }

        /// <summary>
        /// Wait a specific time and allow to use an ability
        /// </summary>
        /// <param name="info">Scriptable object of the ability</param>
        /// <param name="index">Array index of the ability</param>
        /// <returns>IEnumerator used for coroutine</returns>
        public IEnumerator ReloadAbility(AbilityInfo info, int index)
        {
            yield return new WaitForSeconds(info.ReloadTime);
            _canUseAbility[index] = true;
        }

        /// <summary>
        /// Throw a projectile, for when an ability is used
        /// </summary>
        /// <param name="info">Scriptable object of the ability</param>
        /// <param name="index">Array index of the ability</param>
        private void FireProjectile(AbilityInfo info, int index)
        {
            var go = Instantiate(info.Prefab, transform.position, Quaternion.identity);
            var rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(info.ThrowDirection.normalized * info.ThrowForce * new Vector2(_sr.flipX ? -1f : 1f, 1f), ForceMode2D.Impulse);
            go.GetComponent<Projectile>().OnCollision += (_, e) =>
            {
                switch (info.Type)
                {
                    case AbilityType.TELEPORT:
                        transform.position = e.Position;
                        break;

                    default: throw new NotImplementedException();
                }
            };
            Destroy(go, info.TimeBeforeDisappear);
            StartCoroutine(ReloadAbility(_info.AbilityOne, index));
        }

        #region Input System
        public void Move(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;

            // Flip sprite depending of where we are going
            if (_movX > 0f)
            {
                _sr.flipX = false;
            }
            else if (_movX < 0f)
            {
                _sr.flipX = true;
            }
        }

        public void AbilityOne(InputAction.CallbackContext value)
        {
            if (value.performed && _canUseAbility[0])
            {
                FireProjectile(_info.AbilityOne, 0);
            }
        }
        #endregion
    }
}
