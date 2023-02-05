using GlobalGameJam2023.Persistency;
using GlobalGameJam2023.Translation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GlobalGameJam2023.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _quitButton, _locateSaveButton;

        [SerializeField]
        private Slider _soundsSlider, _bgmSlider;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) // We can't quit a WebGL game
            {
                _quitButton.SetActive(false);
                _locateSaveButton.SetActive(false);
            }
            _soundsSlider.value = DataManager.Instance.SaveData.SoundVolume;
            _bgmSlider.value = DataManager.Instance.SaveData.MusicVolume;
        }

        public void SetLanguage(string language)
        {
            Translate.Instance.CurrentLanguage = language;
        }

        public void LaunchGame()
        {
            SceneManager.LoadScene("LevelSelect");
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

        public void OnSoundsChange(float value)
        {
            DataManager.Instance.SaveData.SoundVolume = value;
        }

        public void OnBGMChange(float value)
        {
            DataManager.Instance.SaveData.MusicVolume = value;
        }
    }
}
