using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public GameObject projectilePrefab;

    public float reloadTime = 0.125f;
    private float timeToShoot = 0f;

    public float flightSpeed = 10f;

    [HideInInspector]
    public bool directionRight = true; //last input indicated moving right if true
    [HideInInspector]
    public bool stopped = true;

    public float rotationSpeed = 10f;
    private float angleTreshold = 10f; //below this value, just set rotation to a proper value

    private Quaternion aircraftRight;
    private Quaternion aircraftLeft;

    private Quaternion missileLeft;
    private Quaternion missileRight;

    private void Start()
    {
        Vector3 temp;
        temp.x = 90f;
        temp.y = 0f;
        temp.z = 90f;

        aircraftRight = Quaternion.Euler(temp);

        temp.z = 270f;
        aircraftLeft = Quaternion.Euler(temp);

        angleTreshold = rotationSpeed / 30f;

        temp.x = 0f;
        temp.y = 90f;
        temp.z = 0f;

        missileRight = Quaternion.Euler(temp);

        temp.y = 270f;
        missileLeft = Quaternion.Euler(temp);
    }

    // Update is called once per frame
    void Update () {
        MovePlayer();
        RotatePlayer();
        Shoot();
	}

    private void Shoot()
    {
        if(Input.GetKey(KeyCode.Space) && timeToShoot == 0f)
        {
            Quaternion missileRotation;

            if (directionRight)
                missileRotation = missileRight;
            else
                missileRotation = missileLeft;

            GameObject missile = Instantiate(projectilePrefab, transform.position, missileRotation);

            float sign = directionRight == true ? 1 : -1;

            if (stopped == false)
                missile.GetComponent<Rigidbody>().AddForce(new Vector3(sign * 1000f, 0f, 0f));

            timeToShoot = reloadTime;
        }

        timeToShoot -= Time.deltaTime;
        if (timeToShoot < 0f)
            timeToShoot = 0f;    
    }

    void MovePlayer()
    {
        stopped = true;

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            directionRight = true;
            stopped = false;
        }

        if (horizontal < 0)
        {
            directionRight = false;
            stopped = false;
        }

        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 addedVector = new Vector3(horizontal, vertical, 0);

        addedVector.Normalize();
        addedVector *= flightSpeed;
        addedVector *= Time.deltaTime;

        transform.position += addedVector;
    }

    void RotatePlayer()
    {
        if (directionRight)
        {
            if (transform.eulerAngles.y < 270f + angleTreshold && transform.eulerAngles.y > 270f - angleTreshold)
                transform.rotation = aircraftRight;
            else
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (transform.eulerAngles.y < 90f + angleTreshold && transform.eulerAngles.y > 90f - angleTreshold)
                transform.rotation = aircraftLeft;
            else
                transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsGoingRight()
    {
        return directionRight;
    }

    private void OnDestroy()
    {
        GameManager.Instance.LoseLife();
    }
}
