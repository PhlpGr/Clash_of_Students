using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;

public class SceneLoader : MonoBehaviour
{
    public string tagToCheck;
    public string sceneName;
    public Score score; // Reference to the Score script
    private Version2_GameManager gameManager; // Reference to the GameManager

    void Start()
    {
        // Find the GameManager instance at the start
        gameManager = Version2_GameManager.Instance;
    }

    public void LoadMyScene(string sceneName)
    {
        // Reset globalScore if score reference is set
        if (score != null)
        {
            score.ResetScore();
        }
        else
        {
            Debug.LogWarning("Score reference is not set in the SceneLoader!");
        }

        // Call ResetQuestionCount() on the GameManager
        if (gameManager != null)
        {
            gameManager.ResetQuestionCount();
        }
        else
        {
            Debug.LogWarning("GameManager instance is null!");
        }

        // Find the Timer instance and destroy it before loading the new scene
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.DestroyTimer(); // Destroy the Timer instance
        }

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Is it the player?
        if (collision.CompareTag(tagToCheck))
        {
            LoadMyScene(sceneName);
        }
    }
}
