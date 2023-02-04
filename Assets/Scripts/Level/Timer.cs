using GlobalGameJam2023.Menu;
using TMPro;
using UnityEngine;

namespace GlobalGameJam2023.Level
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timerText;

        private float _timer;

        private void Update()
        {
            if (GameMenu.Instance.IsGameActive)
            {
                _timer += Time.deltaTime;
                _timerText.text = $"{_timer:0.00}";
            }
        }
    }
}
