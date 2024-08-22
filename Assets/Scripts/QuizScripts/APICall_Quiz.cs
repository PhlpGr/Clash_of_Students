using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class API_CALL_Quiz : MonoBehaviour
{
    public QuizTimer quizTimer;  // Referenz zum Timer
    public TextMeshProUGUI frageText;  // UI-Textfeld für die Frage
    public Button[] answerButtons;     // Array von Buttons für die Antworten
    public TextMeshProUGUI feedbackText; // Textfeld für Feedback nach der Auswahl

    private string correctAnswer;      // Speichert die richtige Antwort
    private int attemptCount = 0;      // Zählt die Versuche

    // Füge die Definition der Fact-Klasse hier ein
    public class Fact
    {
        public int id { get; set; }
        public string user { get; set; }
        public string question_type { get; set; }
        public string frage { get; set; }
        public string answer_a { get; set; }
        public string answer_b { get; set; }
        public string answer_c { get; set; }
        public string answer_d { get; set; }
        public string correct_answer { get; set; }
        public string program { get; set; }
        public string course { get; set; }
        public string lection { get; set; }
        public int position { get; set; }
        public string image_url { get; set; }
    }

    public void StartQuiz(string apiURL)
    {
        if (!string.IsNullOrEmpty(apiURL))
        {
            // API-Call starten
            StartCoroutine(GetRequest(apiURL));
        }
        else
        {
            Debug.LogError("API URL ist nicht gesetzt!");
        }
    }

    IEnumerator GetRequest(string URL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Fehler: {webRequest.error}");
            }
            else if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);

                if (fact != null)
                {
                    // Frage setzen
                    frageText.text = fact.frage;

                    // Antworten setzen
                    string[] answers = { fact.answer_a, fact.answer_b, fact.answer_c, fact.answer_d };
                    correctAnswer = fact.correct_answer;

                    for (int i = 0; i < answerButtons.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(answers[i]))
                        {
                            // Antworttext setzen
                            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
                            answerButtons[i].gameObject.SetActive(true);

                            // Button-Click-Listener hinzufügen
                            int index = i;
                            answerButtons[i].onClick.AddListener(() => CheckAnswer(answers[index]));
                        }
                        else
                        {
                            // Button deaktivieren, wenn keine Antwort vorhanden ist
                            answerButtons[i].gameObject.SetActive(false);
                        }
                    }

                    // Timer starten
                    quizTimer.StartTimer();
                }
                else
                {
                    Debug.LogError("Die API-Antwort konnte nicht deserialisiert werden.");
                }
            }
        }
    }

    public void CheckAnswer(string selectedAnswer)
    {
        quizTimer.StopTimer(); // Timer stoppen

        if (selectedAnswer == correctAnswer)
        {
            feedbackText.text = "Richtig!";
            feedbackText.color = Color.green;
            Debug.Log("Antwort richtig!");
            // Hier könnte der Übergang zur nächsten Frage erfolgen
        }
        else
        {
            HandleIncorrectAnswer();
        }

        // Alle Buttons nach einer Auswahl deaktivieren
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }
    }

    public void HandleIncorrectAnswer()
    {
        attemptCount++;

        if (attemptCount == 1)
        {
            feedbackText.text = "Falsch! Versuch es nochmal.";
            feedbackText.color = Color.red;
            Debug.Log("Erster Fehlversuch, Frage wird neu gestellt.");
            StartCoroutine(RetryQuestion());
        }
        else
        {
            feedbackText.text = "Zweite falsche Antwort! Nächste Frage...";
            feedbackText.color = Color.red;
            Debug.Log("Zweiter Fehlversuch, zur nächsten Frage übergehen.");
            // Hier den Übergang zur nächsten Frage implementieren
        }
    }

    IEnumerator RetryQuestion()
    {
        yield return new WaitForSeconds(1f); // Optional: kurze Wartezeit, bevor die Frage neu geladen wird
        quizTimer.ResetTimer(); // Timer zurücksetzen
        quizTimer.StartTimer(); // Timer neu starten

        // Reaktivieren der Buttons
        foreach (Button btn in answerButtons)
        {
            btn.interactable = true;
        }
        feedbackText.text = ""; // Feedback zurücksetzen
    }
}
