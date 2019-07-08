using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpCollision : MonoBehaviour
{
    public int hp = 25;
    public int reward = 750;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
            return;

        hp--;
        Destroy(collision.gameObject);

        if (GameManager.Instance.GetPlayerLost() == false && hp == 0)
        {
            GameManager.Instance.AddScore(reward);
            GameManager.Instance.RemoveFromList(this.GetComponent<Rigidbody>());
            Destroy(gameObject);
        }
    }
}
