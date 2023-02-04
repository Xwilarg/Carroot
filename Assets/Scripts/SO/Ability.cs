using UnityEngine;

namespace GlobalGameJam2023.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/AbilityInfo", fileName = "AbilityInfo")]
    public class AbilityInfo : ScriptableObject
    {
        [Tooltip("GameObject spawned for the projectile")]
        public GameObject Prefab;
        [Tooltip("Additional object that may be spawned depending of the ability")]
        public GameObject PrefabSpe;

        [Tooltip("Time before the ability can be reused (seconds)")]
        public float ReloadTime;
        [Tooltip("Direction the projectile is thrown")]
        public Vector2 ThrowDirection;
        [Tooltip("Force the projectile is thrown with")]
        public float ThrowForce;
        [Tooltip("Time before the profile is automatically deleted")]
        public float TimeBeforeDisappear;
        [Tooltip("What the ability actually does")]
        public AbilityType Type;
    }
}
