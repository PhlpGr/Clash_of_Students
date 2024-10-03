using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(QuizManager))]
public class QuizManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        QuizManager quizManager = (QuizManager)target;

        // Zeige das Standard-Inspector-Fenster für QuizManager an
        DrawDefaultInspector();

        GUILayout.Space(10); // Abstand im Inspector

        // Benutzerdefinierte Felder für Fragen und Antworten
        if (GUILayout.Button("Add Question"))
        {
            quizManager.QnA.Add(new QuestionsAndAnswers());
        }
    }
}
#endif
public class QuizManager : MonoBehaviour
{
    public List<QuestionsAndAnswers> QnA = new List<QuestionsAndAnswers>();
    public GameObject[] options;
    public int currentQuestion;

    public TextMeshProUGUI QuestionTxt;
    private int answeredQuestionsCount = 0;
    public string sceneName;
    private void Start()
    {
        generateQuestion();
    }
    
    public void Correct()
    {
        QnA.RemoveAt(currentQuestion);
        answeredQuestionsCount++;
            if (answeredQuestionsCount == 3) // Wenn der Spieler die dritte Frage richtig beantwortet hat
                {
                SceneManager.LoadScene("Level_1.2");
                }
            else
                {
                generateQuestion();
                }
    }

    public void False()
    {
        QnA.RemoveAt(currentQuestion);
        SceneManager.LoadScene("Level_1.1");
    }

    void SetAnswer() 
    {
        /*for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            int correctAnswer;
            if (int.TryParse(QnA[currentQuestion].CorrectAnswer, out correctAnswer) && correctAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
            Debug.Log($"Option {i + 1}: {options[i].transform.GetChild(0).GetComponent<Text>().text}");
        }*/
        for (int i = 0; i < options.Length; i++)
    {
        if (options[i] != null && options[i].transform.childCount > 0)
        {
            TextMeshProUGUI textComponent = options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            if (textComponent != null)
            {
                textComponent.text = QnA[currentQuestion].Answers[i];

                int correctAnswer;
                if (int.TryParse(QnA[currentQuestion].CorrectAnswer, out correctAnswer) && correctAnswer == i)
                {
                    options[i].GetComponent<AnswerScript>().isCorrect = true;
                }
                else
                {
                    options[i].GetComponent<AnswerScript>().isCorrect = false;
                }
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on the child of option " + i);
            }
        }
        else
        {
            Debug.LogError("Option " + i + " or its child is null");
        }
    }
    }

    void generateQuestion()
    {
        currentQuestion = Random.Range(0, QnA.Count);
        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswer();
        

    }
}
