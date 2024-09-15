using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerTrollCollision : Simulation.Event<PlayerTrollCollision>
    {
        public TrollController troll;
        public PlayerController player;

        public override void Execute()
        {
            // Überprüft, ob der Spieler von oben auf den Troll springt
            var willHurtTroll = player.Bounds.center.y >= troll.Bounds.max.y;

            if (willHurtTroll)
            {
                // Spieler springt nach dem Aufprall zurück
                player.Bounce(2);
            }
            else
            {
                // Spieler stirbt, wenn er frontal mit dem Troll kollidiert
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}
