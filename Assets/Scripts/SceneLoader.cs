using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string tagToCheck;
    public string sceneName;

    public void LoadMyScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // is it the player
        if(collision.CompareTag(tagToCheck))
        {
            LoadMyScene(sceneName);
        }
    }


}
