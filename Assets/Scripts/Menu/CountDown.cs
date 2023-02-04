using GlobalGameJam2023.Player;
using TMPro;
using UnityEngine;

namespace GlobalGameJam2023.Menu
{
    public class CountDown : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _countDown;
        private float _startTimer = 3f;

        private void Update()
        {
            if (_startTimer > 0f)
            {
                _startTimer -= Time.deltaTime;
                _countDown.text = $"{Mathf.FloorToInt(_startTimer)}";
                if (_startTimer <= 0f)
                {
                    PlayerController.Instance.StartGame();
                    Destroy(_countDown.gameObject);
                }
            }
        }
        }
}
