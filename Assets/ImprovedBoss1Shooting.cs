using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedBoss1Shooting : MonoBehaviour
{
    public int baseProjectileCount = 3;
    public float basePosFromCenter = 0.5f;
    public Shooting shooting;
    public Boss1ProjectileScript b1p;

    private Vector3 baseProjectileScale = Vector3.one * 0.6f;

    // Update is called once per frame
    void Update()
    {
        b1p.projectilesCount = baseProjectileCount + shooting.shotsFired / 3;
        b1p.distanceFromCenter = basePosFromCenter + shooting.shotsFired * 0.05f;
        b1p.transform.localScale = baseProjectileScale * (1 + shooting.shotsFired * 0.08f);
    }
}
