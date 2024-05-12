using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ePlayerNumbers { One, Two };

    [Header("Assign which prefab is player one or player two in the map")]
    public ePlayerNumbers PlayerNumber = ePlayerNumbers.One;

    [Range(0, 30)] // No negative, 30 basically is teleporting
    public float MovementSpeed = 10.0f;

    [Header("The highest and lowest position we want the player to be able to go")]
    public float yClamp = 2.75f;

    // We could set these in project settings I think, but this works for simple game
    private KeyCode UpKey = KeyCode.W;
    private KeyCode DownKey = KeyCode.S;
    
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerNumber == ePlayerNumbers.Two)
        {
            UpKey = KeyCode.UpArrow;
            DownKey = KeyCode.DownArrow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(UpKey))
        {
            Move(1);
        }
        else if (Input.GetKey(DownKey))
        {
            Move(-1);
        }
    }

    private void Move(float direction)
    {
        Vector3 currentPosition = transform.position;

        float step = MovementSpeed * Time.deltaTime * direction;
        float newY = Mathf.Clamp(currentPosition.y + step, -yClamp, yClamp);
        
        transform.position = new Vector3(currentPosition.x, newY, 0);
    }
}
