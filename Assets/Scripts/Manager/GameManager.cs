using GlobalGameJam2023.Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject spawn;

    private void Start()
    {
        spawn = GameObject.FindGameObjectsWithTag("Spawn")[0];
        PlayerController.death += PlayerController_death;
    }

    private void PlayerController_death(PlayerController sender)
    {
        sender.transform.position = spawn.transform.position;
    }
}
