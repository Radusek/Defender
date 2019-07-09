using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1easyShooting : MonoBehaviour
{
    public int baseProjectileCount = 3;
    public float basePosFromCenter = 0.5f;
    public Boss1ProjectileScript b1p;

    private Vector3 baseProjectileScale = Vector3.one * 0.6f;


    private void Start()
    {
        b1p.projectilesCount = baseProjectileCount;
        b1p.distanceFromCenter = basePosFromCenter;
        b1p.transform.localScale = baseProjectileScale;
    }
}
