using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;
using Platformer.Gameplay;
using Platformer.Core;
using Platformer.Model;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Referenz auf das UI-Text-Element, um den Timer anzuzeigen
    [SerializeField] float initialTime = 120f; // Startzeit, die manuell eingegeben wird
    [SerializeField] string startSceneName; // Name der Startszene, um sie beim Ablauf des Timers zu laden
    [SerializeField] float respawnDelay = 2f; // Verzögerung vor dem Respawn
    private float remainingTime; // Verbleibende Zeit
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
            StartCoroutine(HandleTimeUp()); // Zeit abgelaufen und Spielprozess verwalten
        }

        UpdateTimerDisplay(); // Aktualisiert die Anzeige des Timers
    }

    private IEnumerator HandleTimeUp()
    {
        isTiming = false; // Timer anhalten

        // Steuerung des Spielers deaktivieren
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.controlEnabled = false; // Deaktiviere die Steuerung des Spielers
        }

        // Zeige "Zeit abgelaufen!" in Weiß
        timerText.text = "Zeit abgelaufen!";
        timerText.color = Color.white;
        timerText.enableWordWrapping = false; // Deaktiviere den Zeilenumbruch

        // Warte für die Dauer der Verzögerung (z.B. 2 Sekunden)
        yield return new WaitForSeconds(respawnDelay);

        // Respawn den Spieler über die PlayerSpawn-Logik
        Simulation.Schedule<PlayerSpawn>();

        // Spielersteuerung wieder aktivieren
        if (player != null)
        {
            player.controlEnabled = true; // Steuerung wieder aktivieren
        }

        // Timer zurücksetzen
        ResetTimer();

        // Score zurücksetzen
        ResetScore();

        // Wenn ein Szenenname angegeben wurde, wechsle zu dieser Szene
        if (!string.IsNullOrEmpty(startSceneName))
        {
            SceneManager.LoadScene(startSceneName);
        }
        else
        {
            Debug.LogWarning("Kein Szenenname angegeben! Bitte setze den Startszene-Namen im Inspector.");
        }
    }

    private void ResetTimer()
    {
        remainingTime = initialTime; // Setzt die Zeit auf die manuell eingestellte Zeit zurück
        isTiming = true; // Startet den Timer neu
        UpdateTimerDisplay(); // Aktualisiert die Anzeige direkt nach dem Reset
    }

    private void ResetScore()
    {
        // Suche nach der Score-Komponente und rufe deren ResetScore() auf
        Score score = FindObjectOfType<Score>();
        if (score != null)
        {
            score.ResetScore(); // Setze den Score zurück
        }
        else
        {
            Debug.LogWarning("Score-Komponente konnte nicht gefunden werden!");
        }
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
