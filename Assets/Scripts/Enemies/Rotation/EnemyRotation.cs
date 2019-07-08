using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    public float rotationSpeed = 600f;
    private float angleTreshold = 10f; //below this value, just set rotation to a proper value

    private Quaternion aircraftRight;
    private Quaternion aircraftLeft;

    private Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<Movement>();

        Vector3 temp;
        temp.x = 0f;
        temp.y = 90f;
        temp.z = 0f;

        aircraftRight = Quaternion.Euler(temp);

        temp.y = 270f;
        aircraftLeft = Quaternion.Euler(temp);

        temp.y = 180f;
        transform.rotation = Quaternion.Euler(temp);

        angleTreshold = rotationSpeed / 30f;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        if (movement.directionRight)
        {
            if (transform.eulerAngles.y < 90f + angleTreshold && transform.eulerAngles.y > 90f - angleTreshold)
                transform.rotation = aircraftRight;
            else
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        }
        else
        {
            if (transform.eulerAngles.y < 270f + angleTreshold && transform.eulerAngles.y > 270f - angleTreshold)
                transform.rotation = aircraftLeft;
            else
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}
