using CamTheGeek.GpioDotNet;
using System;

namespace pushy_button
{
    class Program
    {
         static void Main(string[] args)
        {
            using (var button = new GpioPin(20, Direction.In))
            {
                button.High += Button_High;
                button.Low += Button_Low;
                Console.WriteLine("Waiting for button. Press ENTER to exit.");
                Console.ReadLine();
            }
        }

        private static void Button_Low(object sender, EventArgs e)
        {
            Console.WriteLine("Button released!");
        }

        private static void Button_High(object sender, EventArgs e)
        {
            Console.WriteLine("Button pressed!");
        }
    }
}
