using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooting : Shooting
{
    public GameObject projectilePrefab;

    public float projectileSpeed = 600f;

    public float reloadTime = 1f;
    private float timeToShoot = 1f;

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        timeToShoot -= Time.deltaTime;


        if (timeToShoot > 0f || GameManager.Instance.GetPlayerLost())
            return;

        Vector3 targetPos = GameManager.Instance.player.transform.position;

        Vector3 distanceToTarget = targetPos - transform.position;

        if (distanceToTarget.x > 15f || distanceToTarget.x < -15f)
            return;

        Vector3 projectileDirection = distanceToTarget;
        projectileDirection.Normalize();


        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        shotsFired++;
        projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * projectileSpeed);
        GameManager.Instance.enemyProjectiles.Add(projectile);

        timeToShoot = reloadTime;
    }
}
