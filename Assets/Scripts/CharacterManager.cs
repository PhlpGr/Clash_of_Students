using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;

    void Start()
    {
        Instantiate(characterPrefabs[Characterauswahl.selectedCharacterIndex]);
    }
}