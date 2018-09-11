using System;
using System.Threading;
using System.Threading.Tasks;
using System.Devices.Gpio;

namespace hello_world_reference
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Turning on LED and blinking 5 times...");
            using (var controller = new GpioController(new RaspberryPiDriver()))
            {
                GpioPin led = controller.OpenPin(17, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {

                    led.Write(PinValue.Low);
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    led.Write(PinValue.High);
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                }
                
                led.Dispose();
            }
            Console.WriteLine("Done!");
        }
    }
}
