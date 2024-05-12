using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public eScoreTypes ScoreType = eScoreTypes.Player;
    public GameObject GameManagerObject;

    private float originalWidth;
    private float originalHealth;

    void Start()
    {
        if(GameManagerObject == null)
        {
            Debug.LogError("GameManager reference not set on the Healthbar!");
            return;
        }

        TheGameManager gm = GameManagerObject.GetComponent<TheGameManager>();

        if(ScoreType == eScoreTypes.Enemy)
        {
            gm.OnPlayerHealthChange += OnHealthChange;
            originalHealth = gm.PlayerHealth;
        }
        else if (ScoreType == eScoreTypes.Player)
        {
            gm.OnEnemyHealthChange += OnHealthChange;
            originalHealth = gm.EnemyHealth;
        }

        originalWidth = gameObject.GetComponent<RectTransform>().rect.width;
    }

    void OnHealthChange(float newHealth)
    {
        float healthPercent = Mathf.Lerp(0, originalWidth, newHealth / originalHealth);
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(healthPercent, rectTransform.rect.height);
    }
}
