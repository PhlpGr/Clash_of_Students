using UnityEngine;

public class Version2_QuizEnemy : MonoBehaviour
{
    private JWTData jwtData; // Hier speichern wir die JWT-Daten

    public string lection = "1";
    public int position = 1;

    // Szenennamen für den Szenenwechsel
    public string mainSceneName = "Level_2_Tom"; // Name der Hauptspielszene

    private void Start()
    {
        /*
        // Überprüfe, ob JWTDisplayManager existiert
        if (JWTDisplayManager.Instance == null)
        {
            Debug.LogError("JWTDisplayManager konnte nicht gefunden werden.");
            return;
        }

        // Hole die JWT-Daten vom JWTDisplayManager
        jwtData = JWTDisplayManager.Instance.GetJWTData();
        */
        jwtData = new JWTData
        {
            email = "1",
            firstname = "John",
            lastname = "Doe",
            professor_email = "professor@example.com",
            program = "DBE",
            course = "1"
        };

        if (jwtData == null)
        {
            Debug.LogError("JWT-Daten konnten nicht abgerufen werden.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (jwtData == null)
            {
                Debug.LogError("JWT-Daten sind nicht verfügbar. Quiz kann nicht gestartet werden.");
                return;
            }

            // Nutze den GameManager, um das Quiz zu starten
            Debug.Log("Spieler kollidiert mit Gegner, Quiz wird gestartet.");
            Infos quizInfos = new Infos(jwtData.professor_email, jwtData.program, jwtData.course, lection, position);
            Debug.Log($"Mail: {jwtData.professor_email}");

            // Starte das Quiz mit den gegebenen Informationen und Szenennamen
            Debug.Log($"{quizInfos + mainSceneName}");
            Version2_GameManager.Instance.StartQuiz(quizInfos, mainSceneName);
        }
    }
}

