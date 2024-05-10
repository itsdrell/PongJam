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

    // Callbacks for the UI to listen to
    public UnityAction<float> OnPlayerHealthChange;
    public UnityAction<float> OnEnemyHealthChange;

    // debug
    public GameObject ball;

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

        // delete all balls?
    }

    public void Score(eScoreTypes whoScored, float amount)
    {
        if(whoScored == eScoreTypes.Player)
        {
            EnemyHealth -= amount;
            OnEnemyHealthChange.Invoke(EnemyHealth);
        }
        else if(whoScored == eScoreTypes.Enemy)
        {
            PlayerHealth -= amount;
            OnPlayerHealthChange.Invoke(PlayerHealth);
        }
    }

    // Hot keys to help test features
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
            EnemyHealth = 0.0f;
            OnEnemyHealthChange.Invoke(EnemyHealth);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            PlayerHealth = 0.0f;
            OnPlayerHealthChange.Invoke(PlayerHealth);
        }
    }
}
