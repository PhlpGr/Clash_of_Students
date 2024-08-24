using UnityEngine;

namespace Platformer.Mechanics
{
    public class KastenController : MonoBehaviour
    {
        public PatrolPath path;
        private PatrolPath.Mover mover;

        void Start()
        {
            if (path != null)
                mover = path.CreateMover(1.0f); // Geschwindigkeit kann angepasst werden
        }

        void Update()
        {
            if (mover != null)
            {
                // Aktualisiere die Position des Kastens entlang des Pfades
                transform.position = new Vector3(transform.position.x, mover.Position.y, transform.position.z);
            }
        }
    }
}
