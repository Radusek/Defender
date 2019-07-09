using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlayerShooting : PlayerShooting
{
    // Update is called once per frame
    void Update()
    {
        base.UpdateParameters();
        Shoot();
    }

    private void Shoot()
    {
        if (base.CanShoot() == false)
            return;

        for (int i = 0; i < 2; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + i * 0.2f * Vector3.down, chosenRotation);

            if (movement.stopped == false)
                projectile.GetComponent<Rigidbody>().AddForce(new Vector3(projectileDirection * 1000f, 0f, 0f));
        }


        timeToShoot = reloadTime;
    }
}
