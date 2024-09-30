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
        // Startet das Video automatisch und wartet auf das Ende
        videoPlayer.loopPointReached += OnVideoEnd;  // Eventlistener für das Ende des Videos
    }

    // Diese Methode wird aufgerufen, wenn das Video zu Ende ist
    void OnVideoEnd(VideoPlayer vp)
    {
        // Lade die nächste Szene, deren Name im Inspector angegeben ist
        SceneManager.LoadScene(nextLevelName);
    }

    void Update()
    {
        // Optional: Ermögliche das Überspringen des Videos per Tastendruck (z.B. Leertaste)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Lade das angegebene Level, wenn die Leertaste gedrückt wird
            SceneManager.LoadScene(nextLevelName);
        }
    }
}