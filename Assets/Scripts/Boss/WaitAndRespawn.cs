using System.Collections;
using UnityEngine;

namespace GlobalGameJam2023.Boss
{
    public class WaitAndRespawn : MonoBehaviour
    {
        private SpriteRenderer _sr;

        public bool IsActive = true;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public IEnumerator WaitAndRespawnEnum()
        {
            _sr.enabled = false;
            IsActive = false;
            yield return new WaitForSeconds(3f);
            _sr.enabled = true;
            IsActive = true;
        }
    }
}
