using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gameover : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text HStext;

    int score;
    int highscore;

    private void OnEnable()
    {
        score = GameManager.Instance.GetScore();
        highscore = GameManager.Instance.GetHighScore();

        scoreText.text = score.ToString();
        Time.timeScale = 0;
    }
    private void Start()
    {
        if (score == highscore)
            HStext.gameObject.SetActive(true);
        
    }

    private void OnDisable()
    {
        HStext.enabled = false;
    }
}
