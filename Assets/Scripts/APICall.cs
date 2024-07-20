
using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;
public class API_CALL : MonoBehaviour
{
    public class Fact // Hier muss die aufschlüsselung des HTTP Calls hin: https://json2csharp.com/
    {
        public string fact { get; set; }
        public int length { get; set; }
    }

    public TextMeshProUGUI text;

    void Start()
    {
        StartCoroutine(GetRequest("https://catfact.ninja/fact")); // Hier wird die API aufgerifen -> Da muss die Datenbankanbindung hin.
    }

    IEnumerator GetRequest(string URL)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(string.Format("Fehler: {0}", webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);
                    text.text = fact.fact;
                    // Hier können die Textstücke aus der Request zugewiesen werden.
                    break;

            }
        }


    }
}


