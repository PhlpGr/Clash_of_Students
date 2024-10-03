using UnityEngine;

public class Version2_QuizEnemy : MonoBehaviour
{
    private JWTData jwtData; // Hier speichern wir die JWT-Daten

    public string lection = "1"; // Die Lektion, die abgefragt wird
    public int position = 1;     // Die Position des Feindes im Level

    // Szenennamen für den Szenenwechsel
    public string mainSceneName = "Level_2_Tom"; // Name der Hauptspielszene

    private void Start()
    {
        // Überprüfe, ob der JWTDisplayManager existiert
        if (JWTDisplayManager.Instance != null)
        {
            jwtData = JWTDisplayManager.Instance.GetJWTData(); // Klassenvariable setzen
            if (jwtData != null)
            {
                Debug.Log("User's email: " + jwtData.email);
            }
        }
        else
        {
            Debug.Log("JWTDisplayManager Instance is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Überprüfen, ob der Spieler mit dem Feind kollidiert
        if (collision.CompareTag("Player"))
        {
            if (jwtData == null)
            {
                Debug.LogError("JWT-Daten sind nicht verfügbar. Quiz kann nicht gestartet werden.");
                return;
            }

            // Spieler kollidiert mit dem Gegner, das Quiz wird gestartet
            Debug.Log("Spieler kollidiert mit Gegner, Quiz wird gestartet.");

            // Erstelle Quiz-Informationen mit den JWT-Daten und anderen relevanten Informationen
            Infos quizInfos = new Infos(jwtData.professor_email, jwtData.program, jwtData.course, lection, position);
            //Debug.Log($"Mail: {jwtData.professor_email}");

            // Starte das Quiz mit den gegebenen Informationen und dem Szenennamen
            Version2_GameManager.Instance.StartQuiz(quizInfos, mainSceneName);
        }
    }
}