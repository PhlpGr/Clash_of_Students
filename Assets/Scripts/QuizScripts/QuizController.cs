using UnityEngine;

public class QuizController : MonoBehaviour
{
    public QuizAPIService apiService; // Referenz zum API-Service

    // Diese Methode wird von Unity beim Starten der Szene aufgerufen
    void Start()
    {
        // Initialisierungslogik oder Aufruf des Quiz-Starts
        string email = "tom@one7.one";
        string program = "Digital Business Engineering";
        string course = "it";
        string lection = "1";
        int position = 1;

        apiService.StartQuiz(email, program, course, lection, position);

    }
}
