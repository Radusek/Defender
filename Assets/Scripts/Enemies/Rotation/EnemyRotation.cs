using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    public GameObject selfModel;

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
        selfModel.transform.rotation = Quaternion.Euler(temp);

        angleTreshold = rotationSpeed/15f;
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
            if (selfModel.transform.eulerAngles.y < 90f + angleTreshold && selfModel.transform.eulerAngles.y > 90f - angleTreshold)
                selfModel.transform.rotation = aircraftRight;
            else
                selfModel.transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        }
        else
        {
            if (selfModel.transform.eulerAngles.y < 270f + angleTreshold && selfModel.transform.eulerAngles.y > 270f - angleTreshold)
                selfModel.transform.rotation = aircraftLeft;
            else
                selfModel.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}
