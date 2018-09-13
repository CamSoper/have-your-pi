using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CamTheGeek.GpioDotNet;

namespace door_watcher
{
    class Program
    {
        private static readonly Uri _logicAppUri = new Uri("LOGIC APP URL HERE");
        private static HttpClient _client = new HttpClient();

        static void Main(string[] args)
        {
            using (var contactSwitch = new GpioPin(20, Direction.In))
            {
                contactSwitch.High += ContactSwitch_High;
                contactSwitch.Low += ContactSwitch_Low;
                Console.WriteLine("Waiting for switch. Press ENTER to exit.");
                Console.ReadLine();
            }
        }

        private static void ContactSwitch_Low(object sender, EventArgs e)
        {
            PostStatus("Contact switch is OPEN!");
        }

        private static void ContactSwitch_High(object sender, EventArgs e)
        {
            PostStatus("Contact switch is CLOSED!");
        }

        static void PostStatus(string status)
        {
            Task.Run(() =>
            {
                string statusMessageJson = JsonConvert.SerializeObject(new StatusMessage(status));
                Console.WriteLine(statusMessageJson);
                _client.PostAsync(_logicAppUri, new StringContent(statusMessageJson, Encoding.UTF8, "application/json"));
            });
        }

    }
}
