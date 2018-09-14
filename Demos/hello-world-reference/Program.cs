using CamTheGeek.GpioDotNet;
using System;
using System.Threading;


namespace hello_world_reference
{
    /// <summary>
    /// This is a simple program to turn an LED on/off.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Turning on LED and blinking 5 times...");
            using (var led = new GpioPin(17, Direction.Out))
            {
                for (var i = 0; i < 5; ++i)
                {
                    led.Value = PinValue.High;
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    led.Value = PinValue.Low;
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            Console.WriteLine("Done!");
        }
    }
}
