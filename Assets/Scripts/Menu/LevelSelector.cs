using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public static int TargetLevel { private set; get; } = 1;
        public static int LastLevel => 2;

        public void LoadLevel(int index)
        {
            TargetLevel = index;
            SceneManager.LoadScene("Main");
        }
    }
}
