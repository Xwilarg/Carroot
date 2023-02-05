using GlobalGameJam2023.Persistency;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public static int TargetLevel { set; get; } = 1;
        public static int LastLevel => 5;

        [SerializeField]
        private GameObject _bossButton;

        private void Awake()
        {
            for (int i = 0; i < LastLevel - 1; i++)
            {
                if (!DataManager.Instance.SaveData.LevelData.ContainsKey(i + 1) || DataManager.Instance.SaveData.LevelData[i + 1].Time == 0)
                {
                    _bossButton.SetActive(false);
                    return;
                }
            }
        }

        public void LoadLevel(int index)
        {
            TargetLevel = index;
            SceneManager.LoadScene("Main");
        }
    }
}
