using UnityEngine;
using Cinemachine;  // Importiere Cinemachine für die Kameraführung
using Platformer.Mechanics;
public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;  // Array der Charakter-Prefabs
    private GameObject activeCharacter;    // Das aktuell aktive Charakter-GameObject

    public CinemachineVirtualCamera virtualCamera;  // Referenz zur Cinemachine Virtual Camera

    void Start()
    {
        Debug.Log("CharacterManager Start");

        // Hole den ausgewählten Charakterindex, stelle sicher, dass PlayerSelection korrekt implementiert ist
        int selectedCharacterIndex = PlayerSelection.SelectedCharacterIndex;
        Debug.Log("Selected Character Index: " + selectedCharacterIndex);

        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characterPrefabs.Length)
        {
            Vector3 startPosition = new Vector3(-5.77f, 0.95f, 0f);  // Startposition des Charakters
            activeCharacter = Instantiate(characterPrefabs[selectedCharacterIndex], startPosition, Quaternion.identity);
            activeCharacter.name = "Player";
            Debug.Log("Character instantiated: " + activeCharacter.name);
            // Setze den Layer des Characters
            activeCharacter.layer = LayerMask.NameToLayer("Player");
            // Setze den Player im GameController und update die Kamera, wenn diese Komponenten vorhanden sind
            Debug.Log("Layer assigned to Player: " + LayerMask.LayerToName(activeCharacter.layer));
            SetUpPlayerAndCamera(activeCharacter);
        }
        else
        {
            Debug.LogError("Invalid character index selected");
        }
    }

    private void SetUpPlayerAndCamera(GameObject player)
    {
        // Prüfe auf und verarbeite das PlayerController-Komponent
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log("PlayerController found and setting up player in GameController and camera.");

            // Setze den Spieler in GameController, wenn verwendet
            if (GameController.Instance != null)
                GameController.Instance.SetPlayer(playerController);

            // Verbinde die Cinemachine-Kamera mit dem Spieler
            if (virtualCamera != null)
                virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogError("No PlayerController found on the character.");
        }
    }
}