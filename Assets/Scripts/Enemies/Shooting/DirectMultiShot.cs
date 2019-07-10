using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectMultiShot : Shooting
{
    public int projectilesCount = 2;
    public float projectilesOffset = 0.4f;

    // Update is called once per frame
    void Update()
    {
        base.UpdateParameters();

        if (CanShoot())
            Shoot();
    }

    protected override void Shoot()
    {
        Vector3 targetPos = GameManager.Instance.player.transform.position;
        Vector3 projectileDirection = Vector3.Normalize(targetPos - transform.position);


        for (int i = 0; i < projectilesCount; i++)
        {
            Rigidbody rb = SpawnProjectile(projectileDirection);
            rb.transform.position += projectilesOffset * GetPerpendicularDirection(projectileDirection) * ((float)projectilesCount / 2f - (float)i - 0.5f);
        }

        base.Shoot();      
    }

    private Vector3 GetPerpendicularDirection(Vector3 vector)
    {
        Vector3 perpendicular = Vector3.zero;

        perpendicular.x = vector.y;
        perpendicular.y = -vector.x;

        return Vector3.Normalize(perpendicular);
    }
}
