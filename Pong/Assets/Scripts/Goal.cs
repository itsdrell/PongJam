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
        if (TheGameManager == null)
        {
            Debug.LogError("GameManager reference not set on a Goal!");
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Ball ballScript = collision.gameObject.GetComponent<Ball>();
            float percentDamage = ((float)ballScript.NumberOfSplitsLeft) / ((float)ballScript.MaxNumberOfSplits);
            float totalDamage = ballScript.Damage * percentDamage; 
            
            eScoreTypes type = (WhoseGoal == eWhoseGoal.Player) ? eScoreTypes.Enemy : eScoreTypes.Player;
            TheGameManager.GetComponent<TheGameManager>().Score(type, totalDamage);

            Destroy(collision.gameObject);
        }
    }
}
