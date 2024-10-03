using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;


namespace Platformer.Mechanics
{
    /// <summary>
    /// This class contains the data required for implementing token collection mechanics.
    /// It does not perform animation of the token, this is handled in a batch by the 
    /// TokenController in the scene.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
        public AudioClip tokenCollectAudio;
        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public bool randomAnimationStartTime = false;
        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        //unique index which is assigned by the TokenController in a scene.
        internal int tokenIndex = -1;
        internal TokenController controller;
        //active frame in animation, updated by the controller.
        internal int frame = 0;
        internal bool collected = false;

        public Score score;

       void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            // Versuche die Score-Referenz automatisch zu finden
            score = FindObjectOfType<Score>(); // Sucht nach einem Score-Objekt in der Szene
            if (score == null)
            {
                Debug.LogError("Score reference not found in the scene!");
            }
            else
            {
                Debug.Log("Score reference found and assigned.");
            }

            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);

            sprites = idleAnimation;
        }



        void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);

        }

        void OnPlayerEnter(PlayerController player)
        {
            if (collected) return;

            // Token als eingesammelt markieren und Animation ändern
            frame = 0;
            sprites = collectedAnimation;
            collected = true;

            // Prüfe, ob der Score korrekt gesetzt wurde
            if (score != null)
            {
                score.AddScore(1); // Score erhöhen
                Debug.Log("Score increased to: " + score.globalScore); // Debug-Log hinzufügen
            }
            else
            {
                Debug.LogError("Score reference is missing in TokenInstance!");
            }

            // Deaktiviere den Renderer, aber lasse den Collider aktiv
            _renderer.enabled = false;

            // Event für das Einsammeln des Tokens auslösen
            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;
        }

        public void ResetToken()
        {
            collected = false;
            sprites = idleAnimation;
            frame = 0;

            // Mache den Renderer wieder sichtbar und aktiviere den Collider
            _renderer.enabled = true;
            GetComponent<Collider2D>().enabled = true;

            Debug.Log("Token reset successfully.");
        }

    }
}