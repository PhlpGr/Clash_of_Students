using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;  // Füge dies hinzu, um die Debug-Klasse zu verwenden

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        //public Vector3 spawnPosition;

        public override void Execute()
        {
            // Überprüfe, ob der Spieler im Modell vorhanden ist
            var player = model.player;

            if (player == null)
            {
                Debug.LogError("Player reference is missing in the model.");
                return;
            }

            if (player.collider2d == null)
            {
                Debug.LogError("Player collider2d is not assigned.");
                return;
            }

            if (player.health == null)
            {
                Debug.LogError("Player health component is missing.");
                return;
            }

             if (model.spawnPoint == null)
            {
                Debug.LogError("Spawn point is missing in the model.");
                return;
            }

            // Setze die Position auf die Spawn-Position
            //player.transform.position = spawnPosition;

            player.collider2d.enabled = true;
            player.controlEnabled = true; // Steuerung aktivieren

            // Falls vorhanden, spiele den Respawn-Sound ab
            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);

            player.health.Increment();
            // Teleportiere den Spieler zum Spawnpunkt (optional, wenn benötigt)
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.Grounded;
            player.animator.SetBool("dead", false);

            if (model.virtualCamera != null)
            {
                model.virtualCamera.m_Follow = player.transform;
                model.virtualCamera.m_LookAt = player.transform;
            }
            else
            {
                Debug.LogError("Virtual camera is not assigned.");
            }
/*
            if (model.spawnPoint == null)
            {
                Debug.LogError("Spawn point is missing in the model.");
                return;
            }
*/
            // Optional: Eingabeplaner, um die Eingabe nach einer Verzögerung zu aktivieren
            // Simulation.Schedule<EnablePlayerInput>(2f); 
        }
    }
}
