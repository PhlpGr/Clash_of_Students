using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Referenz auf das UI-Text-Element, um den Timer anzuzeigen
    [SerializeField] float initialTime; // Manuell einstellbare Startzeit in Sekunden
    [SerializeField] string startSceneName; // Name der Startszene, der im Editor festgelegt wird
    [SerializeField] Color defaultColor = Color.white; // Standardfarbe des Timer-Textes
    [SerializeField] Color warningColor = Color.red; // Farbe für die letzten 10 Sekunden
    [SerializeField] Color gameOverColor = Color.white; // Farbe für den "Zeit abgelaufen"-Text

    private float remainingTime; // Aktuell verbleibende Zeit
    private bool isTiming = true; // Timer läuft standardmäßig
    private static Timer instance;

    // Referenz auf das PatrolPath-Objekt
    public GameObject patrolPathObject;

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

        if (patrolPathObject != null)
        {
            DontDestroyOnLoad(patrolPathObject); // Verhindert, dass PatrolPath beim Szenenwechsel zerstört wird
        }
    }

    void Start()
    {
        ResetTimer(); // Setzt den Timer bei Spielstart zurück
        PositionTimerText(); // Positioniere den Timer oben in der Mitte
        InitializePatrolPath(); // Initialisiere das PatrolPath
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
            //GameOver(); // Endet das Spiel, wenn die Zeit abgelaufen ist
        }

        UpdateTimerDisplay(); // Aktualisiert die Anzeige des Timers
    }

    private IEnumerator GameOver()
    {
        isTiming = false; // Stoppt den Timer

        // Setze den Text auf "Zeit abgelaufen!" und wechsle die Farbe auf Weiß
        timerText.text = "Zeit abgelaufen!";
        timerText.color = gameOverColor;

        Debug.Log("Game Over! Restarting...");

        // Warte 2 Sekunden, bevor das Spiel zurückgesetzt wird
        yield return new WaitForSeconds(2f);

        // Lade die Startszene neu
        if (!string.IsNullOrEmpty(startSceneName))
        {
            SceneManager.LoadScene(startSceneName); // Lädt die Szene mit dem angegebenen Namen
            StartCoroutine(InitializeAfterSceneLoad());
        }
        else
        {
            Debug.LogError("Startszene wurde nicht festgelegt.");
        }
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f); // Warte kurz, bis die Szene vollständig geladen ist
        InitializePatrolPath(); // Stelle sicher, dass das PatrolPath neu referenziert wird
    }

    private void InitializePatrolPath()
    {
        if (patrolPathObject == null)
        {
            patrolPathObject = GameObject.Find("PatrolPath"); // Suche das Objekt, falls es noch nicht referenziert wurde
        }

        if (patrolPathObject != null)
        {
            DontDestroyOnLoad(patrolPathObject); // Stelle sicher, dass das Objekt nicht zerstört wird
        }
        else
        {
            Debug.LogWarning("PatrolPath-Objekt konnte nicht gefunden werden!");
        }
    }

    private void UpdateTimerDisplay()
    {
        if (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60); // Berechnet die Minuten
            int seconds = Mathf.FloorToInt(remainingTime % 60); // Berechnet die Sekunden

            // Wenn weniger als 10 Sekunden verbleiben, wird die Textfarbe rot
            if (remainingTime <= 10f)
            {
                timerText.color = warningColor;
            }

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Aktualisiert den Text im MM:SS-Format
        }
    }

    private void ResetTimer()
    {
        remainingTime = initialTime; // Setzt die Zeit auf den Anfangswert zurück
        isTiming = true; // Timer läuft standardmäßig weiter
        timerText.color = defaultColor;
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
