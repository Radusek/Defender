using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpendicularShot : AutoDestroyBase
{
    public float sendProjectilesPower = 200f;

    private static int count = 0;

    public GameObject projectilePrefab;

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
            SendProjectiles();
    }

    private void SendProjectiles()
    {
        if (++count % 3 == 0)     
            for (int i = 0; i < 2; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);

                Vector3 addedVelocity = (i == 0 ? Vector3.up : Vector3.down) * sendProjectilesPower;
                projectile.GetComponent<Rigidbody>().velocity += addedVelocity;

                GameManager.Instance.enemyProjectiles.Add(projectile);
            }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SendProjectiles();
        Destroy(gameObject);
    }
}
