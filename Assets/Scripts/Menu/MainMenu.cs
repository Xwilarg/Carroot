using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _quitButton;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                _quitButton.SetActive(false); // We can't quit a WebGL game
            }
        }

        public void LaunchGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
