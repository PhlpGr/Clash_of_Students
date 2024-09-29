using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class VirusController : MonoBehaviour
    {
        public PatrolPath path; // Der Pfad, dem das Virus folgt
        public float speed = 1.0f; // Geschwindigkeit des Virus entlang des Pfads
        public float rotationSpeed = 100f; // Geschwindigkeit der Rotation
        public float fallSpeed = 5.0f; // Geschwindigkeit, mit der das Virus nach unten fällt

        private bool isFalling = false; // Flag, um zu prüfen, ob das Virus fällt
        internal PatrolPath.Mover mover;
        internal Collider2D _collider;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Simulation.Schedule<PlayerVirusCollision>();
                ev.player = player;
                ev.virus = this;

                // Wenn der Spieler auf das Virus springt, fällt das Virus herunter
                if (player.Bounds.center.y >= Bounds.max.y)
                {
                    isFalling = true; // Virus soll fallen
                }
            }
        }

        void Update()
        {
            if (isFalling)
            {
                // Virus fällt nach unten
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                if (transform.position.y < -10) // Grenze, unter der das Virus verschwindet
                {
                    Destroy(gameObject); // Virus aus dem Spiel entfernen
                }
            }
            else
            {
                // Bewegung auf dem Pfad, wenn das Virus nicht fällt
                if (path != null)
                {
                    if (mover == null) mover = path.CreateMover(speed);
                    transform.position = mover.Position;
                }

                // Rotation des Virus-GameObjects
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
