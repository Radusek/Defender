using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpgradesManager : MonoBehaviour
{
    public static EnemyUpgradesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void SetUpgrades(GameObject enemy, int enemyType)
    {
        if (GameManager.Instance.IsBossWave())
            return;

        int level = GameManager.Instance.DifficultyLevel();

        EnemyCollision collision = enemy.GetComponent<EnemyCollision>();

        if (enemyType == 0) // DRONE
        {
            DirectMultiShot shooting = enemy.GetComponent<DirectMultiShot>();
            shooting.projectilesCount = 1 + level;

            collision.reward = 100 + 35 * level;
        }
        if (enemyType == 1) // BOMBER
        {
            CircleBurstShooting shooting = enemy.GetComponent<CircleBurstShooting>();
            shooting.burstSize = 12 + 2 * level;

            collision.reward = 250 + 25 * level;
        }
        if (enemyType == 2) // KAMIKAZE
        {
            KamikazeMovement movement = enemy.GetComponent<KamikazeMovement>();
            movement.accelerationRate = 1 + 0.2f * level;

            collision.reward = 150 + 30 * level;
        }
    }
}
