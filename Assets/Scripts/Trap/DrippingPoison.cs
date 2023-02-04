using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023.Level
{
    public class DrippingPoison : MonoBehaviour
    {
        [SerializeField] private float timeSpawnPoision = 0.5f;
        [SerializeField] private GameObject prefabPoision;
        [SerializeField] private Transform leftPointSpawn;
        [SerializeField] private Transform righPointSpawn;

        private float elpasedTime = 0f;

        void Update()
        {
            elpasedTime += Time.deltaTime;

            if (elpasedTime >= timeSpawnPoision)
            {
                elpasedTime = 0f;
                Spawn();
            }
        }

        private void Spawn()
        {
            Instantiate(prefabPoision).transform.position = new Vector3(Random.Range(leftPointSpawn.position.x, righPointSpawn.position.x),leftPointSpawn.position.y);
        }
    }
}
