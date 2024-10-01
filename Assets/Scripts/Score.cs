using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        UpdateReferences();
    }

    // Aktualisiere die Referenzen zum UI-Element in der neuen Szene
    private void UpdateReferences()
    {
        // Sucht das Text-Element mit einem bestimmten Tag
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        
        if (scoreText == null)
        {
            Debug.LogError("ScoreText-UI-Element in der neuen Szene nicht gefunden!");
        }
        else
        {
            // Aktualisiere den Score-Text in der neuen Szene
            scoreText.text = globalScore.ToString();
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

    private void OnDestroy()
    {
        // Deregistriere die Methode, um Speicherlecks zu vermeiden
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
