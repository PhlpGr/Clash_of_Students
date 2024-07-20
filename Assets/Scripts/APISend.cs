using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
public class API_CALL1 : MonoBehaviour


{
    static async Task Main(string[] args)
    {
        var url = "http://localhost:1999/api/data";
        var data = new
        {
            key1 = "value1",
            key2 = "value2"
        };

        var response = await PostDataAsync(url, data);
        Console.WriteLine(response);
    }

    public static async Task<string> PostDataAsync(string url, object data)
    {
        using (var httpClient = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }


}