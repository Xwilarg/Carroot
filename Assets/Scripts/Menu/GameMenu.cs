using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu Instance { private set; get; }

        [SerializeField]
        private GameObject _pauseMenu;

        private void Awake()
        {
            Instance = this;
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

        public void LoadNextLevel()
        {
            // TODO
        }

        public bool IsGameActive => !_pauseMenu.activeInHierarchy;
    }
}
