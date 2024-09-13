using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizTimer : MonoBehaviour
{
    public float totalTime = 60f; // Gesamtzeit in Sekunden
    private float remainingTime;
    private TextMeshProUGUI timerText; // Referenz zum Textfeld

    private bool timerIsRunning = false;
    public Version2_GameManager gameManager; // Referenz zum GameManager

    void Start()
    {
        // Versuche, die Referenz auf den GameManager zu finden, falls sie nicht gesetzt ist
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<Version2_GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager-Referenz ist nicht zugewiesen! Bitte überprüfe die Zuweisung im Inspektor.");
            }
        }

        timerIsRunning = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI(remainingTime);
            }
            else
            {
                remainingTime = 0;
                timerIsRunning = false;
                TimerEnded();
            }
        }
    }

    public void StartTimer()
    {
        remainingTime = totalTime;
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        timerIsRunning = false; // Timer anhalten
    }

    public void ResetTimer()
    {
        remainingTime = totalTime;
        UpdateTimerUI(remainingTime); // Aktualisiert das UI sofort
    }

    void UpdateTimerUI(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText = GameObject.Find("Timer Text").GetComponent<TextMeshProUGUI>();

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        if (gameManager != null)
        {
            Debug.Log("Zeit ist abgelaufen! Verarbeite Timer-Ende.");
            gameManager.HandleTimerEnd(); // Aufruf einer speziellen Methode im GameManager für Timer-Ende
        }
        else
        {
            Debug.LogError("GameManager-Referenz ist nicht zugewiesen! Bitte überprüfe die Zuweisung im Inspektor.");
        }
    }
}
