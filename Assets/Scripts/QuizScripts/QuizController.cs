using UnityEngine;

public class QuizController : MonoBehaviour
{
    public QuizAPIService apiService; // Referenz zum API-Service

    void Start()
    {
        // Definiere die Parameter
        string email = "tom@one7.one";
        string program = "Digital Business Engineering";
        string course = "it";
        string lection = "1";
        int position = 1;

        // Übergib die Parameter an die StartQuiz-Methode
        apiService.StartQuiz(email, program, course, lection, position);
    }
}
