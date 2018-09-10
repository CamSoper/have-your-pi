using System;
using System.Threading;
using System.Threading.Tasks;
using System.Devices.Gpio;
using System.Diagnostics;

namespace pushy_button
{
    class Program
    {
         static void Main(string[] args)
        {
            Console.WriteLine("PRESS ENTER");
            Console.ReadLine();

            using (var controller = new GpioController(new RaspberryPiDriver()))
            {
                GpioPin button = controller.OpenPin(20, PinMode.InputPullDown);

                button.NotifyEvents = PinEvent.SyncFallingRisingEdge;
                button.ValueChanged += ButtonPressed;
                button.EnableRaisingEvents = true;

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 100);
                }
            }
            Console.WriteLine("Done!");
        }

        static void ButtonPressed(object sender, PinValueChangedEventArgs e)
        {
            GpioPin button = sender as GpioPin;
            var pinValue = button.Read();
            if(pinValue == PinValue.Low)
            {
                Console.WriteLine("button released");
            }
            else
            {
                Console.WriteLine("button pressed");
            }


        }
    }
}
