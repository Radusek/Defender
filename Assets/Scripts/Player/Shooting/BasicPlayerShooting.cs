using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerShooting : PlayerShooting
{
    // Update is called once per frame
    void Update()
    {
        Shoot(base.UpdateParameters());
    }

    private void Shoot(int shootingType)
    {
        if (shootingType == 0 || base.CanShoot() == false)
            return;
        
        if (shootingType == 1) // Normal shooting
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, chosenRotation);

            if (movement.stopped == false)
                projectile.GetComponent<Rigidbody>().AddForce(new Vector3(projectileDirection * 1000f, 0f, 0f));

            timeToShoot = reloadTime;
        }
        else if (shootingType == 2) // Burst shooting
        {
            for (int i = -1; i <= 1; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position + 2*i * Vector3.right, chosenRotation);

                if (movement.stopped == false)
                    projectile.GetComponent<Rigidbody>().AddForce(new Vector3(projectileDirection * 1000f, 0f, 0f));
            }

            timeToShoot = 2 * reloadTime;
        }
    }
}
