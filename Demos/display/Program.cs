using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Device.I2c;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace display
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // Config
            // Read the configuration file
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string svcBusConn = configuration["ConnectionString"];

            // Setup
            Console.WriteLine("Opening Service Bus Connection...");
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(svcBusConn);
            QueueClient client = new QueueClient(builder, ReceiveMode.ReceiveAndDelete);

            RegisterHandlers(client);

            // Wait
            Console.WriteLine("Ready. Awaiting message from Cortana.");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            // Clean up
            await client.CloseAsync();
        }

        static void RegisterHandlers(QueueClient client)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1
            };

            client.RegisterMessageHandler(MessageReceivedHandler, messageHandlerOptions);
        }

        static async Task MessageReceivedHandler(Message message, CancellationToken token)
        {
            string textToDisplay = Encoding.Default.GetString(message.Body);
            Console.WriteLine($"Displaying message at {DateTime.Now}");
            await DisplayText(textToDisplay);
        }

        static async Task DisplayText(string textToDisplay)
        {
            using (var i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x3F)))
            using (var lcd = new LcdDisplay(i2c))
            {
                await lcd.DisplayText(textToDisplay);
            }
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            if (exceptionReceivedEventArgs.Exception.GetType() != typeof(OperationCanceledException))
            {
                Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            }
            return Task.CompletedTask;
        }
    }
}
