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

    [HideInInspector]
    public bool nextWaveAnimationPlaying = false;

    public GameObject scoreSubmitButton;
    public InputField playerNameField;


    // ######################## ALL AIRCRAFTS ########################

    [HideInInspector]
    public List<Rigidbody> rbs;

    public float verticalPosLimit = 8f;
    public float horizontalPosLimit = 8f;


    // ######################## PLAYER ########################

    [HideInInspector]
    public Rigidbody player;
    public GameObject playerPrefab;

    bool playerLost = false;

    int score = 0;
    public int startingLives = 4;
    private int livesLeft;

    private PlayerMovement playerMovement;


    // ######################## ENEMIES ########################

    public GameObject[] enemyPool;
    public GameObject[] bossPool;

    public int bossWaveInterval = 4;

    [HideInInspector]
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

    public bool quittingScene = false;



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
        Time.timeScale = 1f;
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
    }

    public void SetupLevel()
    {
        DestroyAllAircrafts();
        PlayerUpgradesManager.Instance.Reset();
        RespawnPlayer();

        rbs = new List<Rigidbody>
        {
            player
        };

        camera.transform.position = new Vector3(0f, 0f, -15f);

        timeToNextSpawn = 0f;
        livesLeft = startingLives;
        score = 0;

        livesText.SetText("Lives: " + livesLeft.ToString());
        scoreText.SetText("Score: " + score.ToString());

        playerLost = false;

        EnableSubmit();

        endScreenUI.SetActive(false);

        enemiesToSpawn = enemiesPerWave;
        waveNumber = 1;
    }

    private void EnableSubmit()
    {
        playerNameField.gameObject.SetActive(true);
        playerNameField.text = "";
        scoreSubmitButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate() {
        LimitEntitiesPositions();
    }

    private void SpawnEnemies()
    {
        if (playerLost || nextWaveAnimationPlaying)
            return;

        if (enemiesToSpawn <= 0 && (rbs.Count + portals.Count) == 1 && player != null)
        {
            NextWave();
            return;
        }

        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn <= 0f && enemiesToSpawn > 0)
        {
            if (IsBossWave())
            {
                SummonBoss();
                timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
                return;
            }

            SummonEnemy();

            timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    public bool IsBossWave()
    {
        if (waveNumber % bossWaveInterval == 0)
            return true;

        return false;
    }

    public int DifficultyLevel()
    {
        return (waveNumber - 1) / bossWaveInterval;
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
        portal.enemyType = enemyType;
    }

    void SummonBoss()
    {
        int bossCount = bossPool.Length;
        int bossNumber = waveNumber / bossWaveInterval - 1;

        int uniqueBosses = bossCount / 3;

        while (bossNumber >= bossCount)
            bossNumber -= uniqueBosses;

        Vector3 spawnPoint = new Vector3(10, 0, 0);

        portalScript portal = Instantiate(portalPrefab, spawnPoint, Quaternion.identity);
        portals.Add(portal);
        enemiesToSpawn--;
        portal.enemyPrefab = bossPool[bossNumber];
        portal.enemyType = 0;
    }

    void NextWave()
    {
        DestroyProjectiles();

        waveNumber++;
        nextWaveText.text = "Wave " + waveNumber.ToString();
        
        nextWaveObject.SetActive(true);
        nextWaveAnimation.Play("nextWave");
        nextWaveAnimationPlaying = true;

        enemiesToSpawn = enemiesPerWave + (int)(1.5f*Mathf.Sqrt(2*(waveNumber-1)));

        //Boss wave
        if (waveNumber % bossWaveInterval == 0)
            enemiesToSpawn = 1;

        minSpawnTime *= 0.98f;
        maxSpawnTime *= 0.97f;

        float minRandomRange = 0.1f;

        if (maxSpawnTime < minSpawnTime + minRandomRange)
            maxSpawnTime = minSpawnTime + minRandomRange;
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

        if (IsBossWave())
            enemiesToSpawn = 1;
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
        if (quittingScene)
            return;

        DestroyAllAircrafts();
        DestroyProjectiles();

        if (livesLeft > 0)
        {
            livesLeft--;
            livesText.SetText("Lives: " + livesLeft.ToString());

            RespawnPlayer();
            rbs.Add(player);

            return;
        }

        // GAME OVER
        CleanLevel();
    }

    private void RespawnPlayer()
    {
        player = Instantiate(playerPrefab).GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public void SubmitScore()
    {
        string name = playerNameField.text;
        if (name == "")
            return;

        bool firstChange = true;

        int newScore = score;
        int oldScore = 0;

        string newString = "";
        string oldString = "";

        for (int i = 0; i < 8; i++)
        {
            int currentScore = PlayerPrefs.GetInt("scoreValue" + i.ToString());
            if (newScore > currentScore)
            {
                oldScore = currentScore;
                oldString = PlayerPrefs.GetString("score" + i.ToString());

                PlayerPrefs.SetInt("scoreValue" + i.ToString(), newScore);

                if (firstChange)
                {
                    PlayerPrefs.SetString("score" + i.ToString(), name.PadRight(12) + newScore.ToString().PadRight(9) + waveNumber.ToString().PadRight(4));
                    firstChange = false;
                }
                else
                    PlayerPrefs.SetString("score" + i.ToString(), newString);

                newScore = oldScore;
                newString = oldString;
            }
        }

        scoreSubmitButton.SetActive(false);
        playerNameField.gameObject.SetActive(false);
    }
}
