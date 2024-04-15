using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitQuiz : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(8);
    }
}
