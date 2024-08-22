using System;
using UnityEngine;

public class QuizAPIService : MonoBehaviour
{
    private string baseURL = "http://localhost:1999/api/questions";

    public string GenerateURL(string email, string program, string course, string lection, int position)
    {
        // Erzeugt die dynamische URL basierend auf den übergebenen Parametern
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}", baseURL, email, program, course, lection, position);
    }
}
