using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

using Platformer.Mechanics;


public class CharacterManager : MonoBehaviour
{
/*
    public GameObject[] characterPrefabs;
    private GameObject activeCharacter;

    void Start()
    {
        Debug.Log("CharakterManager Start");
        int selectedCharacterIndex = PlayerSelection.SelectedCharacterIndex;
        Debug.Log("Selected Character Index: " + selectedCharacterIndex);

        if (selectedCharacterIndex >= 0 && selectedCharacterIndex < characterPrefabs.Length)
        {
            // Setze den Charakter an die gewünschte Position
            Vector3 startPosition = new Vector3(-5.77f, 0.95f, 0f);
            activeCharacter = Instantiate(characterPrefabs[selectedCharacterIndex], startPosition, Quaternion.identity);
            activeCharacter.name = "Player";
            Debug.Log("Character instantiated: " + activeCharacter.name);

            if (activeCharacter != null)
            {
                Debug.Log("Character position: " + activeCharacter.transform.position);

                Renderer renderer = activeCharacter.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Debug.Log("Renderer is enabled: " + renderer.enabled);
                }
                else
                {
                    Debug.LogWarning("No Renderer found on the character.");
                }

                // Setze den Player im GameController
                PlayerController playerController = activeCharacter.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    GameController.Instance.SetPlayer(playerController);
                }
                else
                {
                    Debug.LogError("No PlayerController found on the character.");
                }
            }
        }
        else
        {
            Debug.LogError("Invalid character index selected");
        }
    }
}
*/
    public GameObject[] playerPrefabs; // Array der Spieler-Prefabs

    void Start()
    {
        // Beispiel: Auswahl des ersten verfügbaren Charakters
        CreateAndSetPlayer(0);
    }

    public void CreateAndSetPlayer(int selectedCharacterIndex)
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= playerPrefabs.Length)
        {
            Debug.LogError("Index außerhalb des Bereichs der Charakter-Array.");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefabs[selectedCharacterIndex]);
        PlayerController playerController = playerInstance.GetComponent<PlayerController>();

        if (playerController != null)
        {
         //   GameController.Instance.SetPlayer(playerController);
        }
        else
        {
            Debug.LogError("PlayerController-Komponente fehlt im instanziierten Spieler-Prefab.");
        }
    }
}

