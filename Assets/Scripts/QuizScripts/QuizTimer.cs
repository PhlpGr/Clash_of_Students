using TMPro;
using UnityEngine;

public class QuizTimer : MonoBehaviour
{
    public float totalTime = 30f; // Gesamtzeit in Sekunden
    private float remainingTime;
    public TextMeshProUGUI timerText; // Referenz zum Textfeld

    private bool timerIsRunning = false;
    public API_CALL_Quiz quizScript; // Referenz zum Quiz-Skript

    void Start()
    {
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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        if (quizScript != null)
        {
            Debug.Log("Zeit ist abgelaufen! Verarbeite Timer-Ende.");
            quizScript.HandleIncorrectAnswer();
        }
        else
        {
            Debug.LogError("quizScript ist nicht zugewiesen! Bitte überprüfe die Zuweisung im Inspektor.");
        }
    }
}