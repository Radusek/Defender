using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float flightSpeed = 10f;

    protected float movementTimer = 0f;
    protected Vector3 movementVector = Vector3.zero;

    public float minChangeTime = 0.4f;
    public float maxChangeTime = 2f;

    [HideInInspector]
    public bool directionRight = true;
}
