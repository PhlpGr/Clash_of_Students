using UnityEngine;

namespace Platformer.Mechanics
{
    public class KastenController_RL : MonoBehaviour
    {
        public PatrolPath path;
        private PatrolPath.Mover mover;

        void Start()
        {
            if (path != null)
                mover = path.CreateMover(1.5f); // Geschwindigkeit kann angepasst werden
        }

        void Update()
        {
            if (mover != null)
            {
                // Aktualisiere die Position des Kastens entlang des Pfades auf der X-Achse
                transform.position = new Vector3(mover.Position.x, transform.position.y, transform.position.z);
            }
        }
    }
}
