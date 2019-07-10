using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionBehaviour : EnemyCollision
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer || dead)
            return;

        if (GameManager.Instance.GetPlayerLost() == false)
            GameManager.Instance.AddScore(reward);

        GameManager.Instance.RemoveFromList(collision.gameObject.GetComponent<Rigidbody>());
        dead = true;
        Destroy(collision.gameObject);

        GameManager.Instance.RemoveFromList(this.GetComponent<Rigidbody>());

        Destroy(gameObject);
    }
}
