using GlobalGameJam2023.Ability;
using GlobalGameJam2023.Menu;
using GlobalGameJam2023.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2023.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        // Controls info
        private float _movX;
        private bool _isTryingToGoUp;
        private bool _canGoUp;

        // Components
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        // Abilities management
        private bool[] _canUseAbility = new bool[2] { true, true };
        private readonly List<GameObject> _lastLiana = new();

        // Ghost
        private List<Coordinate> _coordinates = new();
        private float _timeRef;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _timeRef = Time.unscaledTime; // TODO: Need to be moved to where the race really start
        }

        private void FixedUpdate()
        {
            if (GameMenu.Instance.DidGameEnded) // Game ended, ignore all inputs
            {
                return;
            }
            _rb.gravityScale = _canGoUp ? 0f : 1f;
            _rb.velocity = new Vector2(
                x: _movX * _info.Speed * Time.fixedDeltaTime,
                y: _canGoUp && _isTryingToGoUp ? _info.ClimbingSpeed * Time.fixedDeltaTime : _rb.velocity.y // Attempt to climb a liana if it's possible
            );
            _coordinates.Add(new()
            {
                TimeSinceStart = Time.unscaledTime - _timeRef,
                Position = new() { X = transform.position.x, Y = transform.position.y },
                Velocity = new() { X = _rb.velocity.x, Y = _rb.velocity.y },
            });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Liana"))
            {
                _canGoUp = true;
            }
            else if (collision.CompareTag("FinishLine"))
            {
                GameMenu.Instance.EndGame(_coordinates);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Trap"))
            {
                Death();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Liana"))
            {
                _canGoUp = false;
            }
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
                        transform.position = e.Position; // We just teleport the player at the impact position
                        break;

                    case AbilityType.DEPLOY_LIANA:
                        foreach (var liana in _lastLiana) // Remove all old instances of the previous liana
                        {
                            Destroy(liana);
                        }
                        _lastLiana.Clear();
                        var down = Vector2.down;
                        while (!Physics2D.OverlapCircle(e.Position + down, .5f, 1 << LayerMask.GetMask("Player"))) // As long as we can spawn liana we do so
                        {
                            _lastLiana.Add(Instantiate(info.PrefabSpe, e.Position + down + Vector2.up * .5f, Quaternion.identity));
                            down += Vector2.down;
                        }
                        break;

                    default: throw new NotImplementedException();
                }
            };
            Destroy(go, info.TimeBeforeDisappear);
            StartCoroutine(ReloadAbility(_info.AbilityOne, index));
        }

        private void Death()
        {
            
        }

        #region Input System
        public void Move(InputAction.CallbackContext value)
        {
            var mov = value.ReadValue<Vector2>();
            _movX = mov.x;
            _isTryingToGoUp = mov.y > 0f;

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

        public void AbilityTwo(InputAction.CallbackContext value)
        {
            if (value.performed && _canUseAbility[1])
            {
                FireProjectile(_info.AbilityTwo, 1);
            }
        }

        public void Pause(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                GameMenu.Instance.TogglePause();
            }
        }
        #endregion
    }
}
