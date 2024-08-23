using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

public class API_CALL_Quiz : MonoBehaviour
{
    public QuizTimer quizTimer;
    public TextMeshProUGUI frageText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;
    public GameObject loadingPanel;
    public QuizController quizController; // Referenz zum QuizController

    private string correctAnswer;
    private int attemptCount = 0;
    private int correctAnswersCount = 0;
    private int incorrectAnswersCount = 0;
    public int CorrectAnswersCount { get { return correctAnswersCount; } }
    public int IncorrectAnswersCount { get { return incorrectAnswersCount; } }

    public void StartQuiz(string apiURL)
    {
        Debug.Log("StartQuiz aufgerufen mit URL: " + apiURL);
      
        if (!string.IsNullOrEmpty(apiURL))
        {
            ShowLoading(true);
            StartCoroutine(GetRequest(apiURL));
        }
        else
        {
            Debug.LogError("API URL ist nicht gesetzt!");
        }
    }

    IEnumerator GetRequest(string URL)
    {
        Debug.Log("API-Anfrage gestartet für URL: " + URL);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            yield return webRequest.SendWebRequest();

            ShowLoading(false);

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Fehler bei der API-Anfrage: {webRequest.error}");
            }
            else if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API-Anfrage erfolgreich, Antwort erhalten.");

                Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);

                if (fact != null)
                {
                    Debug.Log("Frage erhalten: " + fact.frage);
                    LoadQuestion(fact);
                }
                else
                {
                    Debug.LogError("Die API-Antwort konnte nicht deserialisiert werden.");
                }
            }
        }
    }

    private void LoadQuestion(Fact fact)
    {
        Debug.Log("Frage wird geladen: " + fact.frage);

        frageText.text = fact.frage;

        string[] answers = { fact.answer_a, fact.answer_b, fact.answer_c, fact.answer_d };
        
        correctAnswer = fact.correct_answer;

        ActivateButtons(answerButtons);
        attemptCount = 0;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].onClick.RemoveAllListeners();

            if (!string.IsNullOrEmpty(answers[i]))
            {
                Debug.Log("Antwort " + (i + 1) + ": " + answers[i]);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
                answerButtons[i].gameObject.SetActive(true);

                int index = i;
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answers[index]));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        quizTimer.ResetTimer();
        quizTimer.StartTimer();
    }

    public void CheckAnswer(string selectedAnswer)
    {
        quizTimer.StopTimer();
        Debug.Log("Antwort ausgewählt: " + selectedAnswer);

        if (selectedAnswer == correctAnswer)
        {
            feedbackText.text = "Richtig!";
            feedbackText.color = Color.green;
            Debug.Log("Richtige Antwort ausgewählt!");
            correctAnswersCount++; 
            Invoke(nameof(NotifyQuizControllerToLoadNextRound), 1.5f);
        }
        else
        {
            Debug.Log("Falsche Antwort ausgewählt!");
            incorrectAnswersCount++;
            HandleIncorrectAnswer();
        }

        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }
    }

    private void NotifyQuizControllerToLoadNextRound()
    {
        if (quizController != null)
        {
            Debug.Log("Nächste Runde wird gestartet.");
            // Hier erhöhen wir die Position und starten das nächste Quiz
            quizController.LoadNextQuiz();
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
            Invoke(nameof(NotifyQuizControllerToLoadNextRound), 1.5f);
        }
    }
    IEnumerator RetryQuestion()
    {
        yield return new WaitForSeconds(1f);
        quizTimer.ResetTimer();
        quizTimer.StartTimer();

        ActivateButtons(answerButtons);
        
    }

    private void ActivateButtons(Button[] answerButtons){
        feedbackText.text = "";
        foreach (Button btn in answerButtons)
        {
            btn.interactable = true;
        }


    }

    private void ShowLoading(bool isLoading)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(isLoading);
        }
    }
}
