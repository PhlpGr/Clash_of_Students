using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Platformer.Mechanics;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public Score score; // Reference to the Score script
    private Version2_GameManager gameManager; // Reference to the GameManager
    public Button exitButton; // Reference to the Exit Button

    private bool shouldResetScore = false; // Flag to indicate when the score should be reset

    void Start()
    {
        // Find the GameManager instance at the start
        gameManager = Version2_GameManager.Instance;

        // Register for the sceneLoaded event only if not already registered
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove any existing listener to avoid duplicates
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initial setup for the button
        SetupButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Clear the current button reference
        exitButton = null;

        // Update button reference in the new scene
        UpdateButtonReference();
        SetupButton(); // Re-setup the button for the new scene

        // Update the score reference in the new scene
        score = FindObjectOfType<Score>();
        if (score != null)
        {
            Debug.Log("Score reference updated in OnSceneLoaded.");
        }
        else
        {
            Debug.LogWarning("Score object not found in the new scene!");
        }

        // Reset the score only if it was set to reset by the exit button
        if (shouldResetScore && score != null)
        {
            score.ResetScore();
            Debug.Log("Score has been reset in the new scene.");
            shouldResetScore = false; // Reset the flag after resetting the score
        }
    }


    // Update the button reference in the new scene
    private void UpdateButtonReference()
    {
        // Find the Exit Button in the new scene by its name
        GameObject buttonObject = GameObject.Find("ExitButton"); // Replace "ExitButton" with the name of your button in the scene
        if (buttonObject != null)
        {
            exitButton = buttonObject.GetComponent<Button>();
            Debug.Log("Exit Button successfully updated.");
        }
        else
        {
            Debug.LogWarning("Exit Button not found in the new scene!");
        }
    }

    // Set up the button listener
    private void SetupButton()
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners(); // Remove previous listeners to avoid duplicates
            exitButton.onClick.AddListener(() => ExitLevel());
        }
        else
        {
            Debug.LogWarning("Exit Button reference is not set in the SceneLoader!");
        }
    }

    // Method to handle exit button click
    public void ExitLevel()
    {
        // Set the flag to reset the score
        shouldResetScore = true;

        // Call the method to destroy the timer and load the scene
        DestroyTimerAndLoadScene(sceneName);
    }

    // Method to destroy the timer and load the scene
    public void DestroyTimerAndLoadScene(string sceneName)
    {
        // Find the Timer instance and destroy it before loading the new scene
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            Debug.Log("Destroying Timer...");
            timer.DestroyTimer(); // Destroy the Timer instance
        }
        else
        {
            Debug.LogWarning("No Timer instance found!");
        }

        // Load the scene
        LoadMyScene(sceneName);
    }

    public void LoadMyScene(string sceneName)
    {
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
    }

    private void OnDestroy()
    {
        // Unregister from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
