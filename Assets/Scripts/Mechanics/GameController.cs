using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public PlayerController player; //neu plus awake und setplayer auch           
        private void Awake()
        {
            // Singleton-Muster um sicherzustellen, dass nur eine Instanz von GameController existiert.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);  // Zerstöre das zusätzliche GameObject, das diesen Script enthält.
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);  // Optional: Verhindere, dass das Objekt bei Szenenwechsel zerstört wird.
                if (model == null) {
                model = new PlatformerModel();
                }
            }
        }

        // Methode zum Setzen des Players, aufrufbar von anderen Skripten.
        public void SetPlayer(PlayerController newPlayer)
        {
            player = newPlayer;
            model.player = newPlayer;
            // Hier können weitere Initialisierungen durchgeführt werden, z.B. das Einstellen der Kamera:
            // if (virtualCamera != null) virtualCamera.Follow = player.transform;

            Debug.Log("Player gesetzt: " + newPlayer.gameObject.name);
            
            // Weitere Konfigurationen können hier eingefügt werden, wie z.B. Event-Listener.
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
        
        // Neue Methode zum Starten eines neuen Levels
        public void StartNewLevel()
        {
            Timer timer = FindObjectOfType<Timer>(); // Suche nach der Timer-Instanz in der Szene
            if (timer != null)
            {
                timer.SetInitialTime(timer.initialTime); // Setze die initialTime des neuen Levels
                Debug.Log("Neues Level gestartet. Timer wurde zurückgesetzt und gestartet.");
            }
            else
            {
                Debug.LogError("Timer konnte nicht gefunden werden!");
            }
        }
    }
}