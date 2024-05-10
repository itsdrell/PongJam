using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public eScoreTypes ScoreType = eScoreTypes.Player;
    public GameObject GameManagerObject;

    private float originalWidth;

    void Start()
    {
        if(GameManagerObject == null)
        {
            // assert
            return;
        }

        TheGameManager gm = GameManagerObject.GetComponent<TheGameManager>();

        if(ScoreType == eScoreTypes.Enemy)
        {
            gm.OnPlayerHealthChange += OnHealthChange;
        }
        else if (ScoreType == eScoreTypes.Player)
        {
            gm.OnEnemyHealthChange += OnHealthChange;
        }

        originalWidth = gameObject.GetComponent<RectTransform>().rect.width;
    }

    void OnHealthChange(float newHealth)
    {
        float healthPercent = Mathf.Lerp(0, originalWidth, newHealth / 100.0f);
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(healthPercent, rectTransform.rect.height);

    }
}
