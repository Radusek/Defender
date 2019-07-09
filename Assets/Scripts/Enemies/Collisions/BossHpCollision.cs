using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpCollision : MonoBehaviour
{
    public int maxHp = 25;
    private int hp;
    public int reward = 750;

    public Slider healthBar;

    private void Start()
    {
        hp = maxHp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
            return;

        hp--;
        Destroy(collision.gameObject);

        healthBar.value = 1f - ((float)hp / (float)maxHp);

        if (GameManager.Instance.GetPlayerLost() == false && hp == 0)
        {
            GameManager.Instance.AddScore(reward);
            GameManager.Instance.RemoveFromList(transform.parent.gameObject.GetComponent<Rigidbody>());
            Destroy(transform.parent.gameObject);
        }
    }
}
