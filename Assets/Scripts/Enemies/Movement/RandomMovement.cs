using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : Movement
{
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        movementTimer -= Time.deltaTime;

        if (movementTimer <= 0f)
        {
            movementVector.x = Random.Range(-1f, 1f);
            directionRight = movementVector.x > 0f ? true : false;

            movementVector.y = Random.Range(-1f, 1f);

            movementVector.Normalize();
            movementVector *= flightSpeed;

            movementTimer = Random.Range(minChangeTime, maxChangeTime);
        }

        Vector3 addedVector = movementVector * Time.deltaTime;

        transform.position += addedVector;
    }
}
