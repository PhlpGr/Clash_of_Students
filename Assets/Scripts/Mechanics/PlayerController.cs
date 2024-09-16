using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {

            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }
        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
        void OnTriggerEnter2D(Collider2D other)
        {
        if (other.CompareTag("Flame"))
        {
        Debug.Log("Spieler hat Flamme berührt - Auslösen des Flame Collision Events.");
        var flameEvent = Schedule<PlayerFlameCollision>();
        flameEvent.player = this;
        }
        else if (other.CompareTag("Enemy"))
        {
        Debug.Log("Kollision mit Enemy erkannt.");
        var enemyCollision = Schedule<PlayerEnemyCollision>();
        enemyCollision.player = this;
        enemyCollision.enemy = other.GetComponent<EnemyController>();
        }
         else if (other.CompareTag("Blitz"))
        {
        Debug.Log("Kollision mit Blitz erkannt.");
        var forceFieldCollision = Schedule<PlayerForceFieldCollision>();
        forceFieldCollision.player = this;
        forceFieldCollision.forceField = other.GetComponent<ForceFieldController>();
        }
        else if (other.CompareTag("Virus"))  // Make sure the virus tag is assigned
        {
         Debug.Log("Kollision mit Virus erkannt.");
        var virusCollision = Schedule<PlayerVirusCollision>();  // Custom event for virus collision
        virusCollision.player = this;
        virusCollision.virus = other.GetComponent<VirusController>();  // VirusController reference
        }
        else if (other.CompareTag("Troll"))  // Make sure the troll tag is assigned
        {
        Debug.Log("Kollision mit Troll erkannt.");
        var trollCollision = Schedule<PlayerTrollCollision>();  // Custom event for troll collision
        trollCollision.player = this;
        trollCollision.troll = other.GetComponent<TrollController>();  // TrollController reference
        }


        }

        public void DisableControl()
        {
            controlEnabled = false;
            velocity = Vector2.zero;  // Stoppe jegliche Bewegung
        }

            public void EnableControl()
        {
            controlEnabled = true;
        }
    }
}