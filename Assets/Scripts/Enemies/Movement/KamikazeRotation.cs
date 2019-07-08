using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeRotation : MonoBehaviour
{
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = target.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.back);
        transform.rotation = rotation;
    }
}
