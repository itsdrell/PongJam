using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearWall : MonoBehaviour
{
    public float horizontalSpeed = -10.0f;

    private void Update()
    {
        this.transform.position += new Vector3(horizontalSpeed, 0, 0) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
