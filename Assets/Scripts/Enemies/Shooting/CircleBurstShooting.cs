using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBurstShooting : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float projectileSpeed = 600f;

    public int burstSize = 16;

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

        for (int i = 0; i < burstSize; i++)
        {
            double angle = Math.PI * 2D * (double)i / (double)burstSize;
            Vector3 projectileDirection = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0f);
            projectileDirection.Normalize();

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * projectileSpeed);
            GameManager.Instance.enemyProjectiles.Add(projectile);
        }


        timeToShoot = reloadTime;
    }
}
