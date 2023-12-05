using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    public string menuScene = "MainMenu";

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                Application.Quit();
            }
            else
                SceneManager.LoadScene(menuScene);

        }
    }

    public void CloseMyGame()
    {
        Application.Quit();
    }
}
