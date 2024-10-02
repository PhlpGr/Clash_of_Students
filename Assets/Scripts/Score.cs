
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using QuizScripts; // Namensraum für ScoreCounter

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

        // Aktualisiere die Referenz zu scoreText beim Start
        UpdateReferences();

        Debug.Log("Score object exists and is persistent across scenes.");

        // Registriere die Methode, um beim Szenenwechsel die Referenzen zu aktualisieren
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Diese Methode wird aufgerufen, wenn eine neue Szene geladen wird
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateReferences(); // Versuche, die Referenzen jedes Mal neu zu setzen
    }

    private void UpdateReferences()
    {
        // Suche das GameObject mit dem Tag "ScoreText" in der aktuellen Szene
        GameObject scoreObject = GameObject.FindWithTag("ScoreText");

        if (scoreObject != null)
        {
            scoreText = scoreObject.GetComponent<TextMeshProUGUI>();

            // Weitere Einstellungen für das Text-Element vornehmen
            if (scoreText != null)
            {
                scoreText.enableWordWrapping = false;
                scoreText.overflowMode = TextOverflowModes.Overflow;
                scoreText.text = globalScore.ToString();
            }
        }
        else
        {
            Debug.LogWarning("Kein ScoreText-UI-Element mit dem Tag 'ScoreText' in dieser Szene gefunden.");
            scoreText = null; // Setze die Referenz auf null, falls das Element nicht vorhanden ist
        }
    }

    public void AddScore(int amount)
    {
        globalScore += amount;
        if (scoreText != null)
        {
            scoreText.text = globalScore.ToString();
        }
    }

    public void ResetScore()
    {
        globalScore = 0; // Setzt den globalen Score auf 0 zurück
        if (scoreText != null)
        {
            scoreText.text = globalScore.ToString(); // Aktualisiert die Anzeige
        }
    }

    public void SynchronizeScoreWithCounter(ScoreCounter scoreCounter)
    {
        if (scoreCounter != null)
        {
            scoreCounter.lection_score = globalScore;
        }
    }

    private void OnDestroy()
    {
        // Deregistriere die Methode, um Speicherlecks zu vermeiden
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
