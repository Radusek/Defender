using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : Movement
{
    private float verticalBound;

    private void Start()
    {
        verticalBound = GameManager.Instance.verticalPosLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayerLost() || GameManager.Instance.player == null)
            return;

        Move();
    }

    void Move()
    {
        movementTimer -= Time.deltaTime;

        if (movementTimer <= 0f)
        {
            // X position
            if (Math.Abs(transform.position.x - GameManager.Instance.player.transform.position.x) > 12f)
                movementVector.x = GameManager.Instance.player.transform.position.x - transform.position.x;
            else
                movementVector.x = UnityEngine.Random.Range(-1f, 1f);

            directionRight = movementVector.x > 0f ? true : false;


            // Y position
            if (Math.Abs(transform.position.y) > 0.8 * verticalBound)
                movementVector.y = UnityEngine.Random.Range(transform.position.y > 0f ? -1f : 0f, transform.position.y > 0f ? 0f : 1f);
            else
                movementVector.y = UnityEngine.Random.Range(-1f, 1f);


            movementVector.Normalize();
            movementVector *= flightSpeed;

            movementTimer = UnityEngine.Random.Range(minChangeTime, maxChangeTime);
        }

        Vector3 addedVector = movementVector * Time.deltaTime;

        transform.position += addedVector;
    }
}
