using CamTheGeek.GpioDotNet;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace build_status_light
{
    /// <summary>
    /// This program connects to a service bus queue and awaits a build success/failure
    /// message from Azure DevOps. See the slides for an animation.
    /// </summary>
    class Program
    {
        // Please don't hard-code your connection strings. :)
        const string _svcBusConn = @"SERVICE BUS CONNECTION STRING HERE";

        static GpioPin _red;
        static GpioPin _yellow;
        static GpioPin _green;
        static void Main(string[] args)
        {
            // Setup
            Console.WriteLine("Opening pins...");
            _red = new GpioPin(16, Direction.Out, PinValue.High);
            _yellow = new GpioPin(20, Direction.Out, PinValue.High);
            _green = new GpioPin(21, Direction.Out, PinValue.High);

            Console.WriteLine("Opening Service Bus Connection...");
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(_svcBusConn);
            builder.EntityPath = "build";  //Name of the queue
            QueueClient client = new QueueClient(builder, ReceiveMode.ReceiveAndDelete);
            RegisterHandlers(client);

            // Turn on the yellow LED
            _yellow.Value = PinValue.Low;

            // Wait
            Console.WriteLine("Ready. Awaiting message from Azure DevOps. Light will be yellow until we get a status.");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            // Clean up
            client.CloseAsync().GetAwaiter().GetResult();
            _red.Dispose();
            _yellow.Dispose();
            _green.Dispose();
        }

        static void RegisterHandlers(QueueClient client)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1
            };

            client.RegisterMessageHandler(ProcessMessages, messageHandlerOptions);
        }

        static Task ProcessMessages(Message message, CancellationToken token)
        {
            dynamic data = JObject.Parse(System.Text.Encoding.Default.GetString(message.Body));
            string messageText = data.message.text.Value as string;
            ProcessStatusText(messageText);
            return Task.CompletedTask;
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        static void ClearAllLeds()
        {
            _red.Value = PinValue.High;
            _yellow.Value = PinValue.High;
            _green.Value = PinValue.High;
        }

        static void ProcessStatusText(string status)
        {
            ClearAllLeds();

            if (status.Contains(" succeeded"))
                _green.Value = PinValue.Low;
            else if (status.Contains(" failed"))
                _red.Value = PinValue.Low;
            else
                _yellow.Value = PinValue.Low;
        }
    }
}
