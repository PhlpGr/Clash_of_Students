using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the game model in the inspector and ticks the simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        // This model field is public and can be therefore modified in the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private void Awake()
        {
            // Singleton pattern to ensure only one instance of GameController exists.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);  // Optional: Prevents the object from being destroyed when changing scenes.
                if (model == null)
                {
                    model = new PlatformerModel();
                }
            }
        }

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }

        public void FindAndSetSpawnPoint()
        {
            model.spawnPoint = GameObject.FindWithTag("SpawnPoint");
            if (model.spawnPoint == null)
            {
                Debug.LogWarning("Spawn point not found in the current scene!");
            }
            else
            {
                Debug.Log("Spawn point set to: " + model.spawnPoint.name);
            }
        }


        // Find and set the virtual camera in the current scene
        public void FindAndSetVirtualCamera()
        {
            model.virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            if (model.virtualCamera == null)
            {
                Debug.LogError("Virtual camera not found in the current scene!");
            }
        }

        // Method to start a new level
        public void StartNewLevel()
        {
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.SetInitialTime(timer.initialTime); // Set the initial time for the new level
                Debug.Log("Neues Level gestartet. Timer wurde zurückgesetzt und gestartet.");
            }
            else
            {
                Debug.LogError("Timer konnte nicht gefunden werden!");
            }
        }
    }
}
