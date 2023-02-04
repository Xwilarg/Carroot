using TMPro;
using UnityEngine;

namespace GlobalGameJam2023.Persistency
{
    public class ShowBestTime : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _target;

        [SerializeField]
        private int _levelId;

        private void Awake()
        {
            if (DataManager.Instance.SaveData.LevelData.ContainsKey(_levelId))
            {
                _target.text = $"{DataManager.Instance.SaveData.LevelData[_levelId].Time / 100f:0.00}";
            }
            else
            {
                _target.gameObject.SetActive(false);
            }
        }
    }
}
