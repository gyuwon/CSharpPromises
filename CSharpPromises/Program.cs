using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CSharpPromises
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        async static Task Run()
        {
            try
            {
                dynamic story = await GetJson("story.json");
                Console.WriteLine(story.heading);

                var chapterTasks = ((JArray)story.chapterUrls).Select(e => GetJson(e));

                foreach (var task in chapterTasks)
                {
                    dynamic chapter = await task;
                    Console.WriteLine(chapter.html);
                }

                Console.WriteLine("All done");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Argh, borken: {0}", exception.Message);
            }
        }

        async static Task<object> GetJson(dynamic resource)
        {
            const string baseUrl = "http://www.html5rocks.com/en/tutorials/es6/promises/";
            RestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest((string)resource);
            IRestResponse response = await client.ExecuteGetTaskAsync(request);
            return JObject.Parse(response.Content);
        }
    }
}
