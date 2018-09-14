using CamTheGeek.GpioDotNet;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cortana_lights
{
    /// <summary>
    /// This program connects to a service bus queue and awaits a message from an Azure Logic app.
    /// The user speaks a command to Cortana locally, which is connected to IFTTT.com, which posts the
    /// the command to an Azure Logic App via a web hook, which sends the message on the queue.
    /// See the slides for an animation.
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
            QueueClient client = new QueueClient(builder, ReceiveMode.ReceiveAndDelete);
            RegisterHandlers(client);

            // Wait
            Console.WriteLine("Ready. Awaiting message from Cortana.");
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
            ProcessColorText(System.Text.Encoding.Default.GetString(message.Body));
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

        static void ProcessColorText(string color)
        {
            ClearAllLeds();

            switch (color.ToLower())
            {
                case "red":
                    _red.Value = PinValue.Low;
                    break;

                case "yellow":
                    _yellow.Value = PinValue.Low;
                    break;

                case "green":
                    _green.Value = PinValue.Low;
                    break;

                default:
                    BlinkAllLeds();
                    break;
            }
        }

        private static void BlinkAllLeds()
        {
            for(int i = 0; i<10; i++)
            {
                _red.Value = PinValue.Low;
                Thread.Sleep(100);
                _red.Value = PinValue.High;
                _yellow.Value = PinValue.Low;
                Thread.Sleep(100);
                _yellow.Value = PinValue.High;
                _green.Value = PinValue.Low;
                Thread.Sleep(100);
                _green.Value = PinValue.High;
            }
        }
    }
}
