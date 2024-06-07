using UnityEngine;
using UnityEngine.SceneManagement;

public class Characterauswahl : MonoBehaviour
{
    public static int selectedCharacterIndex = 0;

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Hauptmenü");
    }
}