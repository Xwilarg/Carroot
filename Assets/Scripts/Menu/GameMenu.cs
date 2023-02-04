﻿using GlobalGameJam2023.Level;
using GlobalGameJam2023.Persistency;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu Instance { private set; get; }

        [SerializeField]
        private GameObject _pauseMenu, _endGameMenu;

        [SerializeField]
        private TMP_Text _timerText, _bestTimerText;

        private Timer _timer;

        private void Awake()
        {
            Instance = this;
            _timer = GetComponent<Timer>();
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
            _timerText.text = $"{_timer.TimerValue:0.00}";
            var value = Mathf.CeilToInt(_timer.TimerValue * 100f);
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
                    _bestTimerText.text = $"{_timer.TimerValue:0.00}";
                }
            }
            else
            {
                DataManager.Instance.SaveData.LevelData.Add(LevelSelector.TargetLevel, new()
                {
                    Time = value,
                    Replay = replay
                });
                _bestTimerText.text = $"{_timer.TimerValue:0.00}";
            }
            DataManager.Instance.Save();
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            // TODO
        }

        public bool IsGameActive => !_pauseMenu.activeInHierarchy && !DidGameEnded;
        public bool DidGameEnded => _endGameMenu.activeInHierarchy;
    }
}
