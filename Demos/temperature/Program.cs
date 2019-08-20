using System;
using System.Device.Spi;
using System.Threading.Tasks;

namespace temperature
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var spiSettings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 1000000,
                Mode = SpiMode.Mode0
            };

            using (SpiDevice spi = SpiDevice.Create(spiSettings))
            using (RtdProbe rtd = new RtdProbe(spi))
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Console.Clear();
                    Console.WriteLine($"{rtd.ProbeTemp}°F");
                }
            }
        }
    }
}
