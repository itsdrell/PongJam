using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Damage = 6.0f;
    public float InitialSpeed = 10.0f;

    public int MaxNumberOfSplits = 3;
    private int numberOfSplitsLeft;
    public int NumberOfSplitsLeft { get { return numberOfSplitsLeft; } }

    private void Awake()
    {
        numberOfSplitsLeft = MaxNumberOfSplits;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SplitWall")
        {
            if(numberOfSplitsLeft > 0)
            {
                SpawnSplit();
                SpawnSplit();
            }

            Destroy(this.gameObject);
        }
    }

    private void SpawnSplit()
    {
        GameObject newBall = Instantiate(this.gameObject);
        newBall.GetComponent<Ball>().InitSplit(numberOfSplitsLeft - 1);
    }

    private void InitSplit(int splitsLeft)
    {
        Vector2 ballDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized;
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(ballDirection * InitialSpeed);

        this.gameObject.GetComponent<Ball>().numberOfSplitsLeft = splitsLeft;

        float scaleSize = numberOfSplitsLeft * .1f;
        this.transform.localScale = new Vector3(scaleSize, scaleSize, 1);
    }
}
