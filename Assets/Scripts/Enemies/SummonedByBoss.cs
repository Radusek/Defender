using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedByBoss : MonoBehaviour
{
    public Boss3Spawner parent = null;

    private void OnDestroy()
    {
        GameManager.Instance.rbs.Remove(gameObject.GetComponent<Rigidbody>());
        parent.summons.Remove(gameObject);
    }
}
