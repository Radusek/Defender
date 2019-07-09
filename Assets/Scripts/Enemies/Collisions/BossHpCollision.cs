using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpCollision : EnemyCollision
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

        if (hp <= 0)
            dead = true;

        Destroy(collision.gameObject);

        healthBar.value = 1f - ((float)hp / (float)maxHp);


        if (GameManager.Instance.GetPlayerLost() == false && dead)
        {
            GameManager.Instance.AddScore(reward);
            GameManager.Instance.RemoveFromList(transform.parent.gameObject.GetComponent<Rigidbody>());
            Destroy(transform.parent.gameObject);
        }
    }
}
