using GlobalGameJam2023.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Level
{
    public class LevelLoader : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene($"Level{LevelSelector.TargetLevel}", LoadSceneMode.Additive);
        }
    }
}
