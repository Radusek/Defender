using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    // ######################## CAMERA ########################

    public Camera camera;

    public float cameraSmoothness = 0.25f;
    private Vector3 cameraVelocity = Vector3.zero;


    // ######################## UI ########################
    public GameObject pauseScreen;

    public GameObject nextWaveObject;
    private Animator nextWaveAnimation;
    public TextMeshProUGUI nextWaveText;

    public GameObject endScreenUI;
    public TextMeshProUGUI totalScoreText;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    //[HideInInspector]
    public bool nextWaveAnimationPlaying = false;


    // ######################## ALL AIRCRAFTS ########################

    //[HideInInspector]
    public List<Rigidbody> rbs;

    public float verticalPosLimit = 8f;
    public float horizontalPosLimit = 8f;


    // ######################## PLAYER ########################

    //[HideInInspector]
    public Rigidbody player;
    public GameObject playerPrefab;

    bool playerLost = false;

    int score = 0;
    public int livesLeft = 4;

    private PlayerMovement playerMovement;


    // ######################## ENEMIES ########################

    public GameObject[] enemyPool;
    public GameObject[] bossPool;

    public int bossWaveInterval = 4;

    //[HideInInspector]
    public List<portalScript> portals = new List<portalScript>();

    [HideInInspector]
    public List<GameObject> enemyProjectiles = new List<GameObject>();

    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;

    private float timeToNextSpawn = 0f;

    public int enemiesPerWave = 5;
    private int enemiesToSpawn;

    int waveNumber = 1;

    public portalScript portalPrefab;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start() {
        SetupLevel();
        nextWaveAnimation = nextWaveObject.GetComponent<Animator>();
        nextWaveObject.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        MoveCamera();
        SpawnEnemies();
        HandleMenuControls();
    }

    private void HandleMenuControls()
    {
        if (playerLost)
           Time.timeScale = 1f;

        if (Input.GetKeyDown(KeyCode.Escape) && playerLost == false)
        {
            if (Time.timeScale == 1f)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    void CleanLevel()
    {
        endScreenUI.SetActive(true);

        playerLost = true;

        foreach (var rb in rbs)
        {
            Destroy(rb.gameObject);
        }
        rbs.Clear();

        foreach (var portal in portals)
        {
            Destroy(portal.gameObject);
        }
        portals.Clear();

        totalScoreText.text = "Total score: " + score.ToString();

        waveNumber = 1;
    }

    public void SetupLevel()
    {
        DestroyAllAircrafts();
        player = Instantiate(playerPrefab).GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();

        rbs = new List<Rigidbody>
        {
            player
        };

        camera.transform.position = new Vector3(0f, 0f, -15f);

        timeToNextSpawn = 0f;
        livesLeft = 4;
        score = 0;

        livesText.SetText("Lives: " + livesLeft.ToString());
        scoreText.SetText("Score: " + score.ToString());

        playerLost = false;
        endScreenUI.SetActive(false);

        enemiesToSpawn = enemiesPerWave;
    }

    // Update is called once per frame
    void LateUpdate() {
        LimitEntitiesPositions();
    }

    private void SpawnEnemies()
    {
        if (playerLost || nextWaveAnimationPlaying)
            return;

        if (enemiesToSpawn == 0 && (rbs.Count + portals.Count) == 1 && player != null)
        {
            NextWave();
            return;
        }

        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn <= 0f && enemiesToSpawn > 0)
        {
            if (waveNumber % bossWaveInterval == 0)
            {
                SummonBoss();
                return;
            }

            SummonEnemy();

            timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void SummonEnemy()
    {    
        int maxRand = Mathf.Min(enemyPool.Length, waveNumber);
        int enemyType = Random.Range(0, maxRand);

        Vector3 spawnPoint;
        spawnPoint.x = Random.Range(-horizontalPosLimit, horizontalPosLimit);
        spawnPoint.y = Random.Range(-verticalPosLimit, verticalPosLimit);
        spawnPoint.z = 0f;

        portalScript portal = Instantiate(portalPrefab, spawnPoint, Quaternion.identity);
        portals.Add(portal);
        enemiesToSpawn--;
        portal.enemyPrefab = enemyPool[enemyType];
    }

    void SummonBoss()
    {
        int bossCount = bossPool.Length;
        int bossNumber = (waveNumber / bossWaveInterval) % bossCount;

        Vector3 spawnPoint = new Vector3(10, 0, 0);

        portalScript portal = Instantiate(portalPrefab, spawnPoint, Quaternion.identity);
        portals.Add(portal);
        enemiesToSpawn--;
        portal.enemyPrefab = bossPool[bossNumber];
    }

    void NextWave()
    {
        DestroyProjectiles();

        waveNumber++;
        nextWaveText.text = "Wave " + waveNumber.ToString();
        
        nextWaveObject.SetActive(true);
        nextWaveAnimation.Play("nextWave");
        nextWaveAnimationPlaying = true;

        enemiesToSpawn = enemiesPerWave;

        //Boss wave
        if (waveNumber % bossWaveInterval == 0)
            enemiesToSpawn = 1;

        minSpawnTime *= 0.98f; 
        maxSpawnTime *= 0.97f; 
    }


    void DestroyProjectiles()
    {
        foreach (var projectile in enemyProjectiles)
        {
            Destroy(projectile.gameObject);
        }

        enemyProjectiles.Clear();
    }

    public void DestroyAllAircrafts()
    {
        enemiesToSpawn += rbs.Count + portals.Count;

        if (player != null)
            if (rbs.Contains(player))
                enemiesToSpawn--;

        foreach (var rb in rbs)
        {
            Destroy(rb.gameObject);
        }
        rbs.Clear();

        foreach (var portal in portals)
        {
            Destroy(portal.gameObject);
        }
        portals.Clear();
    }

    void MoveCamera()
    {
        if (playerLost)
            return;

        float offset = 12f;

        Vector3 cameraPos = camera.transform.position;
        cameraPos.x = player.position.x;

        Vector3 targetPosition = player.position;
        targetPosition.y = 0f;
        targetPosition.z = -15f;

        if (playerMovement.stopped == false)
        {
            if (playerMovement.directionRight)
            {
                targetPosition.x += offset/3;
            }
            else
            {
                targetPosition.x -= offset/3;
            }
        }


        if (targetPosition.x > horizontalPosLimit - offset)
            targetPosition.x = horizontalPosLimit - offset;

        if (targetPosition.x < -horizontalPosLimit + offset)
            targetPosition.x = -horizontalPosLimit + offset;

        Vector3 smoothedPosition = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothness);

        camera.transform.position = smoothedPosition;
    }

    void LimitEntitiesPositions()
    {
        foreach (var rb in rbs)
        {
            Vector3 pos = rb.transform.position;

            if (pos.x > horizontalPosLimit || pos.x < -horizontalPosLimit)
                pos = new Vector3(System.Math.Sign(pos.x) * horizontalPosLimit, pos.y, pos.z);

            if (pos.y > verticalPosLimit || pos.y < -verticalPosLimit)
                pos = new Vector3(pos.x, System.Math.Sign(pos.y) * verticalPosLimit, pos.z);

            rb.transform.position = pos;
        }
    }

    public void RemoveFromList(Rigidbody rb)
    {
        rbs.Remove(rb);
    }

    public bool GetPlayerLost()
    {
        return playerLost;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score.ToString();
    }

    public void LoseLife()
    {
        DestroyAllAircrafts();
        //enemiesToSpawn--;
        DestroyProjectiles();

        if (livesLeft > 0)
        {
            livesLeft--;
            livesText.SetText("Lives: " + livesLeft.ToString());

            player = Instantiate(playerPrefab).GetComponent<Rigidbody>();
            playerMovement = player.gameObject.GetComponent<PlayerMovement>();
            rbs.Add(player);

            return;
        }

        // GAME OVER
        CleanLevel();
    }
}
