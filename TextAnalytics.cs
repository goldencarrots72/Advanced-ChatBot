using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace QnAMakerBot
{
    public class TextAnalytics
    {
        /// <summary>
        /// Container for subscription credentials. Make sure to enter your valid key.
        /// </summary>
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", "92a851d0eb37473bb178e902232ead48");
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        async public Task<string> Start(string input)
        {
            string response = string.Empty;
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://eastus2.api.cognitive.microsoft.com"
            };

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Extracting language
            //Console.WriteLine("===== LANGUAGE EXTRACTION ======");

            var result = client.DetectLanguageAsync(new BatchInput(
                    new List<Input>()
                        {
                          new Input("1", input)
                    })).Result;

            // Printing language results.
            foreach (var document in result.Documents)
            {
                response += "Language: " + document.DetectedLanguages[0].Name + "\n";
            }

            // Getting key-phrases
            //Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "1", input)
                        })).Result;

            // Printing keyphrases
            foreach (var document in result2.Documents)
            {
                response += ("Key phrases: ");

                foreach (string keyphrase in document.KeyPhrases)
                {
                    response += (keyphrase);
                }
            }

            // Extracting sentiment
            //Console.WriteLine("\n\n===== SENTIMENT ANALYSIS ======");

            SentimentBatchResult result3 = client.SentimentAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "1", input)
                        })).Result;


            // Printing sentiment results
            foreach (var document in result3.Documents)
            {
                response += ("\nSentiment Score: " + document.Score);
            }
            return response;
        }
    }
}