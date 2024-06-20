using UnityEngine;
using UnityEngine.SceneManagement;

public class Characterauswahl : MonoBehaviour
{

    public void OnCharacterSelected(int index)
    {
        PlayerSelection.SelectedCharacterIndex = index;
        SceneManager.LoadScene("Hauptmenü");
    }
}
      
    
    
    /*
    
    
    public static int selectedCharacterIndex = 0;

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Hauptmenü");
    }
}*/