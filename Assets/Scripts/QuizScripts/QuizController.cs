using UnityEngine;

public class QuizController : MonoBehaviour
{
    public QuizAPIService apiService; // Referenz zum API-Service
    public int currentQuestionCount = 0;
    public API_CALL_Quiz apiCallQuiz;
    private const int maxQuestionsPerEnemy = 3;
    private string mail ="tom@one7.one";
    private string program ="Digital Business Engineering";
    private string course = "it";
    private string lection = "1";

    private int currentPosition = 2; // Startposition für das Quiz

    void Start()
    {
        // Vorläufiger Test-Aufruf, später kann dies vom Gegner aufgerufen werden
        StartQuizForEnemy(mail, program, course, lection, currentPosition);
    }

    // Methode zum Starten des Quiz für einen Gegner, flexibel aufrufbar
    public void StartQuizForEnemy(string email, string program, string course, string lection, int position)
    {
        currentPosition = position;
        currentQuestionCount = currentQuestionCount; // Setze die Frageanzahl für diesen Gegner zurück
        LoadNextQuestion();
    }

    // Diese Methode lädt die nächste Frage, ohne dass Argumente übergeben werden müssen
    public void LoadNextQuestion()
    {
        if (currentQuestionCount < maxQuestionsPerEnemy)
        {
            currentQuestionCount++;
            Debug.Log("Lade Quiz für Position: " + currentPosition);
            apiService.StartQuiz(mail, program, course, lection, currentPosition);
        }
        else
        {
            EndQuiz();
        }
    }

    // Diese Funktion wird aufgerufen, um zur nächsten Position zu wechseln und das nächste Quiz zu starten
    public void LoadNextQuiz()
    {
        currentPosition++; // Erhöhe die Position um 1
        Debug.Log("Erhöhe Position auf: " + currentPosition);
        StartQuizForEnemy(mail, program, course, lection, currentPosition); // Starte das nächste Quiz
    }

    private void EndQuiz()
    {
        
        Debug.Log($"Korrekte Antworten: {apiCallQuiz.CorrectAnswersCount}");
        Debug.Log($"Falsche Antworten: {apiCallQuiz.IncorrectAnswersCount}");
        Debug.Log("Das Quiz für diesen Gegner ist beendet.");
    }
}