using UnityEngine;

namespace GlobalGameJam2023.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Tooltip("Speed of the player")]
        public float Speed;

        [Tooltip("Climbing speed of the player when trying to get up a liana")]
        public float ClimbingSpeed;

        [Tooltip("Abilities of the player")]
        public AbilityInfo AbilityOne, AbilityTwo;

        [Tooltip("when the player reaches a certain height in it he dies")]
        public float maxYBeforeDeath = 50f;

        public LevelInfo[] Levels;
    }
}