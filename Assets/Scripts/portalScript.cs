using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemyPrefab;

    private float timeToSpawnEntity = 1.5f;
    private bool spawned = false;

    private void Update()
    {
        timeToSpawnEntity -= Time.deltaTime;
        if (timeToSpawnEntity <= 0f && spawned == false)
        {
            GameManager.Instance.rbs.Add(Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>());
            GameManager.Instance.portals.Remove(this);
            spawned = true;
            Destroy(gameObject);
        }
    }
}
