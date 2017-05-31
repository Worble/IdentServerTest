using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any button to begin");
            Console.ReadLine();
            TestAsync();
            Console.ReadLine();
        }

        private static async System.Threading.Tasks.Task TestAsync()
        {
            Console.WriteLine("Testing Client Token");
            var disco = await DiscoveryClient.GetAsync("http://localhost:61871");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            Console.WriteLine("Talking to Authentification");
            if (tokenResponse.IsError)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine("SUCCESS");
            Console.WriteLine(tokenResponse.Json);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            Console.WriteLine("Talking to Api with username and password...");
            var response = await client.GetAsync("http://localhost:61872/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("NOT SUCCESS");
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine("CONTENT");
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.WriteLine("Testing client with username and password");
            var tokenClient2 = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse2 = await tokenClient2.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            if (tokenResponse2.IsError)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(tokenResponse2.Error);
                return;
            }
            Console.WriteLine("SUCCESS");
            Console.WriteLine(tokenResponse2.Json);
            Console.WriteLine("\n\n");

            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse2.AccessToken);

            Console.WriteLine("Talking to Api with allowed username and password...");
            var response2 = await client2.GetAsync("http://localhost:61872/identity");
            if (!response2.IsSuccessStatusCode)
            {
                Console.WriteLine("NOT SUCCESS");
                Console.WriteLine(response2.StatusCode);
            }
            else
            {
                Console.WriteLine("CONTENT");
                var content = await response2.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}