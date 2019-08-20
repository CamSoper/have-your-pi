using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading;


namespace hello_world
{
    /// <summary>
    /// This is a simple program to turn an LED on/off.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int pin = 17;

            Console.WriteLine("Turning on LED and blinking 5 times...");
            using (GpioController gpio = new GpioController(PinNumberingScheme.Logical, new RaspberryPi3Driver()))
            {
                gpio.OpenPin(pin, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    gpio.Write(pin, 1);
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    gpio.Write(pin, 0);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                gpio.ClosePin(pin);
            }
            Console.WriteLine("Done!");
        }
    }
}
