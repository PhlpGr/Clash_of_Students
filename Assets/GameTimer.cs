using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public float timerDuration = 300.0f; // Setze die Timer-Dauer in Sekunden (z.B. 5 Minuten)
    private float currentTime;
    public TMP_Text timerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentTime = timerDuration;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            // Handle the timer running out (e.g., reset the game or show a game over screen)
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            if (timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (timerText == null)
            {
                timerText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();
            }
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
