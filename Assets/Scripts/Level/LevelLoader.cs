using GlobalGameJam2023.Menu;
using GlobalGameJam2023.Persistency;
using GlobalGameJam2023.Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2023.Level
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player, _ghost;

        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene != SceneManager.GetActiveScene())
                {
                    var spawn = GameObject.FindGameObjectsWithTag("Spawn");
                    Assert.AreEqual(1, spawn.Length, $"There should be only 1 spawn point per level by {spawn.Length} were found");
                    var player = Instantiate(_player, spawn[0].transform.position, Quaternion.identity);
                    if (DataManager.Instance.SaveData.LevelData.ContainsKey(LevelSelector.TargetLevel))
                    {
                        var go = Instantiate(_ghost, spawn[0].transform.position, Quaternion.identity);
                        var ghost = go.GetComponent<Ghost>();
                        ghost.LoadData(DataManager.Instance.SaveData.LevelData[LevelSelector.TargetLevel].Replay);
                        player.GetComponent<PlayerController>().Ghost = ghost;
                    }
                }
            };
            SceneManager.LoadScene($"Level{LevelSelector.TargetLevel}", LoadSceneMode.Additive);
        }
    }
}
