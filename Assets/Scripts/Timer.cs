using UnityEngine;
using TMPro;
using System.Collections;
using Platformer.Mechanics;  // Wichtig für Coroutine

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Referenz auf das UI-Text-Element, um den Timer anzuzeigen
    [SerializeField] float initialTime = 120f; // Startzeit, die manuell eingegeben wird
    [SerializeField] Transform startPoint; // Referenz auf den Startpunkt
    private float remainingTime;
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
        remainingTime = initialTime; // Timer auf die initiale Zeit setzen
        PositionTimerText(); // Positioniere den Timer oben in der Mitte
        UpdateTimerDisplay(); // Aktualisiere den Timer direkt beim Start
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
            StartCoroutine(ShowTimeUpMessage()); // Coroutine starten, um den "Zeit abgelaufen!" Text anzuzeigen
            GameOver(); // Endet das Spiel, wenn die Zeit abgelaufen ist
        }

        UpdateTimerDisplay(); // Aktualisiert die Anzeige des Timers
    }

    private void GameOver()
    {
        isTiming = false; // Stoppt den Timer
        Time.timeScale = 1f; // Stelle sicher, dass das Spiel weiterläuft

        // Respawne den Spieler am Startpunkt
        Vector2 startPosition = startPoint.position; // Verwende die Position des Startpunkts
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.RespawnAtStart(startPosition);
        }

        // Timer zurücksetzen
        ResetTimer();

        // Score zurücksetzen (falls es ein Score-System gibt)
        Score score = FindObjectOfType<Score>();
        if (score != null)
        {
            score.ResetScore();
        }

        Debug.Log("Game Over - Spieler wird zurückgesetzt.");
    }

    private void ResetTimer()
    {
        remainingTime = initialTime; // Setzt die Zeit auf die manuell eingestellte Zeit zurück
        isTiming = true; // Startet den Timer neu
        UpdateTimerDisplay(); // Aktualisiert die Anzeige direkt nach dem Reset
    }

    private IEnumerator ShowTimeUpMessage()
    {
        // Setzt den Text auf "Zeit abgelaufen!" in weiß
        timerText.text = "Zeit abgelaufen!";
        timerText.color = Color.white;
        timerText.enableWordWrapping = false; // Deaktiviere den Zeilenumbruch

        // Warte eine halbe Sekunde (0.5f Sekunden)
        yield return new WaitForSeconds(0.5f);

        // Nach der Wartezeit wird die Farbe auf Rot geändert
        timerText.color = Color.red;
    }

    private void UpdateTimerDisplay()
    {
        if (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60); // Berechnet die Minuten
            int seconds = Mathf.FloorToInt(remainingTime % 60); // Berechnet die Sekunden

            // Zeige die verbleibende Zeit im MM:SS-Format an
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Setze die Farbe auf Rot, wenn weniger als 10 Sekunden übrig sind
            if (remainingTime < 10f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
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
