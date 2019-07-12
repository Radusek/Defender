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

    public float reloadTime = 0.33f;
    protected float timeToShoot = 0f;

    protected int projectileDirection = 1;

    private void Start()
    {
        Vector3 temp = new Vector3(0f, 90f, 0f);
        projectileRight = Quaternion.Euler(temp);

        temp.y = 270f;
        projectileLeft = Quaternion.Euler(temp);
    }

    protected void UpdateParameters()
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
    }

    protected bool CanShoot()
    {
        if (Input.GetKey(KeyCode.Space) && timeToShoot == 0f)
            return true;

        return false;
    }
}