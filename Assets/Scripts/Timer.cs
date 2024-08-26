using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Referenz auf das UI-Text-Element, um den Timer anzuzeigen
    [SerializeField] float remainingTime; // Manuell einstellbare Startzeit in Sekunden
    private bool isTiming = true; // Timer läuft standardmäßig

    private static Timer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Verhindert, dass dieses Objekt beim Szenenwechsel zerstört wird
        }
        else
        {
            Destroy(gameObject); // Zerstört das neue Objekt, um sicherzustellen, dass nur ein Timer existiert
        }
    }

    void Start()
    {
        PositionTimerText(); // Positioniere den Timer oben in der Mitte
    }

    void Update()
    {
        if (isTiming && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Reduziert die verbleibende Zeit
        }
        else if (remainingTime <= 0 && isTiming)
        {
            remainingTime = 0; // Stellt sicher, dass der Timer nicht unter 0 geht
            GameOver(); // Endet das Spiel, wenn die Zeit abgelaufen ist
        }

        UpdateTimerDisplay(); // Aktualisiert die Anzeige des Timers
    }

    private void GameOver()
    {
        isTiming = false; // Stoppt den Timer
        timerText.color = Color.red; // Setzt die Textfarbe auf Rot

        // Stellt sicher, dass der Text in einer Zeile bleibt
        timerText.enableWordWrapping = false; // Deaktiviert den Zeilenumbruch
        timerText.overflowMode = TextOverflowModes.Overflow; // Ermöglicht das Überlaufen des Textes, falls er zu lang ist
        
        timerText.text = "Zeit abgelaufen!"; // Zeigt "Zeit abgelaufen!" in einer Zeile an

        // Hier wird das Spiel angehalten
        Time.timeScale = 0f;

        Debug.Log("Game Over!"); // Debug-Nachricht für Game Over

        // Optional: Lade eine "Game Over"-Szene oder zeige ein Game Over-Menü
        // Beispiel:
        // SceneManager.LoadScene("GameOverScene");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelEnd"))
        {
            isTiming = false;
            remainingTime = 0;
            timerText.color = Color.green;
            // Hier kannst du Code hinzufügen, um den nächsten Level zu laden oder den Timer zurückzusetzen.
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetPlayerPosition(); // Der Timer läuft weiter, wenn der Spieler auf ein Hindernis trifft
        }
    }

    private void ResetPlayerPosition()
    {
        // Hier kannst du die Logik hinzufügen, um die Position des Spielers zurückzusetzen
        Debug.Log("Player position reset, timer continues.");
    }

    private void UpdateTimerDisplay()
    {
        if (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60); // Berechnet die Minuten
            int seconds = Mathf.FloorToInt(remainingTime % 60); // Berechnet die Sekunden
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Aktualisiert den Text im MM:SS-Format
        }
    }

private void PositionTimerText()
{
    RectTransform rectTransform = timerText.GetComponent<RectTransform>();
    
    // Ändere die Ankerpunkte, um den Timer in der Mitte des Canvas zu positionieren
    rectTransform.anchorMin = new Vector2(0.5f, 0.5f); // Ankerpunkt Mitte
    rectTransform.anchorMax = new Vector2(0.5f, 0.5f); // Ankerpunkt Mitte
    rectTransform.pivot = new Vector2(0.5f, 0.5f);     // Pivot auch in der Mitte
    
    // Diese Werte passen die Position relativ zum Ankerpunkt an
    rectTransform.anchoredPosition = new Vector2(100, 175); // X-Wert erhöht für weiter rechts, Y-Wert negativ für leichtes Nach-unten-Verschieben

    rectTransform.sizeDelta = new Vector2(200, 50); // Größe anpassen, falls nötig
}



}
