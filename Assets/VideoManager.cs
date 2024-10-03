using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Video; 

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referenz auf den VideoPlayer
    [Tooltip("Der Name des Levels, das nach dem Video geladen wird")]
    public string nextLevelName; // Das Level, das nach dem Video geladen werden soll

    void Start()
    {
        // Debug Nachricht: Video startet
        Debug.Log("Video started!");

        // Sicherstellen, dass das Video nicht in einer Schleife abgespielt wird
        videoPlayer.isLooping = false; // Zusätzliche Sicherheit

        // Eventlistener hinzufügen: Was passiert, wenn das Video zu Ende ist
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Diese Methode wird aufgerufen, wenn das Video zu Ende ist
    void OnVideoEnd(VideoPlayer vp)
    {
        // Debug Nachricht: Video zu Ende
        Debug.Log("Video finished, loading next level...");

        // Lade die nächste Szene, deren Name im Inspector angegeben ist
        SceneManager.LoadScene(nextLevelName);
    }

    /*
    void Update()
    {
        // Optional: Ermögliche das Überspringen des Videos per Tastendruck (z.B. Leertaste)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Lade das angegebene Level, wenn die Leertaste gedrückt wird
            SceneManager.LoadScene(nextLevelName);
        }
    }
    */
}