using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a Player collides with a ForceField (Blitz).
    /// </summary>
    public class PlayerForceFieldCollision : Simulation.Event<PlayerForceFieldCollision>
    {
        public ForceFieldController forceField;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // Spieler stirbt sofort bei Berührung des Blitzes
            Schedule<PlayerDeath>();

            // Optional: Spiele den Sound ab, wenn vorhanden
            if (forceField.ouchEffect != null && forceField._audio != null)
            {
                forceField._audio.PlayOneShot(forceField.ouchEffect);
            }

            // Hier könntest du zusätzliche Effekte hinzufügen, z.B. Partikeleffekte oder eine Animation.
        }
    }
}
