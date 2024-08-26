using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class ForceFieldController : MonoBehaviour
    {
        public AudioClip ouchEffect;  // Sound, der abgespielt wird, wenn der Spieler den Blitz berührt

        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer initialisieren
        }

        void Update()
        {
            // Überprüfen, ob der Blitz verschwunden ist (d.h. das Sprite ist null) oder wieder sichtbar wird
            if (spriteRenderer.sprite == null)
            {
                _collider.enabled = false; // Deaktiviert den Collider, damit der Spieler nicht hängen bleibt
                Debug.Log("Collider deaktiviert, weil Sprite None ist.");
            }
            else
            {
                _collider.enabled = true;  // Aktiviert den Collider, wenn das Sprite nicht None ist
                Debug.Log("Collider aktiviert, weil Sprite nicht None ist.");
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Kollision erkannt mit: " + collision.gameObject.name);
            var player = collision.gameObject.GetComponent<PlayerController>();

            // Überprüfe, ob der Player vorhanden ist und das Sprite nicht None ist
            if (player != null && spriteRenderer.sprite != null)
            {
                Debug.Log("Kollision mit Spieler erkannt und Sprite ist nicht None!");
                var ev = Schedule<PlayerForceFieldCollision>();
                ev.player = player;
                ev.forceField = this;

                // Optional: Spiele den Sound ab, wenn vorhanden
                if (ouchEffect != null && _audio != null)
                {
                    _audio.PlayOneShot(ouchEffect);
                }
            }
            else if (player != null)
            {
                Debug.Log("Sprite ist None, keine Aktion ausgeführt.");
            }
            else
            {
                Debug.Log("Kollidiert mit Nicht-Spieler-Objekt.");
            }
        }
    }
}
