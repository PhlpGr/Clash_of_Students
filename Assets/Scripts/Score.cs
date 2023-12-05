using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int ScorePerItem;
    public int globalScore;

    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = "0";
    }

    public void AddScore()
    {
        globalScore += ScorePerItem;
        scoreText.text = globalScore.ToString();
    }
}
