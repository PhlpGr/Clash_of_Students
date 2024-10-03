using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// This event is fired when user input should be enabled.
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player != null){
            player.controlEnabled = true;
            Debug.Log("Spielersteuerung aktiviert.");
            }
            else{
                Debug.Log("Spielermodell wurde nicht korrekt initialisiert.");
            }
        }
    }
}