using UnityEngine;

public class ForceFieldTrigger : MonoBehaviour
{
    public Animator blitzAnimator; // Verweise auf den Animator des Blitz-GameObjects

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Überprüfe, ob der Spieler den Trigger betritt
        {
            blitzAnimator.Play("ForceFieldAnimation", 0, 0.33f); // Stoppt die Animation bei force-field4
            blitzAnimator.speed = 0; // Animation anhalten

            // Hier den Code hinzufügen, um den Spieler durch das Tor gehen zu lassen
            // Beispiel: player.GetComponent<PlayerController>().GoThroughGate();
        }
    }
}
