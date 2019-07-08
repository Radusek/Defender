using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ProjectileScript : MonoBehaviour
{
    public int projectilesCount = 3;

    private Rigidbody[] projectiles;

    public float distanceFromCenter = 0.5f;
    public float sendProjectilesPower = 200f;
    public float lifeTime = 2f;

    public GameObject projectilePrefab;

    private float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        projectiles = new Rigidbody[projectilesCount];

        for (int i = 0; i < projectilesCount; i++)
        {
            float tempAngle = 2f * Mathf.PI * (float)i / (float)projectilesCount;
            Vector3 positionOffset = new Vector3(Mathf.Cos(tempAngle), Mathf.Sin(tempAngle), 0f);
            positionOffset *= distanceFromCenter;

            projectiles[i] = Instantiate(projectilePrefab, gameObject.transform.position + positionOffset, Quaternion.identity).GetComponent<Rigidbody>();
            GameManager.Instance.enemyProjectiles.Add(projectiles[i].gameObject);
        }   
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        angle += Time.deltaTime;

        RotateProjectiles();

        if (lifeTime <= 0f)
            SendProjectiles();
    }

    void RotateProjectiles()
    {
        for (int i = 0; i < projectilesCount; i++)
        {
            float baseAngle = 2f * Mathf.PI * (float)i / (float)projectilesCount;
            Vector3 positionOffset = new Vector3(Mathf.Cos(baseAngle + angle), Mathf.Sin(baseAngle + angle), 0f);
            positionOffset *= distanceFromCenter;

            projectiles[i].transform.position = transform.position + positionOffset;
        }
    }

    private void SendProjectiles()
    {
        for (int i = 0; i < projectilesCount; i++)
        {
            Vector3 addedVelocity = Vector3.Normalize(projectiles[i].transform.position - transform.position) * sendProjectilesPower;

            projectiles[i].velocity += addedVelocity;
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);

        Destroy(gameObject);
    }
}
