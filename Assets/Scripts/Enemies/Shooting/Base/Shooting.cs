using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [HideInInspector]
    public int shotsFired = 0;

    public GameObject projectilePrefab;

    public float projectileSpeed = 600f;

    public float reloadTime = 1f;
    protected float timeToShoot = 1f;

    public float startingTimeOffset = 1f;

    private void Start()
    {
        timeToShoot = startingTimeOffset;
    }

    protected void UpdateParameters()
    { 
        timeToShoot -= Time.deltaTime;
        if (timeToShoot < 0f)
            timeToShoot = 0f;
    }

    protected bool CanShoot()
    {
        if (timeToShoot > 0f || GameManager.Instance.GetPlayerLost() || PlayerReachable() == false)
            return false;

        return true;
    }

    protected bool PlayerReachable()
    {
        Vector3 distanceToTarget = GameManager.Instance.player.transform.position - transform.position;

        if (distanceToTarget.x > 18f || distanceToTarget.x < -18f)
            return false;

        return true;
    }

    protected Rigidbody SpawnProjectile(Vector3 projectileDirection)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectileDirection * projectileSpeed);

        GameManager.Instance.enemyProjectiles.Add(projectile);

        return rb;
    }

    protected void ShootingDone()
    {
        shotsFired++;
        timeToShoot = reloadTime;
    }

    protected virtual void Shoot()
    {
        ShootingDone();
    }
}
