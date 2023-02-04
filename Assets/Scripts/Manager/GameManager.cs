using GlobalGameJam2023.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject spawn;

    private void Start()
    {
        spawn = GameObject.FindGameObjectsWithTag("Spawn")[0];
        Debug.Log("spawn " + spawn);
        PlayerController.death += PlayerController_death;
    }

    private void PlayerController_death(PlayerController sender)
    {
        Debug.Log(sender);
        Debug.Log(spawn.gameObject.name);
        sender.transform.position = spawn.transform.position;
    }
}
