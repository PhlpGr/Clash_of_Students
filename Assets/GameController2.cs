using UnityEngine;
using Platformer.Core;
using Platformer.Model;

// Stelle sicher, dass du das richtige Namespace verwendest oder die erforderlichen using-Anweisungen anfügst,
// falls du spezielle Komponenten wie Cinemachine oder andere benutzt.
namespace Platformer.Mechanics
{
public class GameController2 : MonoBehaviour
{
    public static GameController2 Instance { get; private set; }
    public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    // Hier halten wir eine Referenz auf den PlayerController.
    public PlayerController player;

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
        }
    }

    // Methode zum Setzen des Players, aufrufbar von anderen Skripten.
    public void SetPlayer(PlayerController newPlayer)
    {
        player = newPlayer;

        // Hier können weitere Initialisierungen durchgeführt werden, z.B. das Einstellen der Kamera:
        // if (virtualCamera != null) virtualCamera.Follow = player.transform;

        Debug.Log("Player gesetzt: " + newPlayer.gameObject.name);
        
        // Weitere Konfigurationen können hier eingefügt werden, wie z.B. Event-Listener.
    }

    private void Update()
    {
        if (player != null)
        {
            // Hier kannst du Update-Logik für den Spieler durchführen, wie z.B. Überprüfen des Spielerzustands.
        }
    }

    // Weitere Methoden des GameController können hier implementiert werden, wie z.B. Spielstart und Spielende.
}
}