using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Version2_GameManager : MonoBehaviour
{
    public static Version2_GameManager Instance;
    public QuizTimer quizTimer;
    private Infos quizInfos;
    private string mainSceneName;
    private const string quizSceneName = "Quiz"; // Konstanten für die Szenennamen
    private const int maxQuestionsPerEnemy = 2;

    private int attemptCount = 0; // Zähler für die Anzahl der Fehlversuche
    private int currentQuestionCount = 0;
    private int correctAnswersCount = 0;
    private int incorrectAnswersCount = 0;
    private string url = "";

    private TextMeshProUGUI frageText;
    private TextMeshProUGUI feedbackText;
    private Button answerButton1;
    private Button answerButton2;
    private Button answerButton3;
    private Button answerButton4;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuiz(Infos quizInfos, string mainSceneName)
    {
        this.quizInfos = quizInfos;
        this.mainSceneName = mainSceneName;

        // Generiere die URL einmal hier
        url = GenerateURL(quizInfos.Mail, quizInfos.Program, quizInfos.Course, quizInfos.Lection, quizInfos.CurrentPosition);
        Debug.Log("Generierte URL: " + url);

        SceneManager.sceneLoaded += OnQuizSceneLoaded;
        SceneManager.LoadScene(quizSceneName);
    }

    private void OnQuizSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == quizSceneName)
        {
            SceneManager.sceneLoaded -= OnQuizSceneLoaded;

            // Buttons dynamisch zuweisen
            AssignButtonsDynamically();

            StartCoroutine(LoadQuestion(url)); // Verwende die zuvor generierte URL
        }
    }

    private void AssignButtonsDynamically()
    {
        // Finde die Buttons anhand ihrer Namen in der Szene
        answerButton1 = GameObject.Find("Antwort1").GetComponent<Button>();
        answerButton2 = GameObject.Find("Antwort2").GetComponent<Button>();
        answerButton3 = GameObject.Find("Antwort3").GetComponent<Button>();
        answerButton4 = GameObject.Find("Antwort4").GetComponent<Button>();

        // Überprüfe, ob die Buttons gefunden wurden
        if (answerButton1 == null || answerButton2 == null || answerButton3 == null || answerButton4 == null)
        {
            Debug.LogError("Mindestens ein Button konnte in der Szene nicht gefunden werden. Bitte überprüfe die Namen der Buttons in der Szene.");
        }
    }

    private IEnumerator LoadQuestion(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Fehler bei der API-Anfrage: {webRequest.error}");
                yield break;
            }

            Debug.Log("API-Antwort erhalten: " + webRequest.downloadHandler.text);
            Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);

            if (fact != null)
            {
                DisplayQuestion(fact);
            }
            else
            {
                Debug.LogError("Die API-Antwort konnte nicht deserialisiert werden.");
            }
        }
    }

    private void DisplayQuestion(Fact fact)
    {
        frageText = GameObject.Find("Question_Info").GetComponent<TextMeshProUGUI>();
        feedbackText = GameObject.Find("ScoreCounter").GetComponent<TextMeshProUGUI>();

        frageText.text = fact.frage;

        // Setze die Antworttexte und aktiviere die Buttons
        SetupButton(answerButton1, fact.answer_a, fact.correct_answer);
        SetupButton(answerButton2, fact.answer_b, fact.correct_answer);
        SetupButton(answerButton3, fact.answer_c, fact.correct_answer);
        SetupButton(answerButton4, fact.answer_d, fact.correct_answer);

        // Starte den Timer für die Frage
        quizTimer.ResetTimer();
        quizTimer.StartTimer();
    }

    private void SetupButton(Button button, string answerText, string correctAnswer)
    {
        if (button == null)
        {
            Debug.LogError("Button ist nicht zugewiesen! Bitte überprüfe die Button-Referenzen im Inspektor.");
            return;
        }

        button.onClick.RemoveAllListeners(); // Entferne vorherige Listener

        if (!string.IsNullOrEmpty(answerText))
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null)
            {
                Debug.LogError("TextMeshProUGUI-Komponente nicht im Button gefunden: " + button.name);
                return;
            }

            buttonText.text = answerText;
            button.gameObject.SetActive(true);
            button.interactable = true; // Mache den Button klickbar

            // Listener hinzufügen, um die Antwort zu prüfen
            button.onClick.AddListener(() => CheckAnswer(answerText, correctAnswer));
        }
        else
        {
            button.gameObject.SetActive(false); // Deaktiviere den Button, wenn keine Antwort vorhanden ist
        }
    }

    public void CheckAnswer(string selectedAnswer, string correctAnswer)
    {
        quizTimer.StopTimer(); // Stoppe den Timer
        Debug.Log("Antwort ausgewählt: " + selectedAnswer);
    
        // Deaktiviere alle Buttons nach der Auswahl einer Antwort
        DisableAllButtons();
    
        if (selectedAnswer == correctAnswer)
        {
            feedbackText.text = "Richtig!";
            feedbackText.color = Color.green;
            Debug.Log("Richtige Antwort ausgewählt!");
            correctAnswersCount++; // Erhöhe den Zähler für richtige Antworten
            StartCoroutine(HideFeedbackText());
            Invoke(nameof(LoadNextQuestion), 1.5f); // Lade die nächste Frage nach 1,5 Sekunden
        }
        else
        {
            Debug.Log("Falsche Antwort ausgewählt!");
            incorrectAnswersCount++; // Erhöhe den Zähler für falsche Antworten
            HandleIncorrectAnswer(); // Behandle den falschen Antwortfall
        }
    }


    private void DisableAllButtons()
    {
        answerButton1.interactable = false;
        answerButton2.interactable = false;
        answerButton3.interactable = false;
        answerButton4.interactable = false;
    }

    private void EnableAllButtons()
    {
        answerButton1.interactable = true;
        answerButton2.interactable = true;
        answerButton3.interactable = true;
        answerButton4.interactable = true;
    }
    
    private void LoadNextQuestion()
    {
        if (currentQuestionCount < maxQuestionsPerEnemy)
        {
            currentQuestionCount++;
            attemptCount = 0;
            quizInfos.CurrentPosition++; // Erhöhe die CurrentPosition um 1 für die nächste Frage
            url = GenerateURL(quizInfos.Mail, quizInfos.Program, quizInfos.Course, quizInfos.Lection, quizInfos.CurrentPosition);
            Debug.Log("Neue generierte URL für nächste Frage: " + url);
            StartCoroutine(LoadQuestion(url)); // Verwende die aktualisierte URL
        }
        else
        {
            EndQuiz();
        }
    }

    public void HandleTimerEnd()
    {
        Debug.Log("Timer ist abgelaufen! Behandle Timer-Ende.");
        feedbackText.text = "Zeit abgelaufen!";
        feedbackText.color = Color.red;
        StartCoroutine(HideFeedbackText());
        incorrectAnswersCount++; // Erhöhe den Zähler für falsche Antworten
        HandleIncorrectAnswer(); // Behandle den Fall einer falschen Antwort
    }

    
    
    public void HandleIncorrectAnswer()
    {
        attemptCount++; // Erhöhe den Fehlversuchszähler
    
        if (attemptCount == 1)
        {
            // Erster Fehlversuch
            feedbackText.text = "Falsch! Versuch es nochmal.";
            feedbackText.color = Color.red;
            StartCoroutine(HideFeedbackText());
            StartCoroutine(RetryQuestion());
        }
        else if (attemptCount >= 2)
        {
            // Zweiter Fehlversuch oder mehr
            feedbackText.text = "Zweite falsche Antwort! Nächste Frage...";
            feedbackText.color = Color.red;
            StartCoroutine(HideFeedbackText());
            Invoke(nameof(LoadNextQuestion), 1.5f); // Lade die nächste Frage nach 1,5 Sekunden
            attemptCount = 0; // Setze den Fehlversuchszähler zurück
        }
    }




    private IEnumerator RetryQuestion()
    {
        yield return new WaitForSeconds(1f);
        quizTimer.ResetTimer();
        quizTimer.StartTimer();

        // Mache die Buttons wieder klickbar für den erneuten Versuch
        EnableAllButtons();
    }




    public string GenerateURL(string email, string program, string course, string lection, int position)
    {
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}", "http://localhost:1999/api/questions", email, program, course, lection, position);
    }

    private IEnumerator HideFeedbackText()
    {
        yield return new WaitForSeconds(1.5f);
        feedbackText.text = ""; // Setzt den Feedback-Text zurück
    }

    public void EndQuiz()
    {
        Debug.Log($"Korrekte Antworten: {correctAnswersCount}");
        Debug.Log($"Falsche Antworten: {incorrectAnswersCount}");
        Debug.Log("Das Quiz ist beendet.");
        SceneManager.LoadScene(mainSceneName); // Zurück zur Hauptspielszene
    }



    
}    
    
    