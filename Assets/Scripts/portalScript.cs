using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemyPrefab;

    [HideInInspector]
    public int enemyType = 0;

    private float timeToSpawnEntity = 1.5f;
    private bool spawned = false;

    private void Update()
    {
        timeToSpawnEntity -= Time.deltaTime;
        if (timeToSpawnEntity <= 0f && spawned == false)
        {
            GameObject entity = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            EnemyUpgradesManager.Instance.SetUpgrades(entity, enemyType);

            Rigidbody rb = entity.GetComponent<Rigidbody>();
            GameManager.Instance.rbs.Add(rb);

            GameManager.Instance.portals.Remove(this);
            spawned = true;
            Destroy(gameObject);
        }
    }
}
