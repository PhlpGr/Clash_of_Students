using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerVirusCollision : Simulation.Event<PlayerVirusCollision>
    {
        public VirusController virus;
        public PlayerController player;

        public override void Execute()
        {
            // Überprüft, ob der Spieler von oben auf das Virus springt
            var willHurtVirus = player.Bounds.center.y >= virus.Bounds.max.y;

            if (willHurtVirus)
            {
                // Spieler springt nach dem Aufprall zurück
                player.Bounce(2);
            }
            else
            {
                // Spieler stirbt, wenn er frontal mit dem Virus kollidiert
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}
