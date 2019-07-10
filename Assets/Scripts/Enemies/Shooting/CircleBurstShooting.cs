using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBurstShooting : Shooting
{
    public int burstSize = 16;

    // Update is called once per frame
    void Update()
    {
        base.UpdateParameters();

        if (CanShoot())
            Shoot();
    }

    override protected void Shoot()
    {
        Vector3 targetPos = GameManager.Instance.player.transform.position;

        for (int i = 0; i < burstSize; i++)
        {
            float angle = Mathf.PI * 2f * i / burstSize;
            Vector3 projectileDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            projectileDirection.Normalize();

            SpawnProjectile(projectileDirection);
        }

        base.Shoot();
    }
}
