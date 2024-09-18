using UnityEngine;

public class Version2_QuizEnemy : MonoBehaviour
{
    public static JWTDisplayManager JWTDManager; // Manager zur Verwaltung der JWT-Daten
    private string email = "tom@one7.one";//JWTDManager.email;
    private string professor_email = "tom@one7.one";//JWTDManager.professor_email;
    private string program = "Digital Business Engineering"; //JWTDManager.program;
    private string course = "it"; //JWTDManager.course;
    public string lection = "1";
    public int position = 1;

    // Szenennamen für den Szenenwechsel
    public string mainSceneName = "Level_2_Tom"; // Name der Hauptspielszene
    //private string quizSceneName = "Quiz"; // Name der Quizszene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Nutze den GameManager, um das Quiz zu starten
            Debug.Log("Spieler kollidiert mit Gegner, Quiz wird gestartet.");
            Infos quizInfos = new Infos(email, program, course, lection, position);
            Debug.Log($"mail: {email}");

            // Starte das Quiz mit den gegebenen Informationen und Szenennamen
            Debug.Log($"{quizInfos + mainSceneName}");
            Version2_GameManager.Instance.StartQuiz(quizInfos, mainSceneName);
        }
    }
}
