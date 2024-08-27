using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject buttonObject;     // Der Button als GameObject
    public GameObject pillar;           // Das Säulen-GameObject
    public float pillarMoveDistance = 2.0f;  // Abstand, den die Säule nach oben bewegt werden soll

    private bool isPressed = false;
    private Collider2D buttonCollider;

    void Start()
    {
        // Sicherstellen, dass der Button sichtbar ist und der Collider korrekt zugewiesen ist
        buttonObject.SetActive(true);
        buttonCollider = buttonObject.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            // Überprüfen, ob der Spieler auf den Button springt, egal aus welcher Richtung
            Vector2 playerBottom = other.bounds.min; // Unterster Punkt des Spielers
            Vector2 buttonTop = buttonCollider.bounds.max; // Oberster Punkt des Buttons

            if (playerBottom.y > buttonTop.y - 0.1f) // Spieler muss sich auf dem Button befinden
            {
                ActivateButton();
            }
        }
    }

    void ActivateButton()
    {
        // Den Button verschwinden lassen
        buttonObject.SetActive(false);

        // Die Säule nach oben bewegen
        pillar.transform.position = new Vector3(pillar.transform.position.x, pillar.transform.position.y + pillarMoveDistance, pillar.transform.position.z);

        isPressed = true;
    }
}
