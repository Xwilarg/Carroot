using UnityEngine;

namespace GlobalGameJam2023.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/RabbitInfo", fileName = "RabbitInfo")]
    public class RabbitInfo : ScriptableObject
    {
        public float Speed;
        public float MinDistance;
    }
}