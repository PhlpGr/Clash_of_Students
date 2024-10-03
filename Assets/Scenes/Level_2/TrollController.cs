using System.Collections;
using UnityEngine;
using Platformer.Mechanics; // Importiere den Namespace für PlayerController
using Platformer.Core; // Importiere den Namespace für Simulation
using Platformer.Gameplay; // Importiere den Namespace für PlayerDeath

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TrollController : MonoBehaviour
{
    public float jumpForce = 4f; // Standardwert für die Sprungkraft
    public float jumpInterval = 0f; // Standardwert für das Zeitintervall zwischen Sprüngen
    public float rotationSpeed = 100f; // Geschwindigkeit der Kippbewegung
    private bool isGrounded = true; // Überprüfung, ob der Troll auf dem Boden ist
    private bool isFalling = false; // Zustand, ob der Troll kippt
    private bool hasFallen = false; // Zustand, ob der Troll bereits gekippt ist

    private Rigidbody2D rb;
    internal Collider2D _collider; // Collider-Referenz

    public Bounds Bounds => _collider.bounds; // Zugriff auf die Collider-Bounds

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>(); // Collider initialisieren
        StartCoroutine(JumpRoutine());
    }

    // Coroutine, die den Troll in regelmäßigen Abständen springen lässt
    IEnumerator JumpRoutine()
    {
        while (!isFalling && !hasFallen)
        {
            if (isGrounded)
            {
                Jump();
            }
            yield return new WaitForSeconds(jumpInterval);
        }
    }

    // Funktion, um den Troll springen zu lassen
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // Setzt den Status auf "nicht auf dem Boden", bis die Landung erkannt wird
    }

    // Überprüfung, ob der Troll auf dem Boden aufkommt
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Wenn der Troll den Boden berührt, kann er wieder springen
        }

        // Spieler-Kollision überprüfen
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && !isFalling && !hasFallen)
        {
            var ev = Simulation.Schedule<PlayerTrollCollision>();
            ev.player = player;
            ev.troll = this;

            // Wenn der Spieler auf den Troll springt, startet das Kippen
            if (player.Bounds.center.y >= Bounds.max.y)
            {
                StartFalling(); // Troll kippt nach rechts
            }
            else
            {
                // Spieler stirbt bei Kollision von der Seite oder unten
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }

    // Startet das Kippen des Trolls nach rechts
    void StartFalling()
    {
        isFalling = true;
        rb.velocity = Vector2.zero; // Stoppt jegliche Bewegung
        rb.isKinematic = true; // Schaltet die Physik für den Troll aus, damit wir ihn manuell rotieren können
        StartCoroutine(KippenUndLiegenbleiben());
    }

    // Coroutine, die das Kippen und Liegenbleiben des Trolls steuert
    IEnumerator KippenUndLiegenbleiben()
    {
        // Kippt nach rechts über einen Zeitraum von etwa einer Sekunde
        float rotateAmount = 0f;
        while (rotateAmount < 90f) // Kippe den Troll um 90 Grad
        {
            float step = rotationSpeed * Time.deltaTime; // Bestimme die Schrittgröße
            transform.Rotate(0, 0, -step); // Kippen nach rechts (negative Rotation auf der Z-Achse)
            rotateAmount += step; // Erhöhe den Rotationswert
            yield return null; // Warte bis zum nächsten Frame
        }

        hasFallen = true; // Setze den Status, dass der Troll gefallen ist
        rb.isKinematic = false; // Du kannst Physik wieder einschalten, wenn er liegen bleibt, falls nötig
    }
}
