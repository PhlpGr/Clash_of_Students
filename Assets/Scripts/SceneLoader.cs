using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;

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

        // Suche nach der Timer-Instanz und setze die initialTime aus dem neuen Level
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.SetInitialTime(timer.initialTime); // Setze die initialTime für das neue Level
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // is it the player
        if (collision.CompareTag(tagToCheck))
        {
            LoadMyScene(sceneName);
        }
    }
}