using GlobalGameJam2023.Level;
using GlobalGameJam2023.Persistency;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GlobalGameJam2023.Menu
{
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu Instance { private set; get; }

        [SerializeField]
        private GameObject _pauseMenu, _endGameMenu, _nextLevelButton;

        [SerializeField]
        private TMP_Text _timerText, _bestTimerText;

        [SerializeField]
        private Image[] _skillCooldown;

        [SerializeField]
        private TMP_Text[] _skillLeft;

        public void SetSkillCooldown(int index, float v)
        {
            _skillCooldown[index].rectTransform.sizeDelta = new(_skillCooldown[index].rectTransform.sizeDelta.x, v * 100f);
        }

        public void SetSkillLeft(int index, int value)
        {
            _skillLeft[index].text = $"x{value}";
        }

        private void Awake()
        {
            Instance = this;
            if (LevelSelector.TargetLevel == LevelSelector.LastLevel)
            {
                _nextLevelButton.SetActive(false);
            }
        }

        public void ResumeTimeScale()
        {
            Time.timeScale = 1f;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("LevelSelect");
        }

        public void TogglePause()
        {
            _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
            Time.timeScale = IsGameActive ? 1f : 0f;
        }

        public void EndGame(IEnumerable<Coordinate> replay)
        {
            _endGameMenu.SetActive(true);
            _timerText.text = $"{Timer.Instance.TimerValue:0.00}";
            var value = Mathf.CeilToInt(Timer.Instance.TimerValue * 100f);
            if (DataManager.Instance.SaveData.LevelData.ContainsKey(LevelSelector.TargetLevel))
            {
                var previous = DataManager.Instance.SaveData.LevelData[LevelSelector.TargetLevel].Time;
                if (value < previous)
                {
                    DataManager.Instance.SaveData.LevelData[LevelSelector.TargetLevel] = new()
                    {
                        Time = value,
                        Replay = replay
                    };
                    _bestTimerText.text = $"{previous / 100f:0.00}";
                }
                else
                {
                    _bestTimerText.text = $"{Timer.Instance.TimerValue:0.00}";
                }
            }
            else
            {
                DataManager.Instance.SaveData.LevelData.Add(LevelSelector.TargetLevel, new()
                {
                    Time = value,
                    Replay = replay
                });
                _bestTimerText.text = $"{Timer.Instance.TimerValue:0.00}";
            }
            DataManager.Instance.Save();
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            LevelSelector.TargetLevel++;
            SceneManager.LoadScene("Main");
        }

        public bool IsGameActive => !_pauseMenu.activeInHierarchy && !DidGameEnded;
        public bool DidGameEnded => _endGameMenu.activeInHierarchy;
    }
}
