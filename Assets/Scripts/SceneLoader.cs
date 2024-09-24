using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string tagToCheck;
    public string sceneName;

    public void LoadMyScene(string sceneName)
    {
        // Überprüfe, ob der GameManager existiert, und setze den currentQuestionCount zurück
        if (Version2_GameManager.Instance != null)
        {
            Version2_GameManager.Instance.ResetQuestionCount();
        }
        else
        {
            Debug.LogWarning("Version2_GameManager Instance ist null!");
        }

        // Lade die Szene
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // is it the player
        if(collision.CompareTag(tagToCheck))
        {
            LoadMyScene(sceneName);
        }
    }
}
