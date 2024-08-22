using UnityEngine;

public class QuizAPIService : MonoBehaviour
{
    public string baseURL = "http://localhost:1999/api/questions";
    public API_CALL_Quiz quizScript; // Referenz zum API_CALL_Quiz-Skript

    public void StartQuiz(string email, string program, string course, string lection, int position)
    {
        // Generiere die URL dynamisch
        string url = GenerateURL(email, program, course, lection, position);

        // Starte das Quiz, indem die URL an API_CALL_Quiz übergeben wird
        quizScript.StartQuiz(url); // Beachte, dass nur die URL übergeben wird
    }

    public string GenerateURL(string email, string program, string course, string lection, int position)
    {
        // Erzeugt die dynamische URL basierend auf den übergebenen Parametern
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}", baseURL, email, program, course, lection, position);
    }
}
