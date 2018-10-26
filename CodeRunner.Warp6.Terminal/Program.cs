using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeRunner.Warp6.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var uri = "https://ohfzo3bq31.execute-api.us-east-2.amazonaws.com/default";
            var person = new { Id = "10", Name = "John", Surname = "Doe" };
            using (HttpClient client = new HttpClient())
            {
                var putResponse = await client.PutAsync(uri, new StringContent(JsonConvert.SerializeObject(person)));
                Console.WriteLine(await putResponse.Content.ReadAsStringAsync());
                var postResponse = await client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(new { Id = person.Id })));
                Console.WriteLine(await postResponse.Content.ReadAsStringAsync());
            }
            Console.ReadKey();
        }
    }
}
