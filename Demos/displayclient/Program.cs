using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace displayclient
{
    class Program
    {
        private const string url = "CONNECTION STRING HERE";
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            
            Console.WriteLine("Type a message, then press enter. Ctrl+C to exit.");
            while(true)
            {
                string textToDisplay = Console.ReadLine();
                var request = new StringContent($"{{\"message\":\"{textToDisplay}\"}}", Encoding.Default, "application/json");
                client.PostAsync(url, request);
            }
        }
    }
}
