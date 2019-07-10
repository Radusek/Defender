using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeMovement : MonoBehaviour
{
    public float flightSpeed = 8f;
    public bool movementWithAcceleration = false;
    public float accelerationRate = 0.5f;

    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayerLost())
            return;

        Move();
    }

    private void Move()
    {
        if (movementWithAcceleration)
            flightSpeed += accelerationRate * Time.deltaTime;

        Vector3 movementDirection = target.transform.position - transform.position;
        movementDirection.Normalize();

        Vector3 addedVector = movementDirection * flightSpeed * Time.deltaTime;

        transform.position += addedVector;
    }
}

