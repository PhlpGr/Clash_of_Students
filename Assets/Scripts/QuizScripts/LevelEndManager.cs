using UnityEngine;
using QuizScripts; // Für den Zugriff auf ScoreCounter
using System.Collections; // Für IEnumerator und Coroutines

public class LevelEndManager : MonoBehaviour
{
    private ScoreCounter scoreCounter; // Referenz auf den zentralen ScoreCounter
    private Infos quizInfos; // Informationen zum aktuellen Quiz

    private void Start()
    {
        // Hole den zentralen ScoreCounter aus dem GameManager
        scoreCounter = Version2_GameManager.Instance.scoreCounter;

        if (scoreCounter == null)
        {
            Debug.LogError("ScoreCounter konnte nicht aus dem GameManager geladen werden.");
        }

        // Hole die Infos vom GameManager
        quizInfos = Version2_GameManager.Instance.quizInfos;

        // Überprüfe JWTDisplayManager-Instanz
        if (JWTDisplayManager.Instance == null)
        {
            Debug.LogError("JWTDisplayManager konnte nicht gefunden werden.");
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Spieler hat das Level-Ende erreicht, jetzt Score posten
            Debug.Log("Level beendet, Score wird gepostet...");
            //Debug.Log($"{JWTDisplayManager.Instance.professor_email}");
            //Debug.Log($"{JWTDisplayManager.Instance.program}");
            //Debug.Log($"{JWTDisplayManager.Instance.course}");
            Debug.Log($"{quizInfos.Lection}");
            Debug.Log($"{scoreCounter.lection_score}");
            Debug.Log($"{quizInfos.Mail}");
            
            // PostScoreAsync-Methode aufrufen, um den Score zu senden
            StartCoroutine(PostFinalScore());
        }
    }

    private IEnumerator PostFinalScore()
    {
        // Warten auf den PostScoreAsync-Aufruf
        yield return scoreCounter.PostScoreAsync(
            JWTDisplayManager.Instance.professor_email, // E-Mail des Professors
            JWTDisplayManager.Instance.program,         // Programm aus dem JWT
            JWTDisplayManager.Instance.course,          // Kurs aus dem JWT
            quizInfos.Lection, 
            scoreCounter.lection_score, 
            quizInfos.Mail // Nutzer-Mail
        );

        Debug.Log("Score erfolgreich gepostet.");
    }
}