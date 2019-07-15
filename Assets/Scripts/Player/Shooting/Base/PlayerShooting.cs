using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public PlayerMovement movement;

    protected Quaternion projectileLeft;
    protected Quaternion projectileRight;

    protected Quaternion chosenRotation;

    public GameObject projectilePrefab;

    public float reloadTime = 0.2f;
    protected float timeToShoot = 0f;

    protected int projectileDirection = 1;


    public float maxHoldDownTime = 3f;
    private float holdDownTime = 0f;
    private bool spaceReleased = true;


    private void Start()
    {
        Vector3 temp = new Vector3(0f, 90f, 0f);
        projectileRight = Quaternion.Euler(temp);

        temp.y = 270f;
        projectileLeft = Quaternion.Euler(temp);
    }

    protected int UpdateParameters()
    {
        if (movement.directionRight)
        {
            chosenRotation = projectileRight;
            projectileDirection = 1;
        }
        else
        {
            chosenRotation = projectileLeft;
            projectileDirection = -1;
        }


        timeToShoot -= Time.deltaTime;
        if (timeToShoot < 0f)
            timeToShoot = 0f;


        if (Input.GetKey(KeyCode.Space))
        {
            holdDownTime += Time.deltaTime;
            spaceReleased = false;
            return 0;
        }
        else if (holdDownTime >= maxHoldDownTime)
        {
            if (spaceReleased == false)
            {
                //szczeloj mocno
                holdDownTime = 0f;
                spaceReleased = true;
                return 2;
            }
            else
                return 0;
        }
        else
        {
            if (spaceReleased == false)
            {
                //szczeloj slabo
                holdDownTime = 0f;
                spaceReleased = true;
                return 1;
            }
            else
                return 0;
        }
    }

    protected bool CanShoot()
    {
        if (timeToShoot == 0f)
            return true;

        return false;
    }
}