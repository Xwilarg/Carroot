using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GlobalGameJam2023.Player
{
    public class Ghost : MonoBehaviour
    {
        public void LoadData(IEnumerable<Coordinate> coordinates)
        {
            _coordinates = coordinates.ToArray();
        }

        public void StartGhost()
        {
            _refTimer = Time.unscaledTime;
            _didStart = true;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_didStart)
            {
                var targetTime = Time.unscaledTime - _refTimer;
                var last = _coordinates[0];
                for (int i = 1; i < _coordinates.Length; i++)
                {
                    var current = _coordinates[i];
                    if (targetTime >= last.TimeSinceStart && targetTime <= current.TimeSinceStart)
                    {
                        var prog = (targetTime - last.TimeSinceStart) / (current.TimeSinceStart - last.TimeSinceStart);
                        transform.position = Vector2.Lerp(last.Position.ToVector2(), current.Position.ToVector2(), prog);
                        _rb.velocity = Vector2.Lerp(last.Velocity.ToVector2(), current.Velocity.ToVector2(), prog);
                    }
                    last = current;
                }
            }
        }

        private Rigidbody2D _rb;

        private Coordinate[] _coordinates;
        private float _refTimer;
        private bool _didStart;
    }
}
