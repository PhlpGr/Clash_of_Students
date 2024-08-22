using UnityEngine;

public class QuizController : MonoBehaviour
{
    public QuizAPIService apiService; // Referenz zum API-Service

    void Start()
    {
        // Startet das Quiz über den API-Service
        apiService.StartQuiz();
    }
}
