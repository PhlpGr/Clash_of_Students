using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;

public class LevelEnd : MonoBehaviour
{
    private Timer timer; // Reference to the Timer script
    private Score score; // Reference to the Score script
    private LevelEndManager levelEndManager; // Reference to the LevelEndManager script
    public string nextSceneName; // Name of the next scene to load
    public float timeReductionSpeed = 5f; // Speed for counting down the timer quickly
    public float scoreIncrementDelay = 0.05f; // Delay between each score increment

    void Start()
    {
        // Find the Timer, Score, and LevelEndManager components in the scene
        timer = FindObjectOfType<Timer>();
        score = FindObjectOfType<Score>();
        levelEndManager = FindObjectOfType<LevelEndManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has reached the LevelEnd
        if (other.CompareTag("Player"))
        {
            // Notify the Timer that the level has ended
            if (timer != null)
            {
                StartCoroutine(HandleLevelEnd());
            }

            // Disable player control (if applicable)
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DisableControl();
            }
        }
    }

    private IEnumerator HandleLevelEnd()
    {
        // Stop the timer to prevent it from continuing elsewhere
        timer.isTiming = false;

        // Store the remaining time before starting the countdown
        int remainingSeconds = Mathf.FloorToInt(timer.remainingTime);

        // Gradually increase the global score by the remaining seconds
        if (score != null)
        {
            yield return StartCoroutine(GraduallyIncreaseScore(remainingSeconds)); // Wait for this to finish first
            Debug.Log("Global score increased by remaining seconds: " + remainingSeconds);
        }

        // Notify the LevelEndManager to post the score now
        if (levelEndManager != null)
        {
            Debug.Log("Posting the score now...");
            levelEndManager.PostScoreAndReset(); // This should happen after the score increment
        }

        // Count down the timer quickly to zero
        while (timer.remainingTime > 0)
        {
            float timeToReduce = Mathf.Min(timeReductionSpeed * Time.deltaTime, timer.remainingTime);
            timer.remainingTime -= timeToReduce;
            timer.UpdateTimerDisplay(); // Update the display in real-time
            yield return null;
        }

        // Ensure the timer displays 0
        timer.remainingTime = 0;
        timer.UpdateTimerDisplay();

        // Wait for a brief moment to ensure the player sees the countdown reaching 0
        yield return new WaitForSeconds(1f);

        // Reset the timer after handling the level end
        if (timer != null)
        {
            timer.ResetTimer();
        }

        // **Reset the score before loading the next scene**
        if (score != null)
        {
            score.ResetScore();
            Debug.Log("Score has been reset.");
        }

        // Load the next scene if specified
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // Coroutine to gradually increase the score
    private IEnumerator GraduallyIncreaseScore(int remainingSeconds)
    {
        for (int i = 0; i < remainingSeconds; i++)
        {
            score.AddScore(1); // Increment score by 1
            yield return new WaitForSeconds(scoreIncrementDelay); // Delay to show the increment visually
        }
    }
}
