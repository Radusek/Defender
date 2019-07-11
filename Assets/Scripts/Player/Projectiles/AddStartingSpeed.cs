using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStartingSpeed : MonoBehaviour
{
    public Rigidbody rb;

    public float projectileSpeed = 1000f;

    // Use this for initialization
    void Start()
    {
        int sign = 1;
        if (transform.eulerAngles.y == 270f)
            sign = -1;

        rb.AddForce(sign * projectileSpeed, 0f, 0f);
    }
}
