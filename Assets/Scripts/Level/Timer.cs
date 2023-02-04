using GlobalGameJam2023.Menu;
using TMPro;
using UnityEngine;

namespace GlobalGameJam2023.Level
{
    public class Timer : MonoBehaviour
    {
        public static Timer Instance { private set; get; }

        [SerializeField]
        private TMP_Text _timerText;

        public float TimerValue { private set; get; }

        public bool IsPlayerReady { set; private get; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (IsPlayerReady && GameMenu.Instance.IsGameActive)
            {
                TimerValue += Time.deltaTime;
                _timerText.text = $"{TimerValue:0.00}";
            }
        }
    }
}
