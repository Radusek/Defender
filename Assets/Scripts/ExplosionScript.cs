using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        
        Destroy(gameObject, ps.main.duration + 0.1f);
    }
}
