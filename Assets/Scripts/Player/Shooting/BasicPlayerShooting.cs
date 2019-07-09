using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerShooting : PlayerShooting
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
        
        GameObject projectile = Instantiate(projectilePrefab, transform.position, chosenRotation);

        if (movement.stopped == false)
            projectile.GetComponent<Rigidbody>().AddForce(new Vector3(projectileDirection * 1000f, 0f, 0f));

        timeToShoot = reloadTime;
    }
}
