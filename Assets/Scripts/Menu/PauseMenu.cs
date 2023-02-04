using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance { private set; get; }

        [SerializeField]
        private GameObject _pauseMenu;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void TogglePause()
        {
            _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
            Time.timeScale = IsGameActive ? 1f : 0f;
        }

        public bool IsGameActive => !_pauseMenu.activeInHierarchy;
    }
}
