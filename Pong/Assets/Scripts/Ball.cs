using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Damage = 6.0f;
    public float InitialSpeed = 10.0f;

    public int MaxNumberOfSplits = 3;
    public int NumberOfSplitsLeft;

    private void Awake()
    {
        NumberOfSplitsLeft = MaxNumberOfSplits;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SplitWall")
        {
            if(NumberOfSplitsLeft > 0)
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
        newBall.GetComponent<Ball>().InitSplit(NumberOfSplitsLeft - 1);
    }

    private void InitSplit(int splitsLeft)
    {
        Vector2 ballDirection = new Vector2(Random.Range(-1, 1), 1);
        ballDirection.Normalize();
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(ballDirection * InitialSpeed);

        this.gameObject.GetComponent<Ball>().NumberOfSplitsLeft = splitsLeft;

        float scaleSize = NumberOfSplitsLeft * .1f;
        this.transform.localScale = new Vector3(scaleSize, scaleSize, 1);
    }
}
