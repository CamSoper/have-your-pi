# Have Your Pi and Eat It Too: .NET Core on Raspberry Pi

Thanks so much for attending my talk! I hope you found it informative and enjoyable. I've gathered together all the information I could think of to help you get started with .NET Core on Raspberry Pi.

## Get the hardware

The demos for my talk were performed on a [Raspberry Pi 3 B](https://www.raspberrypi.org/products/raspberry-pi-3-model-b/). [Raspberry Pi 3 B+](https://www.raspberrypi.org/products/raspberry-pi-3-model-b-plus/) is an incremental revision that adds PoE (power over ethernet) support, which can be really useful for home automation projects.

The Pi itself will require, at a minimum, power and a micro SD card. The official power supply is 2100 mA, so aim for that or higher. (Any 5V micro USB power supply technically works, but I have seen them complain about low power with a 5V 1500 mA power supply.) There are many different introductory kits that include the power supply, micro SD card, and various accessories by different manufacturers. I can't speak for all of them, but I do have personal experience with CanaKit and have been impressed with their packages.

Cases are optional. If you have access to a 3D printer, you can always make your own. My favorite designs are [this one](https://www.thingiverse.com/thing:1549574) and [this one](https://www.thingiverse.com/thing:3061437) (I customized the second one).

[Heat sinks are optional](https://raspberrypi.stackexchange.com/questions/43752/do-i-need-to-use-a-heat-sink). The Pi does a pretty good job with its own thermal throttling, and it's debatable whether a tiny stick-on heat sink without any thermal paste actually reduces the temperature by any significant amount. 

## Get some accessories

There are a ton of accessories available on Amazon, eBay, AliExpress, etc. The accessories I used for my talk are:

* [Freenove Ultimate Starter Kit for Raspberry Pi](http://a.co/d/0Pl9Tdp) - This is a complete kit and has everything you need to get started (and then some!) except for the two items listed below.
* [2 Channel 5V Relays](http://a.co/d/j5lcbjm) - There are a wide variety of brands on this, but they all seem to be about the same. 
* [Wired Door Sensor Magnetic Switch](http://a.co/d/i3lq5l2)

For purposes of having my circuits pre-assembled for the talk, I also used [extra breadboards](http://a.co/d/1UH92rN) and [GPIO breakouts](http://a.co/d/d7bWMI5).

## Get the bits

You'll need an [operating system](https://www.raspberrypi.org/downloads/). I use Raspbian Lite. [Windows IOT Core will also run .NET Core](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md) if you'd prefer a Windows OS.

## Build the demos

### Circuits
You can refer to the slides to see how the circuits were assembled.

### Dependencies
The demos depend on System.Devices.Gpio, which comes from a NuGet package hosted on the corefxlabs MyGet feed. To add the feed to your package sources on Windows, open `%appdata%\NuGet\NuGet.Config` and add the following element to **packageSources**:

```xml
<add key="dotnet corefxlab MyGet" value="https://dotnet.myget.org/F/dotnet-corefxlab/api/v3/index.json" />
```

On Linux and Mac developer machines, you may need to use the *source* switch with `dotnet restore` to point to the above feed. 

### Visual Studio
Open `.\Demos\have-your-pi.sln` in Visual Studio and build.

You will need to run `dotnet publish -r linux-arm` at the command line to build the Self-Contained Deployment (SCD). This can be automated as a post-build task in Visual Studio if you wish.

### Command Line
From `.\Demos`, run:

```console
dotnet restore
dotnet build
dotnet publish -r linux-arm
```

## Run the demos

Using FileZilla, SCP, or your favorite file transfer tool, move the contents of `.\Demos\<demo>\bin\Debug\netcoreapp2.1\linux-arm\publish` to a location on your Pi. Execute `chmod 755` on the executable to give it run permissions. You can then run the executable by name, including the path. For example, assuming you are in your home (~) location and you've deployed the *pushy-button* demo to `~/pushy-button/`, you'd type:

```bash
./pushy-button/pushy-button
```

> **IMPORTANT** - On subsequent builds in Visual Studio or at the command line, the `publish` folder will not be updated unless you do it manually. After making changes and compiling, the latest DLL and PDB will be located in `.\Demos\<demo>\bin\Debug\netcoreapp2.1`. Those are all you really need to deploy to test/debug changes.

## Debugging on the Pi

### Visual Studio

Visual Studio can connect directly to a SCD via SSH. To do so, click **Debug > Attach to Process...**. In the dialog, for **Connection type** choose SSH, and then for **Connection target** enter `<username>@<address>`.  For example, to connect to the device at 192.168.1.101 using the user *pi*, enter `pi@192.168.1.101`. After authenticating, the list of processes will be populated and you can attach to the process. 

### Visual Studio Code

Debugging from Visual Studio Code depends on vsdbg on the target machine and requires some configuration in VSCode. [See this walkthrough for details](https://github.com/OmniSharp/omnisharp-vscode/wiki/Remote-Debugging-On-Linux-Arm).