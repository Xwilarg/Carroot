using GlobalGameJam2023.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject spawn;

    void Start()
    {
        GameObject.FindGameObjectsWithTag("Spawn");
        PlayerController.death += PlayerController_death;
    }

    private void PlayerController_death(PlayerController sender)
    {
        
    }
}
