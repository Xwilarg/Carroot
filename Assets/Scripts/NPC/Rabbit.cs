﻿using GlobalGameJam2023.SO;
using UnityEngine;

namespace GlobalGameJam2023.NPC
{
    public class Rabbit : MonoBehaviour
    {
        [SerializeField]
        private RabbitInfo _info;

        public GameObject Target { set; private get; }
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (Target != null)
            {
                var x = 0f;
                if (Mathf.Abs(Target.transform.position.x - transform.position.x) > _info.MinDistance)
                {
                    x = transform.position.x < Target.transform.position.x ? 1f : -1f;

                }
                _rb.velocity = new Vector2(x * Time.deltaTime * _info.Speed, _rb.velocity.y);
            }
        }
    }
}