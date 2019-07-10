using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Spawner : MonoBehaviour
{
    public GameObject[] summonPool;

    [HideInInspector]
    public List<GameObject> summons;
    public float summonRate = 1f;
    public float initialSummonCooldown = 3f;
    private float timeToSummon;

    // Start is called before the first frame update
    void Start()
    {
        timeToSummon = initialSummonCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timeToSummon -= Time.deltaTime;
        if (timeToSummon <= 0f)
            Summon();
    }

    void Summon()
    {
        int summonType = Random.Range(0, summonPool.Length);

        GameObject summon = Instantiate(summonPool[summonType], transform.position + Vector3.down, Quaternion.identity);
        SummonedByBoss script = summon.AddComponent<SummonedByBoss>();
        script.parent = this;
        summon.GetComponent<EnemyCollision>().reward = 0;

        summons.Add(summon);
        GameManager.Instance.rbs.Add(summon.GetComponent<Rigidbody>());

        timeToSummon = summonRate;
    }

    private void OnDestroy()
    {
        foreach(var summon in summons)
        {
            Destroy(summon);
        }
        summons.Clear();
    }
}
