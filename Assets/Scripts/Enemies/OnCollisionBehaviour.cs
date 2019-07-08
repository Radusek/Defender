using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionBehaviour : MonoBehaviour
{
    public int reward = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
            return;


        if (GameManager.Instance.GetPlayerLost() == false)
            GameManager.Instance.AddScore(reward);

        GameManager.Instance.RemoveFromList(collision.gameObject.GetComponent<Rigidbody>());
        Destroy(collision.gameObject);

        GameManager.Instance.RemoveFromList(this.GetComponent<Rigidbody>());

        Destroy(gameObject);
    }
}
