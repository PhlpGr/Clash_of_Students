using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
        if (model == null || model.player == null) {
        Debug.LogError("Model or Player is not properly initialized.");
        return;
        }

        var player = model.player;
    
        // Sicherstellen, dass die Health-Komponente vorhanden ist.
        if (player.health == null) {
        Debug.LogError("Health component is missing on the player.");
        return;
        }

        if (player.health.IsAlive)
        {
        player.health.Die();

        // Sicherstellen, dass die Virtual Camera initialisiert ist.
        if (model.virtualCamera != null) {
            model.virtualCamera.m_Follow = null;
            model.virtualCamera.m_LookAt = null;
        } else {
            Debug.LogError("Virtual Camera is not set in the model.");
        }

        player.controlEnabled = false;

        if (player.audioSource != null && player.ouchAudio != null)
            player.audioSource.PlayOneShot(player.ouchAudio);

        if (player.animator != null) {
            player.animator.SetTrigger("hurt");
            player.animator.SetBool("dead", true);
        } else {
            Debug.LogError("Animator is missing on the player.");
        }

        Simulation.Schedule<PlayerSpawn>(2);
        }
    }
}}