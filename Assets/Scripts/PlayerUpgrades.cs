using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject projectilePrefab;

    public GameObject perpendicularProjectilePrefab;

    private AutoDestroyBase projectileStats;

    // base = 0.35f
    private float baseProjectileSize = 0.35f;
    private float biggerProjectileSize = 0.45f;

    // base = 0.3f
    private float baseMissileLifetime = 0.3f;
    private float extendedMissileLifetime = 0.38f;

    private int shootingLevel = 1;

    private bool canDash = false;
    private bool canSetMines = false;
    private bool canReflect = false;
    private bool canSlowTime = false;
    private bool canBeImmortal = false;

    private void Awake()
    {
        projectileStats = projectilePrefab.GetComponent<AutoDestroy>();
        SetUpgrades();
    }

    public void SetUpgrades()
    {
        PlayerShooting shooting = null;


        // ############################################## MISSILES STATS ##############################################

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.Perpendicular])
        {
            projectilePrefab = perpendicularProjectilePrefab;
            projectileStats = projectilePrefab.GetComponent<PerpendicularShot>();
        }

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.MissileRange])
            projectileStats.lifeTime = extendedMissileLifetime;
        else
            projectileStats.lifeTime = baseMissileLifetime;

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.MissileSize])
            projectilePrefab.transform.localScale = Vector3.one * biggerProjectileSize;
        else
            projectilePrefab.transform.localScale = Vector3.one * baseProjectileSize;


        if (shootingLevel % 2 == 1)
            gameObject.GetComponent<BasicPlayerShooting>().projectilePrefab = projectilePrefab;
        if (shootingLevel % 2 == 0)
            gameObject.GetComponent<DoublePlayerShooting>().projectilePrefab = projectilePrefab;

        // ############################################## MISSILES COUNT ##############################################

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.TripleMissile])
        {
            if (shootingLevel == 1)
                shooting = gameObject.AddComponent<DoublePlayerShooting>();    
            else if (shootingLevel == 2)
                shooting  = gameObject.AddComponent<BasicPlayerShooting>();

            shooting.movement = playerMovement;
            shooting.projectilePrefab = projectilePrefab;

            shootingLevel = 3;
        }
        else if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.DoubleMissile])
        {
            if (shootingLevel == 1)
                shooting = gameObject.AddComponent<DoublePlayerShooting>();

            shooting.movement = playerMovement;
            shooting.projectilePrefab = projectilePrefab;

            Destroy(GetComponent<BasicPlayerShooting>());

            shootingLevel = 2;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canSetMines)
            SetMine();

        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash)
            Dash();

        if (Input.GetKeyDown(KeyCode.C) && canBeImmortal)
            BecomeImmortal();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canReflect)
            Reflect();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlowTime)
            SlowTime();
    }


    private void SetMine()
    {

    }

    private void Dash()
    {

    }

    private void BecomeImmortal()
    {

    }

    private void Reflect()
    {

    }

    private void SlowTime()
    {

    }
}

