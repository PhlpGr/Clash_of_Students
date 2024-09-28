using UnityEngine;
using QuizScripts; // Für den Zugriff auf ScoreCounter
using System.Collections; // Für IEnumerator und Coroutines

public class LevelEndManager : MonoBehaviour
{
    private ScoreCounter scoreCounter; // Referenz auf den zentralen ScoreCounter
    private Infos quizInfos; // Informationen zum aktuellen Quiz
    private Score score; // Reference to the Score component

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

        // **Initialisierung von `score`**
        score = FindObjectOfType<Score>();
        if (score == null)
        {
            Debug.LogError("Score-Komponente konnte nicht gefunden werden.");
        }

    }

/*    private void OnTriggerEnter2D(Collider2D collision)
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

*/

    // **Newly added method to post the score and reset**
    public void PostScoreAndReset()
    {
        // Start the coroutine to post the score
        StartCoroutine(PostFinalScore());
    }

    // Modified `PostFinalScore` coroutine to reset the score after posting
    private IEnumerator PostFinalScore()
    {
        // Post the score asynchronously
        yield return scoreCounter.PostScoreAsync(
            JWTDisplayManager.Instance.professor_email,
            JWTDisplayManager.Instance.program,
            JWTDisplayManager.Instance.course,
            quizInfos.Lection,
            scoreCounter.lection_score,
            quizInfos.Mail
        );

        Debug.Log("Score successfully posted.");

        // Reset the score after posting
        if (score != null)
        {
            score.ResetScore();
            Debug.Log("Score has been reset.");
        }
        else
        {
            Debug.LogError("Score component not found. Unable to reset score.");
        }
    }
}