using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlayerShooting : PlayerShooting
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

        if (shootingType == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position + i * 0.2f * Vector3.down, chosenRotation);

                projectile.GetComponent<Rigidbody>().AddForce(new Vector3(
                    movement.stopped == false ? projectileDirection * 500f : 0f,
                    i == 0 ? 200f : -200f,
                    0f)
                );
            }

            timeToShoot = reloadTime;
        }
        else if (shootingType == 2)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject projectile = Instantiate(projectilePrefab, transform.position + i * 0.2f * Vector3.down + 2*j * Vector3.right, chosenRotation);

                    projectile.GetComponent<Rigidbody>().AddForce(new Vector3(
                        movement.stopped == false ? projectileDirection * 500f : 0f,
                        i == 0 ? 200f : -200f,
                        0f)
                    );
                }
            }

            timeToShoot = 2 * reloadTime;
        }
    }
}
