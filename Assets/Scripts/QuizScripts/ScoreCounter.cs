using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace QuizScripts
{
    public class ScoreCounter
    {
        public int lection_score = 0;

        public void CountScore(int incorrectAnswersCount, int correctAnswersCount)
        {
            lection_score += correctAnswersCount;
            lection_score -= incorrectAnswersCount;
        }

        public async Task PostScoreAsync(string user, string program, string course, string lection, int final_score, string course_creator)
        {
            string url = "http://localhost:1999/score-submission";

            var payload = new
            {
                user = user,
                program = program,
                course = course,
                lection = lection,
                final_score = final_score,
                course_creator = course_creator
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending request...");

            // Asynchronous request using UnityWebRequest
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield(); // Let Unity continue to render frames
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response received: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Request error: " + request.error);
            }
        }
    }
}
