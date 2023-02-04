using GlobalGameJam2023.Persistency;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _quitButton, _locateSaveButton;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) // We can't quit a WebGL game
            {
                _quitButton.SetActive(false);
                _locateSaveButton.SetActive(false);
            }
        }

        public void LaunchGame()
        {
            SceneManager.LoadScene("LevelSelector");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void LocateSave()
        {
            DataManager.Instance.OpenSaveFolder();
        }

        public void DeleteSave()
        {
            DataManager.Instance.DeleteSaveFolder();
        }
    }
}
