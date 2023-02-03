using UnityEngine;

namespace GlobalGameJam2023.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Tooltip("Speed of the player")]
        public float Speed;

        public AbilityInfo AbilityOne, AbilityTwo;
    }
}