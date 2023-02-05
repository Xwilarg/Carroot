using DG.Tweening;
using GlobalGameJam2023.Ability;
using GlobalGameJam2023.Boss;
using GlobalGameJam2023.Level;
using GlobalGameJam2023.Menu;
using GlobalGameJam2023.SO;
using GlobalGameJam2023.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Player
{
    public delegate void PlayerControllerEventHandler(PlayerController sender);

    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { private set; get; }

        [SerializeField]
        private PlayerInfo _info;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] LayerMask colisionRaycastFoot;
        [SerializeField] private float distanceRaycast = 0.5f;

        // Controls info
        private Vector2 _mov;
        private int _canGoUp;

        // Components
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Animator _anim;

        // Abilities management
        private float[] _canUseAbility = new float[3] { 0f, 0f, 0f };
        private int[] _abilityLeft;
        private float[] _canUseAbilityMax;
        private readonly List<GameObject> _lastLiana = new();

        // Ghost
        private List<Coordinate> _coordinates = new();
        private float _timeRef;

        public Ghost Ghost { set; private get; }

        [SerializeField]
        private GameObject _powerupBoss;

        private float _baseGravityScale;
        private Vector3 initScale;

        private void Awake()
        {
            Instance = this;
            _rb = GetComponent<Rigidbody2D>();
            _baseGravityScale = _rb.gravityScale;
            _sr = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _canUseAbilityMax = new[] { _info.AbilityOne, _info.AbilityTwo, _info.AbilityBoss }.Select(x => x.ReloadTime).ToArray();
            _abilityLeft = new[] { _info.Levels[LevelSelector.TargetLevel - 1].NumberSkillTeleportation, _info.Levels[LevelSelector.TargetLevel - 1].NumberSkillLiana, 0 };
            for (int i = 0; i < _abilityLeft.Length; i++)
            {
                GameMenu.Instance.SetSkillLeft(i, _abilityLeft[i]);
            }
        }

        public void StartGame()
        {
            _timeRef = Time.unscaledTime;
            if (Ghost != null)
            {
                Ghost.StartGhost();
            }
            Timer.Instance.IsPlayerReady = true;
        }

        private void FixedUpdate()
        {
            if (GameMenu.Instance.DidGameEnded || !Timer.Instance.IsPlayerReady) // Game ended, ignore all inputs
            {
                return;
            }
            _rb.gravityScale = _canGoUp > 0 ? 0f : _baseGravityScale;
            _rb.velocity = new Vector2(
                x: _mov.x * _info.Speed * Time.fixedDeltaTime,
                y: _canGoUp > 0 ? _info.ClimbingSpeed * Time.fixedDeltaTime * _mov.y : _rb.velocity.y // Attempt to climb a liana if it's possible
            );
            
            _coordinates.Add(new()
            {
                TimeSinceStart = Time.unscaledTime - _timeRef,
                Position = new() { X = transform.position.x, Y = transform.position.y },
                Velocity = new() { X = _rb.velocity.x, Y = _rb.velocity.y },
            });
        }

        private IEnumerator WaitCoroutine(Action<CollisionEventArgs> callBack, float seconds, CollisionEventArgs e = null)
        {
            yield return new WaitForSeconds(seconds);
            callBack(e);
            yield return null;
        }

        private void DoTP(CollisionEventArgs e)
        {
            initScale = transform.lossyScale;
            float duration = 0.2f;

            transform.DOScale(0f, duration);
            transform.DORotate(Vector3.one * 360f, duration).OnComplete(() => {
                transform.position = e.GameObjectPosition;
                transform.DORotate(Vector3.zero, 0.2f);
                transform.DOScale(initScale, 0.2f);
            });

        }

        private void Update()
        {
            for (int i = 0; i < _canUseAbility.Length; i++)
            {
                if (_abilityLeft[i] > 0)
                {
                    _canUseAbility[i] -= Time.deltaTime;
                    GameMenu.Instance.SetSkillCooldown(i, Mathf.Clamp01(_canUseAbility[i] / _canUseAbilityMax[i]));
                }
            }
        }

        public void Win()
        {
            _rb.velocity = Vector2.zero;
            GameMenu.Instance.EndGame(_coordinates);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Liana"))
            {
                _canGoUp++;
            }
            else if (collision.CompareTag("FinishLine"))
            {
                Win();
            }
            else if (collision.CompareTag("Powerup"))
            {
                var powerup = collision.GetComponent<WaitAndRespawn>();
                if (powerup.IsActive)
                {
                    _abilityLeft[2]++;
                    GameMenu.Instance.SetSkillLeft(2, _abilityLeft[2]);
                    StartCoroutine(powerup.WaitAndRespawnEnum());
                }
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
                _canGoUp--;
            }
            else if (collision.CompareTag("MovingPlatform"))
            {
                transform.parent = null;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("MovingPlatform"))
            {
                transform.parent = null;
            }
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
            rb.AddTorque(UnityEngine.Random.Range(10f, 100f));
            go.GetComponent<Projectile>().OnCollision += (_, e) =>
            {
                switch (info.Type)
                {
                    case AbilityType.TELEPORT:
                        //transform.position = e.GameObjectPosition; // We just teleport the player at the projectile position
                        StartCoroutine(WaitCoroutine(DoTP, 0f, e));
                        break;

                    case AbilityType.DEPLOY_LIANA:
                        foreach (var liana in _lastLiana) // Remove all old instances of the previous liana
                        {
                            Destroy(liana);
                        }
                        _lastLiana.Clear();
                        var down = Vector2.down;
                        var ignoreLayer = (1 << LayerMask.NameToLayer("Player"));
                        ignoreLayer |= (1 << LayerMask.NameToLayer("Projectile"));
                        ignoreLayer |= (1 << LayerMask.NameToLayer("Rabbit"));
                        ignoreLayer |= (1 << LayerMask.NameToLayer("RabbitRadar"));
                        ignoreLayer = ~ignoreLayer;
                        while (!Physics2D.OverlapCircle(e.Position + down, .1f, ignoreLayer)) // As long as we can spawn liana we do so
                        {
                            _lastLiana.Add(Instantiate(info.PrefabSpe, e.Position + down + Vector2.up * .5f, Quaternion.identity));
                            down += Vector2.down;
                        }
                        break;

                    case AbilityType.BOSS:
                        break;

                    default: throw new NotImplementedException();
                }
            };
            Destroy(go, info.TimeBeforeDisappear);
            _canUseAbility[index] = _canUseAbilityMax[index];
            _abilityLeft[index]--;
            GameMenu.Instance.SetSkillLeft(index, _abilityLeft[index]);
            if (_abilityLeft[index] == 0)
            {
                GameMenu.Instance.SetSkillCooldown(index, 0f);
            }
        }

        private void Death()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #region Input System
        public void Move(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;

            bool isMoving = _mov.x != 0;

            if (isMoving)
            {
                AudioSystem.Instance.PlayFootstep();
            }
            else 
            {
                AudioSystem.Instance.StopFootstep();
            }

            _anim.SetBool("IsMoving", isMoving);
            
            // Flip sprite depending of where we are going
            if (_mov.x > 0f)
            {
                _sr.flipX = false;
            }
            else if (_mov.x < 0f)
            {
                _sr.flipX = true;
            }
        }

        public void AbilityOne(InputAction.CallbackContext value)
        {
            if (value.performed && _canUseAbility[0] <= 0f && Timer.Instance.IsPlayerReady && _abilityLeft[0] > 0)
            {
                FireProjectile(_info.AbilityOne, 0);
            }
        }

        public void AbilityTwo(InputAction.CallbackContext value)
        {
            if (value.performed && _canUseAbility[1] <= 0f && Timer.Instance.IsPlayerReady && _abilityLeft[1] > 0)
            {
                FireProjectile(_info.AbilityTwo, 1);
            }
        }

        public void AbilityThree(InputAction.CallbackContext value)
        {
            if (value.performed && _canUseAbility[2] <= 0f && Timer.Instance.IsPlayerReady && _abilityLeft[2] > 0)
            {
                FireProjectile(_info.AbilityBoss, 2);
            }
        }

        public void Pause(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                GameMenu.Instance.TogglePause();
            }
        }

        public void Restart(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        #endregion
    }
}
