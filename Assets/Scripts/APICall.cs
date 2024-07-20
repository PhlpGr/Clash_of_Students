using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UI;
public class API_CALL : MonoBehaviour
{
    public class Fact // Hier muss die aufschlüsselung des HTTP Calls hin: https://json2csharp.com/
    {
        public int id { get; set; }
        public string user { get; set; }
        public string question_type { get; set; }
        public string frage { get; set; }
        public string answer_a { get; set; }
        public string answer_b { get; set; }
        public string answer_c { get; set; }
        public string answer_d { get; set; }
        public string correct_answer { get; set; }
        public string program { get; set; }
        public string course { get; set; }
        public string lection { get; set; }
        public int position { get; set; }
        public object image_url { get; set; }
    }

    public TextMeshProUGUI frage; // Erstellt Fragefeld in Unity -> Refactor Rename für Umbenennung
    public TextMeshProUGUI answer_a; // Erstellt Antwortfeld
    public TextMeshProUGUI answer_b; // Erstellt Antwortfeld
    public TextMeshProUGUI answer_c; // Erstellt Antwortfeld
    public TextMeshProUGUI answer_d; // Erstellt Antwortfeld


    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:1999/api/questions/tom@one7.one/Digital%20Business%20Engineering/it/1/1")); // Hier wird die API aufgerifen -> Da muss die Datenbankanbindung hin.
     // URL muss abhängig von Levelposition geändert werden. Service erforderlich. oder Übergabe bei Levelstart
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
                    Fact facts = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);
                    //catfact.text = fact.facts; // Bef+llt Fragefeld
                    //print(catfact.text); 
                    frage.text = facts.frage;
                    answer_a.text = facts.answer_a; // Befüllt Antwortfeld
                    answer_b.text = facts.answer_b;
                    answer_c.text = facts.answer_c;
                    answer_d.text = facts.answer_d;
                    // Hier können die Textstücke aus der Request zugewiesen werden.
                    break;

            }
        }


    }
}