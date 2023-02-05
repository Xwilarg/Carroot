using GlobalGameJam2023.Persistency;
using UnityEngine;

namespace GlobalGameJam2023.System 
{
    public class AudioSystem : MonoBehaviour {

        [SerializeField] private AudioSource _soundsSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _footstepSource;
        private bool _isFirstFrameFootstep = false;

        public static AudioSystem Instance { get; private set; }
        protected virtual void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _soundsSource.volume = DataManager.Instance.SaveData.SoundVolume;
                _footstepSource.volume = DataManager.Instance.SaveData.SoundVolume;
                _musicSource.volume = DataManager.Instance.SaveData.MusicVolume;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip clip, float vol = 1) 
        {
            _soundsSource.PlayOneShot(clip, vol);
        }

        public void PlayMusic(AudioClip clip) 
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    
        public void PlayFootstep() 
        {
            _footstepSource.Play();
        }

        public void StopFootstep() 
        {
            _footstepSource.Stop();
            _isFirstFrameFootstep = false;
        }
    }
}