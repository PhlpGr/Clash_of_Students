using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class PlayerFlameCollision : Simulation.Event<PlayerFlameCollision>
    {
        public PlayerController player;

        public override void Execute()
        {
            Schedule<PlayerDeath>();  // Respawnt oder führt eine Todesanimation aus, je nach deiner Spiellogik
        }
    }
}
