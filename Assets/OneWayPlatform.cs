using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider2D platformCollider;

    private void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        // Stelle sicher, dass dieser Collider als Trigger fungiert.
        platformCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Rigidbody2D rb = collider.attachedRigidbody;
            if (rb.velocity.y <= 0) // Spieler fällt oder springt nicht nach oben
            {
                IgnorePlatformCollision(collider, false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Rigidbody2D rb = collider.attachedRigidbody;
            // Wenn der Spieler fällt oder nicht nach oben springt und sich unterhalb der Oberkante der Plattform befindet
            if (rb.velocity.y <= 0 && collider.transform.position.y < (transform.position.y + (platformCollider.size.y / 2)))
            {
                IgnorePlatformCollision(collider, true);
            }
            else
            {
                IgnorePlatformCollision(collider, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            IgnorePlatformCollision(collider, false);
        }
    }

    void IgnorePlatformCollision(Collider2D collider, bool ignore)
    {
        Physics2D.IgnoreCollision(platformCollider, collider, ignore);
    }
}
