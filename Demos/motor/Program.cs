using System;

namespace motor
{
    class Program
    {
        const string _validArgs = "on,off,fwd,rev,exit";
        const string _help = "Valid commands: on|off|fwd|rev|exit";
        static void Main(string[] args)
        {
            int powerPin = 20;
            int[] polarityPins = new int[] { 19, 26 };
            
            using (var motor = new Motor(powerPin, polarityPins))
            {
                Console.WriteLine(_help);

                while (true)
                {
                    var command = Console.ReadLine();
                    switch (command)
                    {
                        case "on":
                            Console.WriteLine("Turning motor ON.");
                            motor.On();
                            break;

                        case "off":
                            Console.WriteLine("Turning motor OFF.");
                            motor.Off();
                            break;

                        case "rev":
                            Console.WriteLine("Reversing motor polarity.");
                            motor.Reverse();
                            break;

                        case "fwd":
                            Console.WriteLine("Resetting motor polarity.");
                            motor.Forward();
                            break;

                        case "exit":
                            Console.WriteLine("Bye!");
                            return;
                            
                        default:
                            Console.WriteLine(_help);
                            break;

                    }
                }
                
            }
        }
    }
}
