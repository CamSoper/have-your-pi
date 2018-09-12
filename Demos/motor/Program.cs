using System;

namespace motor
{
    class Program
    {
        const string _validArgs = "on,off,fwd,rev";
        static void Main(string[] args)
        {
            if (args.Length != 1 || !_validArgs.Contains(args[0]))
            {
                Console.WriteLine("Usage: motor on|off|fwd|rev");
                return;
            }
            using (var motor = new Motor(21, 27))
            {
                switch (args[0].ToLower())
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
                }
            }
        }
    }
}
