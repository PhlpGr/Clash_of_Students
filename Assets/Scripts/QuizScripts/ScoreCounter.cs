using System;
using System.Net.Http;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
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

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Antwort erfolgreich erhalten:");
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine($"Fehler bei der Anfrage: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }
    }
}
