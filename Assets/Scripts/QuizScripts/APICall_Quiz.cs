using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class API_CALL_Quiz : MonoBehaviour
{
    public QuizTimer quizTimer;
    public TextMeshProUGUI frageText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;
    public QuizAPIService apiService;  // Referenz zum QuizAPIService

    private string correctAnswer;
    private int questionCount = 0;
    private const int maxQuestions = 3;

    private string email;
    private string program;
    private string course;
    private string lection;
    private int position;

    public class Fact
    {
        public string frage { get; set; }
        public string answer_a { get; set; }
        public string answer_b { get; set; }
        public string answer_c { get; set; }
        public string answer_d { get; set; }
        public string correct_answer { get; set; }
    }

    public void StartQuiz(string email, string program, string course, string lection, int position)
    {
        this.email = email;
        this.program = program;
        this.course = course;
        this.lection = lection;
        this.position = position;

        LoadNextQuestion();
    }

    private void LoadNextQuestion()
    {
        if (apiService == null)
        {
            Debug.LogError("apiService ist nicht zugewiesen!");
            return;
        }

        if (questionCount < maxQuestions)
        {
            questionCount++;
            string url = apiService.GenerateURL(email, program, course, lection, position);
            Debug.Log("Lade nächste Frage von URL: " + url); // Debugging
            StartCoroutine(GetRequest(url));
        }
        else
        {
            Debug.Log("Alle Fragen beantwortet. Gegner besiegt!");
            // Hier kannst du den Ablauf implementieren, der nach dem Beantworten aller Fragen erfolgt.
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
                Debug.Log("Daten erfolgreich abgerufen: " + webRequest.downloadHandler.text); // Debugging
                Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);

                if (fact != null)
                {
                    frageText.text = fact.frage;
                    string[] answers = { fact.answer_a, fact.answer_b, fact.answer_c, fact.answer_d };
                    correctAnswer = fact.correct_answer;

                    for (int i = 0; i < answerButtons.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(answers[i]))
                        {
                            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
                            answerButtons[i].gameObject.SetActive(true);
                            int index = i;
                            answerButtons[i].onClick.RemoveAllListeners();  // Vermeidung von doppelten Listenern
                            answerButtons[i].onClick.AddListener(() => CheckAnswer(answers[index]));
                        }
                        else
                        {
                            answerButtons[i].gameObject.SetActive(false);
                        }
                    }

                    quizTimer.ResetTimer();  // Timer zurücksetzen und neu starten
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
            LoadNextQuestion();
        }
        else
        {
            feedbackText.text = "Falsch!";
            feedbackText.color = Color.red;
            HandleIncorrectAnswer();
        }

        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }
    }

    public void HandleIncorrectAnswer()
    {
        feedbackText.text = "Falsch! Versuch es nochmal.";
        feedbackText.color = Color.red;
        StartCoroutine(RetryQuestion());
    }

    IEnumerator RetryQuestion()
    {
        yield return new WaitForSeconds(1f);
        quizTimer.ResetTimer();
        quizTimer.StartTimer();

        foreach (Button btn in answerButtons)
        {
            btn.interactable = true;
        }
        feedbackText.text = "";
    }
}
