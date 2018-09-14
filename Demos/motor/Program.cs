using System;

namespace motor
{
    class Program
    {
        const string _validArgs = "on,off,fwd,rev,exit";
        const string _help = "Valid commands: on|off|fwd|rev|exit";
        static void Main(string[] args)
        {
            
            using (var motor = new Motor(21, 27))
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
