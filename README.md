# Have Your Pi and Eat It Too: .NET Core on Raspberry Pi

Thanks so much for attending my talk! I hope you found it informative and enjoyable. I've gathered together all the information I could think of to help you get started with .NET Core on Raspberry Pi.

## Get the hardware

The demos for my talk were performed on a [Raspberry Pi 3 B](https://www.raspberrypi.org/products/raspberry-pi-3-model-b/). [Raspberry Pi 3 B+](https://www.raspberrypi.org/products/raspberry-pi-3-model-b-plus/) is an incremental revision that adds PoE (power over ethernet) support, which can be really useful for home automation projects.

The Pi itself will require, at a minimum, power and a micro SD card. The official power supply is 2100 mA, so aim for that or higher. (Any 5V micro USB power supply technically works, but I have seen them complain about low power with a 5V 1500 mA power supply.) There are many different introductory kits that include the power supply, micro SD card, and various accessories by different manufacturers. I can't speak for all of them, but I do have personal experience with CanaKit and have been impressed with their packages.

Cases are optional. If you have access to a 3D printer, you can always make your own. My favorite designs are [this one](https://www.thingiverse.com/thing:1549574) and [this one](https://www.thingiverse.com/thing:3061437) (I customized the second one).

[Heat sinks are optional](https://raspberrypi.stackexchange.com/questions/43752/do-i-need-to-use-a-heat-sink). The Pi does a pretty good job with its own thermal throttling, and it's debatable whether a tiny stick-on heat sink without any thermal paste actually reduces the temperature by any significant amount.   

## Get the bits

You'll need an [operating system](https://www.raspberrypi.org/downloads/). I use Raspbian Lite. [Windows IOT Core will also run .NET Core](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md).

