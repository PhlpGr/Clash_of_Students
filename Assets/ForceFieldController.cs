using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    public Transform player; // Referenz auf den Spieler
    public float stopDistance = 1.0f; // Abstand zum Tor, bei dem die Animation gestoppt wird

    private Animator animator;
    private bool isPlayerNear = false;

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator-Komponente des Force-Field-Objekts holen
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= stopDistance && !isPlayerNear)
        {
            isPlayerNear = true;

            // Stoppe die Animation und setze das letzte Frame (force-field4)
            animator.speed = 0; // Animation anhalten
            animator.Play("ForceFieldAnimation", 0, 0.66f); // 0.66f sollte das force-field4 Frame sein
        }
    }
}
