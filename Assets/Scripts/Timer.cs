using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Referenz auf das UI-Text-Element, um den Timer anzuzeigen
    [SerializeField] float remainingTime; // Manuell einstellbare Startzeit in Sekunden
    private bool isTiming = true; // Timer läuft standardmäßig

    void Start()
    {
        PositionTimerText(); // Positioniere den Timer oben in der Mitte
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Reduziert die verbleibende Zeit
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0; // Stellt sicher, dass der Timer nicht unter 0 geht
            GameOver(); // Endet das Spiel, wenn die Zeit abgelaufen ist
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60); // Berechnet die Minuten
        int seconds = Mathf.FloorToInt(remainingTime % 60); // Berechnet die Sekunden
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Aktualisiert den Text im MM:SS-Format
    }

    private void GameOver()
    {
        isTiming = false; // Stoppt den Timer
        timerText.color = Color.red; // Setzt die Textfarbe auf Rot

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

    private void PositionTimerText()
    {
        // Positioniere das Timer-Text-Element oben in der Mitte des Bildschirms
        RectTransform rectTransform = timerText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1.0f); // Setzt den Anker auf die obere Mitte
        rectTransform.anchorMax = new Vector2(0.5f, 1.0f); // Setzt den Anker auf die obere Mitte
        rectTransform.pivot = new Vector2(0.5f, 1.0f); // Setzt das Pivot-Punkt auf die obere Mitte
        rectTransform.anchoredPosition = new Vector2(0, -30); // Setzt den Text leicht unterhalb des oberen Randes
    }
}
