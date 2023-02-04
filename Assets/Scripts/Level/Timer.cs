using GlobalGameJam2023.Menu;
using TMPro;
using UnityEngine;

namespace GlobalGameJam2023.Level
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timerText;

        public float TimerValue { private set; get; }

        private void Update()
        {
            if (GameMenu.Instance.IsGameActive)
            {
                TimerValue += Time.deltaTime;
                _timerText.text = $"{TimerValue:0.00}";
            }
        }
    }
}
