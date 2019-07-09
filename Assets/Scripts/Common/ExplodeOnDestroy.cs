using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDestroy : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnDestroy()
    {
        if (GameManager.Instance.quittingScene)
            return;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
