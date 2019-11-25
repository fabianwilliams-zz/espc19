using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FabsAFTrigBindSess.httpToVisionFaceAPI
{
    public class FaceRectangle
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }
    public class Emotion
    {
        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }
    }
    public class FaceAttributes
    {
        public Emotion Emotion { get; set; }
    }
    public class Face
    {
        public FaceRectangle FaceRectangle { get; set; }
        public FaceAttributes FaceAttributes { get; set; }
    }

    public static class Function1
    {
        public static string ProcessFaces(Face[] faces)
        {
            int angerCount = 0;
            int contemptCount = 0;
            int disgustCount = 0;
            int fearCount = 0;
            int happinessCount = 0;
            int neutralCount = 0;
            int sadnessCount = 0;
            int surpriseCount = 0;

            foreach (var face in faces)
            {
                var emotion = face.FaceAttributes.Emotion;

                if (emotion.Anger > 0.1)
                {
                    angerCount += 1;
                }
                if (emotion.Contempt > 0.1)
                {
                    contemptCount += 1;
                }
                if (emotion.Disgust > 0.1)
                {
                    disgustCount += 1;
                }
                if (emotion.Fear > 0.1)
                {
                    fearCount += 1;
                }
                if (emotion.Happiness > 0.1)
                {
                    happinessCount += 1;
                }
                if (emotion.Neutral > 0.1)
                {
                    neutralCount += 1;
                }
                if (emotion.Sadness > 0.1)
                {
                    sadnessCount += 1;
                }
                if (emotion.Surprise > 0.1)
                {
                    surpriseCount += 1;
                }
            }

            return $"For {faces.Length} detected faces:\n\n" +
                $"- {angerCount} showed anger\n" +
                $"- {contemptCount} showed contempt\n" +
                $"- {disgustCount} showed disgust\n" +
                $"- {fearCount} showed fear\n" +
                $"- {happinessCount} showed happiness\n" +
                $"- {neutralCount} seemed neutral\n" +
                $"- {sadnessCount} seemed sad\n" +
                $"- {surpriseCount} seemed surprised.";
        }

        public static async Task<Face[]> GetFacialAnalysisResults(string imageUrl)
        {
            string faceUrlBase = Environment.GetEnvironmentVariable("CognitiveServicesUrlBase");
            string key1 = Environment.GetEnvironmentVariable("CognitiveServicesKey1");

            // FaceLandmarks gives us a rectangle containing the face.
            // FaceAttributes=emotion will tell Cognitive Services to read the emotion of a face.
            string reqParams = "?returnFaceLandmarks=true&returnFaceAttributes=emotion";

            var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); ;
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key1);

            var json = "{\"url\":\"" + $"{imageUrl}" + "\"}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, faceUrlBase + reqParams);
            request.Content = new StringContent(json,
                                                Encoding.UTF8,
                                                "application/json");

            var resp = await client.SendAsync(request);
            var jsonResponse = await resp.Content.ReadAsStringAsync();


            var faces = JsonConvert.DeserializeObject<Face[]>(jsonResponse);

            return faces;
        }

        public static string GetImageUrl(HttpRequest req)
        {
            string imageUrl = req.Query["url"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            return imageUrl = imageUrl ?? data?.name;
        }
    }


    public static class FaceDontLie
    {
        public static string ProcessFaces(Face[] faces)
        {
            int angerCount = 0;
            int contemptCount = 0;
            int disgustCount = 0;
            int fearCount = 0;
            int happinessCount = 0;
            int neutralCount = 0;
            int sadnessCount = 0;
            int surpriseCount = 0;

            foreach (var face in faces)
            {
                var emotion = face.FaceAttributes.Emotion;

                if (emotion.Anger > 0.1)
                {
                    angerCount += 1;
                }
                if (emotion.Contempt > 0.1)
                {
                    contemptCount += 1;
                }
                if (emotion.Disgust > 0.1)
                {
                    disgustCount += 1;
                }
                if (emotion.Fear > 0.1)
                {
                    fearCount += 1;
                }
                if (emotion.Happiness > 0.1)
                {
                    happinessCount += 1;
                }
                if (emotion.Neutral > 0.1)
                {
                    neutralCount += 1;
                }
                if (emotion.Sadness > 0.1)
                {
                    sadnessCount += 1;
                }
                if (emotion.Surprise > 0.1)
                {
                    surpriseCount += 1;
                }
            }

            return $"For {faces.Length} detected faces:\n\n" +
                $"- {angerCount} showed anger\n" +
                $"- {contemptCount} showed contempt\n" +
                $"- {disgustCount} showed disgust\n" +
                $"- {fearCount} showed fear\n" +
                $"- {happinessCount} showed happiness\n" +
                $"- {neutralCount} seemed neutral\n" +
                $"- {sadnessCount} seemed sad\n" +
                $"- {surpriseCount} seemed surprised.";
        }

        public static async Task<Face[]> GetFacialAnalysisResults(string imageUrl)
        {
            string faceUrlBase = Environment.GetEnvironmentVariable("CognitiveServicesUrlBase");
            string key1 = Environment.GetEnvironmentVariable("CognitiveServicesKey1");

            // FaceLandmarks gives us a rectangle containing the face.
            // FaceAttributes=emotion will tell Cognitive Services to read the emotion of a face.
            string reqParams = "?returnFaceLandmarks=true&returnFaceAttributes=emotion";

            var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); ;
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key1);

            var json = "{\"url\":\"" + $"{imageUrl}" + "\"}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, faceUrlBase + reqParams);
            request.Content = new StringContent(json,
                                                Encoding.UTF8,
                                                "application/json");

            var resp = await client.SendAsync(request);
            var jsonResponse = await resp.Content.ReadAsStringAsync();


            var faces = JsonConvert.DeserializeObject<Face[]>(jsonResponse);

            return faces;
        }

        public static string GetImageUrl(HttpRequest req)
        {
            string imageUrl = req.Query["url"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            return imageUrl = imageUrl ?? data?.name;
        }


        [FunctionName("FaceDontLie")]
        public static async Task<IActionResult> FacesDontLie(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var imageUrl = GetImageUrl(req);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return new BadRequestObjectResult("Fabian Faces API call is about to be made...");
            }
            else
            {
                var faces = await GetFacialAnalysisResults(imageUrl);
                var message = ProcessFaces(faces);

                dynamic dump = JsonConvert.SerializeObject(faces);

                log.LogInformation($"The Payload looks like: {dump}");

                return new OkObjectResult($"{message}");
            }
        }
    }
}
