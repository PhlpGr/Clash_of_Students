using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("Richtige Antwort");
            quizManager.Correct();
        }
        else
        {
            Debug.Log("Falsche Antwort");
            quizManager.Correct();
        }
    }
}
