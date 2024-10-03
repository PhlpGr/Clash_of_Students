using UnityEngine;
using TMPro;
using System.Collections;
using Platformer.Mechanics;
using Platformer.Gameplay;
using Platformer.Core;
using Platformer.Model;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Reference to the UI text element for displaying the timer
    [SerializeField] public float initialTime = 120f; // Start time set for each level in the Inspector
    [SerializeField] string startSceneName; // Name of the start scene to load
    [SerializeField] float respawnDelay = 2f; // Delay before respawn
    public float remainingTime; // Public for other scripts to access
    public bool isTiming = false; // Timer does not run by default
    private static Timer instance;
    private bool levelEnded = false; // Flag to check if the player has completed the level
    private bool isLevelEndCountdown = false; // Flag to indicate if it is a level end countdown
    private Version2_GameManager gameManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene change
        }
        else
        {
            Destroy(gameObject); // Destroy any new duplicate Timer objects
        }
    }

    void Start()
    {
        remainingTime = initialTime; // Use the initialTime set in the Inspector
        PositionTimerText(); // Position the timer on the screen
        UpdateTimerDisplay(); // Update the timer display immediately

        // Find and store reference to Version2_GameManager
        gameManager = Version2_GameManager.Instance;
    }

    void Update()
    {
        if (isTiming && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Decrease the remaining time
        }
        else if (remainingTime <= 0 && isTiming)
        {
            remainingTime = 0; // Ensure the timer doesn't go below 0
            StartCoroutine(HandleTimeUp()); // Handle what happens when the timer runs out
        }

        UpdateTimerDisplay(); // Update the timer display
    }

    // This method sets the initial time for a new level
    public void SetInitialTime(float time)
    {
        initialTime = time; // Update initialTime for the new level
        remainingTime = initialTime; // Reset remaining time
        isTiming = true; // Start the timer
        UpdateTimerDisplay();
        Debug.Log("New level started. Timer set to: " + initialTime);
    }

    // Called when the player reaches the LevelEnd
    public void LevelEndReached()
    {
        levelEnded = true;
        isLevelEndCountdown = true; // Set the flag to true to indicate a level end countdown
        ResetTimer(); // Reset the timer and stop it
        ResetScore(); // Reset the score
    }

    private IEnumerator HandleTimeUp()
    {
        if (levelEnded)
        {
            yield break; // Exit if the level has ended
        }

        isTiming = false; // Stop the timer

        // Call ResetQuestionCount() on the GameManager
        if (gameManager != null)
        {
            gameManager.ResetQuestionCount();
        }

        // Disable player control
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.controlEnabled = false; // Disable player control
        }

        // Display "Zeit abgelaufen!"
        timerText.text = "Zeit abgelaufen!";
        timerText.color = Color.white;
        timerText.enableWordWrapping = false; // Disable word wrapping

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Load the specified start scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneName);

        // Wait for the scene to load
        yield return new WaitForSeconds(0.1f);

        // Find the spawn point in the start scene using a tag (e.g., "Respawn")
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (spawnPoint != null && player != null)
        {
            // Set the spawn point in the model
            PlatformerModel model = Simulation.GetModel<PlatformerModel>();
            model.spawnPoint = spawnPoint.transform;

            // Use PlayerSpawn to move the player
            Simulation.Schedule<PlayerSpawn>();
        }

        // Reset the timer
        ResetTimer();

        // Start the timer again
        StartTimer();

        // Reset the score
        ResetScore();
    }
    public void ResetTimer()
    {
        remainingTime = initialTime; // Set the timer back to the initial time
        UpdateTimerDisplay(); // Update the timer display to show the reset time
        isTiming = false; // Do not start the timer automatically
        Debug.Log("Timer has been reset.");
    }

    private void StartTimer()
    {
        isTiming = true; // Start the timer
        Debug.Log("Timer has started.");
    }

    private void ResetScore()
    {
        // Find the Score component and call its ResetScore() method
        Score score = FindObjectOfType<Score>();
        if (score != null)
        {
            score.ResetScore(); // Reset the score
            Debug.Log("Score has been reset.");
        }
        else
        {
            Debug.LogWarning("Score component not found!");
        }
    }

    public void UpdateTimerDisplay()
    {
        if (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Display the remaining time in MM:SS format
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Change to red if less than 10 seconds and it's not during level end countdown
            if (remainingTime < 10f && !isLevelEndCountdown)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
        else
        {
            timerText.text = "00:00";
        }
    }

    private void PositionTimerText()
    {
        RectTransform rectTransform = timerText.GetComponent<RectTransform>();

        // Adjust the anchor points to position the timer at the center of the canvas
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Set the position relative to the anchor point
        rectTransform.anchoredPosition = new Vector2(100, 175);

        rectTransform.sizeDelta = new Vector2(200, 50); // Adjust size if necessary
    }


    public void DestroyTimer()
    {
    // Entferne das Timer-Objekt aus der Szene
    Destroy(gameObject);
    Debug.Log("Timer wurde zerstört.");
    }


}