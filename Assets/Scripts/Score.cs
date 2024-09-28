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
            // Verhindere doppelte Score-Objekte, wenn eine neue Szene geladen wird
            if (FindObjectsOfType<Score>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            // Mache dieses GameObject persistent über Szenenwechsel
            DontDestroyOnLoad(gameObject);
    
            scoreText.text = "0";

            Debug.Log("Score object exists and is persistent across scenes.");

    }

    public void AddScore(int amount)
    {
        globalScore += amount;
        scoreText.text = globalScore.ToString();
    }

      public void ResetScore()
    {
        globalScore = 0; // Setzt den globalen Score auf 0 zurück
        scoreText.text = globalScore.ToString(); // Aktualisiert die Anzeige
    }
}
