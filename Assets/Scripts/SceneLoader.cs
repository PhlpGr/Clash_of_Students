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

        // Load the scene
        SceneManager.LoadScene(sceneName);

        // Find the Timer instance and set the initialTime for the new level
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.SetInitialTime(timer.initialTime); // Set initialTime for the new level
        }
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
