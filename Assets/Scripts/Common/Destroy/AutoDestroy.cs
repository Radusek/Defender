using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : AutoDestroyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
