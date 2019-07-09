using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject projectilePrefab;

    private int shootingLevel = 1;

    private void Awake()
    {
        while (shootingLevel < PlayerUpgradesManager.Instance.playerShootingLevel)
            UpgradeShooting(true);
    }

    public void UpgradeShooting(bool respawning)
    {
        if (shootingLevel > 2)
            return;

        PlayerShooting shooting = null;

        if (shootingLevel == 1)
        {
            Destroy(GetComponent<BasicPlayerShooting>());
            shooting = gameObject.AddComponent<DoublePlayerShooting>();
        }

        if (shootingLevel == 2)
        {
            shooting = gameObject.AddComponent<BasicPlayerShooting>();
        }

        shooting.movement = playerMovement;
        shooting.projectilePrefab = projectilePrefab;

        shootingLevel++;
        if (respawning == false)
            PlayerUpgradesManager.Instance.playerShootingLevel++;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            UpgradeShooting(false);
    }
}
