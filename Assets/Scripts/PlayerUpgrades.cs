using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerUpgrades : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject projectilePrefab;

    public GameObject perpendicularProjectilePrefab;

    private AutoDestroyBase projectileStats;

    // base = 0.35f
    private float baseProjectileSize = 0.35f;
    private float biggerProjectileSize = 0.55f;

    // base = 0.3f
    private float baseMissileLifetime = 0.3f;
    private float extendedMissileLifetime = 0.38f;

    private int shootingLevel = 1;



    private bool canDash = false;

    private Rigidbody rb;
    private float dashForce = 1500f;
    private float dashDuration = 0.3f;
    private float dashCooldown = 5f;
    private float timeToDash = 0;
    private float lastDashDirection = 0f;
    bool stoppedDash = true;



    private bool canSetMines = false;

    public GameObject minePrefab;
    private float mineCooldown = 7f;
    private float timeToSetMine = 0f;



    private bool canReflect = false;

    private float reflectCooldown = 15f;
    private float timeToReflect = 0f;




    private bool canSlowTime = false;

    private float slowMultiplier = 0.5f;
    private float slowDuration = 1.5f;
    private float slowCooldown = 12f;
    private float timeToSlow = 0f;
    bool slowTimeSpeedReduced = true;



    private bool canBeImmortal = false;

    private float immortalDuration = 1.5f;
    private float immortalCooldown = 15f;
    private float timeToImmortal = 0f;
    public GameObject immortalEffect;



    private void Awake()
    {
        projectileStats = projectilePrefab.GetComponent<AutoDestroy>();
        SetUpgrades();
        rb = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
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

        // ############################################## ABILITIES ##############################################

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.Dash])
        {
            canDash = true;
            stoppedDash = true;
        }

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.Mine])
            canSetMines = true;

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.Immortal])
        {
            canBeImmortal = true;
            immortalEffect.SetActive(false);
        }

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.TimeSlow])
        {
            slowTimeSpeedReduced = true;
            canSlowTime = true;
        }

        if (PlayerUpgradesManager.Instance.upgradeBought[(int)PlayerUpgradesManager.Upgrade.Reflection])
            canReflect = true;

    }


    private void Update()
    {
        UpdateCooldowns();

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

    void UpdateCooldowns()
    {
        timeToDash -= Time.deltaTime;
        if (timeToDash <= dashCooldown - dashDuration && stoppedDash == false)
        {
            rb.AddForce(-lastDashDirection, 0f, 0f);
            stoppedDash = true;
        }

        if (timeToDash < 0f)
            timeToDash = 0f;

        PlayerUpgradesManager.Instance.SetCooldownSlider(timeToDash / dashCooldown, (int)PlayerUpgradesManager.Upgrade.Dash);



        timeToSlow -= Time.deltaTime;
        if (timeToSlow <= slowCooldown - slowDuration && slowTimeSpeedReduced == false)
        {
            playerMovement.flightSpeed /= 2;

            PlayerShooting shooting = null;
            if (shootingLevel % 2 == 1)
            {
                shooting = gameObject.GetComponent<BasicPlayerShooting>();
                shooting.reloadTime *= 2;
            }
            if (shootingLevel % 2 == 0)
            {
                shooting = gameObject.GetComponent<DoublePlayerShooting>();
                shooting.reloadTime *= 2;
            }

            Time.timeScale = 1f;

            slowTimeSpeedReduced = true;
        }


        if (timeToSlow < 0f)
            timeToSlow = 0f;

        PlayerUpgradesManager.Instance.SetCooldownSlider(timeToSlow / slowCooldown, (int)PlayerUpgradesManager.Upgrade.TimeSlow);



        timeToImmortal -= Time.deltaTime;
        if (timeToImmortal <= immortalCooldown - immortalDuration)
        {
            immortalEffect.SetActive(false);
            gameObject.layer = 9; // PLAYER
        }

        if (timeToImmortal < 0f)
            timeToImmortal = 0f;

        PlayerUpgradesManager.Instance.SetCooldownSlider(timeToImmortal / immortalCooldown, (int)PlayerUpgradesManager.Upgrade.Immortal);




        timeToReflect -= Time.deltaTime;

        if (timeToReflect < 0f)
            timeToReflect = 0f;

        PlayerUpgradesManager.Instance.SetCooldownSlider(timeToReflect / reflectCooldown, (int)PlayerUpgradesManager.Upgrade.Reflection);



        timeToSetMine -= Time.deltaTime;

        if (timeToSetMine < 0f)
            timeToSetMine = 0f;

        PlayerUpgradesManager.Instance.SetCooldownSlider(timeToSetMine / mineCooldown, (int)PlayerUpgradesManager.Upgrade.Mine);

    }


    private void SetMine()
    {
        if (timeToSetMine > 0f)
            return;

        Instantiate(minePrefab, gameObject.transform.position, Quaternion.identity);

        timeToSetMine = mineCooldown;
    }

    private void Dash()
    {
        if (timeToDash > 0f)
            return;

        lastDashDirection = dashForce * (playerMovement.directionRight ? 1 : -1);
        rb.AddForce(lastDashDirection, 0f, 0f);
        stoppedDash = false;
        timeToDash = dashCooldown;
    }

    private void BecomeImmortal()
    {
        if (timeToImmortal > 0f)
            return;

        immortalEffect.SetActive(true);
        gameObject.layer = 14; // IMMORTAL
        timeToImmortal = immortalCooldown;
    }

    private void Reflect()
    {
        if (timeToReflect > 0f)
            return;

        GameManager.Instance.ReflectEnemyProjectiles();

        timeToReflect = reflectCooldown;
    }

    private void SlowTime()
    {
        if (timeToSlow > 0f)
            return;

        playerMovement.flightSpeed *= 2;
        slowTimeSpeedReduced = false;

        PlayerShooting shooting = null;
        if (shootingLevel % 2 == 1)
        {
            shooting = gameObject.GetComponent<BasicPlayerShooting>();
            shooting.reloadTime /= 2;
        }
        if (shootingLevel > 1)
        {
            shooting = gameObject.GetComponent<DoublePlayerShooting>();
            shooting.reloadTime /= 2;
        }

        Time.timeScale = slowMultiplier;
        timeToSlow = slowCooldown;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}

