using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023.Level
{
    public class PlatformMobile : MonoBehaviour
    {
        [SerializeField] private List<Transform> allPoint;
        [SerializeField] private float speed = 3f;

        private int index = 0;
        private int factor = 1;

        private void Start()
        {
            index++;
        }

        private void Update()
        {
            if (allPoint.Count <= 0)
                return;

            transform.position = Vector3.MoveTowards(transform.position,allPoint[index].position, speed * Time.deltaTime);

            if (transform.position == allPoint[index].position)
            {
                if (index == 0 || index >= allPoint.Count - 1)
                    factor *= -1;

                index += factor;
            }
        }
    }
}
