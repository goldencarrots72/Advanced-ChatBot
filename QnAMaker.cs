using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QnAMakerBot
{
    public class QnAMaker
    {
        
        private class QnAMakerObject
        {
            public Answer[] answers { get; set; }
        }
        private class Answer
        {
            public string[] questions { get; set; }
            public string answer { get; set; }
            public float score { get; set; }
            public int id { get; set; }
            public string source { get; set; }
            public object[] metadata { get; set; }
        }

        private float threshhold = 70;

        //endpoint key provided by Knowledge Base deployment details
        private static string endpoint_key = "key";

        async public Task<string> TryQuery(string query) {
            string question = $"{{\"question\": \"{query}\"}}";
            var response = await Post(question);
            JObject json = JObject.Parse(response);
            QnAMakerObject QnAMaker = JsonConvert.DeserializeObject<QnAMakerObject>(response);
            float score = QnAMaker.answers[0].score;
            string ans = QnAMaker.answers[0].answer;
            return (score > threshhold ? ans : "Fall Back Response ") + ("\nConfidence Score: " + score);
        }

        async static Task<string> Post(string question)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                string uri = "https://upsqamaker.azurewebsites.net/qnamaker/knowledgebases/{kb key}/generateAnswer";
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(question, Encoding.UTF8, "application/json");
                request.Headers.Add("Authorization", "EndpointKey " + endpoint_key);

                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
