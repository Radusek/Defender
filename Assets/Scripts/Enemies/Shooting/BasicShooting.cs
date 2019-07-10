using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooting : Shooting
{
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
        Vector3 projectileDirection = Vector3.Normalize(targetPos - transform.position);

        SpawnProjectile(projectileDirection);

        base.Shoot();
    }
}
