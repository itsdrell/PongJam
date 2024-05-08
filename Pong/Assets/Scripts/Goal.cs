using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum eWhoseGoal { Player, Enemy};

    public eWhoseGoal WhoseGoal = eWhoseGoal.Player;

    public GameObject TheGameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if(TheGameManager == null)
        {
            // assert
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            eScoreTypes type = (WhoseGoal == eWhoseGoal.Player) ? eScoreTypes.Player : eScoreTypes.Enemy;
            TheGameManager.GetComponent<TheGameManager>().Score(type, 10);

            Destroy(collision.gameObject);
        }
    }
}
