using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eScoreTypes
{
    Player,
    Enemy
}

public class TheGameManager : MonoBehaviour
{
    public float PlayerHealth = 100.0f;
    public float EnemyHealth = 100.0f;

    [Header("Game Prefabs")]
    public GameObject SoloPlayerOnePrefab;
    public GameObject DuoPlayerOnePrefab;
    public GameObject DuoPlayerTwoPrefab;
    public GameObject BossPrefab;

    [Header("Spawn Locations")]
    public Transform SoloPlayerOneSpawn;
    public Transform DuoPlayerOneSpawn;
    public Transform DuoPlayerTwoSpawn;
    public Transform BossSpawn;

    [Header("UI Containers")]
    public GameObject MainMenuUI;
    public GameObject GameOverUI;
    public GameObject GameWonUI;

    [Header("Debug")]
    public GameObject ball;

    // Callbacks for the UI to listen to
    public event UnityAction<float> OnPlayerHealthChange;
    public event UnityAction<float> OnEnemyHealthChange;

    // Cleanup Tracking
    private GameObject BossSpawned;
    private GameObject PlayerOneSpawned;
    private GameObject PlayerTwoSpawned;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        DebugUpdate();
    }

    private void Reset()
    {
        PlayerHealth = 100.0f;
        EnemyHealth = 100.0f;

        OnPlayerHealthChange.Invoke(PlayerHealth);
        OnEnemyHealthChange.Invoke(EnemyHealth);

        ClearBoard();
    }

    private void ClearBoard()
    {
        Destroy(BossSpawned);
        Destroy(PlayerOneSpawned);
        Destroy(PlayerTwoSpawned);

        // Gross way to make sure the board is clean
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }

    public void StartGame(int numberOfPlayers)
    {
        Reset();

        // Spawn Players
        if(numberOfPlayers == 1)
        {
            PlayerOneSpawned = Instantiate(SoloPlayerOnePrefab, SoloPlayerOneSpawn.transform.position, SoloPlayerOneSpawn.transform.rotation);
        }
        else
        {
            PlayerOneSpawned = Instantiate(DuoPlayerOnePrefab, DuoPlayerOneSpawn.transform.position, DuoPlayerOneSpawn.transform.rotation);
            PlayerTwoSpawned = Instantiate(DuoPlayerTwoPrefab, DuoPlayerTwoSpawn.transform.position, DuoPlayerTwoSpawn.transform.rotation);
        }

        BossSpawned = Instantiate(BossPrefab, BossSpawn.transform.position, BossSpawn.transform.rotation);
        Boss bossScript = BossSpawned.GetComponent<Boss>();
        bossScript.SetReferences(this.gameObject, BossSpawn);

        MainMenuUI.SetActive(false);
    }

    public void Score(eScoreTypes whoScored, float amount)
    {
        if(whoScored == eScoreTypes.Player)
        {
            EnemyHealth -= amount;
            OnEnemyHealthChange.Invoke(EnemyHealth);

            if(EnemyHealth <= 0)
            {
                GameWon();
            }
        }
        else if(whoScored == eScoreTypes.Enemy)
        {
            PlayerHealth -= amount;
            OnPlayerHealthChange.Invoke(PlayerHealth);

            if(PlayerHealth <= 0)
            {
                GameLoss();
            }
        }
    }

    void GameLoss()
    {
        GameOverUI.SetActive(true);
        ClearBoard();
    }

    void GameWon()
    {
        GameWonUI.SetActive(true);
        ClearBoard();
    }

    public void GoBackToMainMenu()
    {
        MainMenuUI.SetActive(true);

        GameWonUI.SetActive(false);
        GameOverUI.SetActive(false);
    }

    // Hot keys to help test features
    // aka judgement free zone :)
    public void DebugUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            GameObject testBall = Instantiate(ball);
            Rigidbody2D rb = testBall.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, -200));
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameLoss();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            GameWon();
        }
    }
}
