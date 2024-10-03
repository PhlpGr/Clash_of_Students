using UnityEngine;
using QuizScripts; // Für den Zugriff auf ScoreCounter
using System.Collections; // Für IEnumerator und Coroutines

public class LevelEndManager : MonoBehaviour
{
    private ScoreCounter scoreCounter; // Referenz auf den zentralen ScoreCounter
    private Infos quizInfos; // Informationen zum aktuellen Quiz
    private Score score; // Reference to the Score component
    public static JWTDisplayManager JWTDManager;
    private JWTData jwtData; // Hier speichern wir die JWT-Daten

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

    // **Newly added method to post the score and reset**
    public void PostScoreAndReset()
    {
        // Start the coroutine to post the score
        StartCoroutine(PostFinalScore());
    }

    // Modified `PostFinalScore` coroutine to reset the score after posting
    private IEnumerator PostFinalScore()
    {
        Debug.Log("Starting PostFinalScore Coroutine...");

        // Abrufen der JWT-Daten über den JWTDisplayManager
        if (JWTDisplayManager.Instance != null){
            JWTData data = JWTDisplayManager.Instance.GetJWTData();
            if (data != null)
            {
                Debug.Log("User's email: " + data.email);
            }
        }
        else {
            Debug.Log("JWTDisplayManager Instance is null");
        }

        Debug.Log(jwtData.professor_email + jwtData.program + jwtData.course);
        Debug.Log(quizInfos.Lection);
        Debug.Log(quizInfos.Mail);
        Debug.Log(scoreCounter.lection_score);

        yield return scoreCounter.PostScoreAsync(
            jwtData.email,
            jwtData.program,
            jwtData.course,
            quizInfos.Lection,
            scoreCounter.lection_score,
            quizInfos.Mail
        );

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