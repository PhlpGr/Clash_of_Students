using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class API_CALL2 : MonoBehaviour
{
    private async void Start()
    {
        string url = "http://localhost:1999/api/data";
        var data = new
        { // Hier soll eingetragen werden, was dann richtung DB geschickt wird., Darf erst nach Beantwortung der Frage geschickt werden.
            key1 = "value1",
            key2 = "value2"
        };

        string response = await PostDataAsync(url, data);
        Debug.Log(response);
    }

    public static async Task<string> PostDataAsync(string url, object data)
    {
        string json = JsonConvert.SerializeObject(data);
        byte[] postData = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
                return null;
            }
        }
    }
}
