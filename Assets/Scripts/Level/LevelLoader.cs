using GlobalGameJam2023.Menu;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Level
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene != SceneManager.GetActiveScene())
                {
                    var spawn = GameObject.FindGameObjectsWithTag("Spawn");
                    Assert.AreEqual(1, spawn.Length, $"There should be only 1 spawn point per level by {spawn.Length} were found");
                    Instantiate(_player, spawn[0].transform.position, Quaternion.identity);
                }
            };
            SceneManager.LoadScene($"Level{LevelSelector.TargetLevel}", LoadSceneMode.Additive);
        }
    }
}
