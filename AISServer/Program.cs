using System.Reflection;
using AISServer.Components.ConveyorBelts;
using AISServer.Components.Hardware.Profibus;
using AISServer.Components.Hardware.Profibus.Enums;
using AISServer.Components.Hardware.Profibus.Interfaces;
using AISServer.Components.Io;
using AISServer.Components.Io.Enums;
using AISServer.Components.Io.Interfaces;
using AISServer.Components.Logging;
using AISServer.Components.Logging.Interfaces;
using Summer.DependencyInjection;

namespace AISServer;

internal class Program
{
    static Task Main(string[] args)
    {
        // ComponentsEngine.Start(Assembly.GetExecutingAssembly());
        //
        // Console.WriteLine("===============================================");
        // Console.WriteLine("Hello, AIS!");
        // Console.WriteLine("- Type exit or bye at any time to exit the program!");
        // Console.WriteLine("===============================================\n");
        //
        // var lastMessage = "None";
        //
        // while (lastMessage != "exit" && lastMessage != "bye")
        // {
        //     Console.WriteLine("Please enter a command: ");
        //     lastMessage = Console.ReadLine();
        //     ParseCommand(lastMessage);
        // }

        Test();
        
        return Task.CompletedTask;
    }

    private static void Test()
    {
        // Sample logger for testing (replace with a real logger implementation)
        ILogger logger = new ConsoleLogger();

        // Profibus IO mappings
        var ioMappings = new ProfibusIoMappings();
        
        IIoController currentIoController = null;


        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Enter command (io1, io2, get, on, off, exit):");
            string command = Console.ReadLine()?.ToLower();

            switch (command)
            {
                case "io1":
                    Console.WriteLine("Initializing IoController");
                    currentIoController = new IoController(ioMappings, logger);
                    currentIoController.Initialize();
                    Console.WriteLine("IoController initialized!");
                    break;
                case "io2":
                    Console.WriteLine("Initializing IoController2");
                    currentIoController = new IoController2(ioMappings, logger);
                    currentIoController.Initialize();
                    Console.WriteLine("IoController2 initialized!");
                    break;
                case "get":
                    if (currentIoController is null)
                    {
                        Console.WriteLine("Current IO controller is not set. Use 'io1' or 'io2' to set the current IO controller.");
                        break;
                    }
                    
                    // Get the status of the imaging light
                    bool imagingLightStatus = currentIoController.GetBit(ProfibusOutputId.IMAGE_LIGHT_ON);
                    Console.WriteLine($"Imaging light status: {imagingLightStatus}");
                    break;
                case "on":
                    if (currentIoController is null)
                    {
                        Console.WriteLine("Current IO controller is not set. Use 'io1' or 'io2' to set the current IO controller.");
                        break;
                    }
                    // Turn on the imaging light
                    currentIoController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, true);
                    break;
                case "off":
                    if (currentIoController is null)
                    {
                        Console.WriteLine("Current IO controller is not set. Use 'io1' or 'io2' to set the current IO controller.");
                        break;
                    }
                    // Turn off the imaging light
                    currentIoController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, false);
                    break;
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid command. Use 'on', 'off', or 'exit'.");
                    continue;
            }

            
        }

        // Shutdown Profibus card
        currentIoController?.Shutdown(1);
    }

    private static void ParseCommand(string? lastMessage)
    {
        if (string.IsNullOrEmpty(lastMessage)) return;
        
        switch (lastMessage)
        {
            case "f":
            case "forward":
                ComponentsEngine.GetComponent<InfeedConveyor>()?.Forward();
                break;
            case "b":
            case "backward":
                ComponentsEngine.GetComponent<InfeedConveyor>()?.Backward();
                break;
            case "s":
            case "stop":
                ComponentsEngine.GetComponent<InfeedConveyor>()?.Stop();
                break;
            case "exit":
            case "bye":
                Console.WriteLine("Goodbye!");
                break;
            default:
                Console.WriteLine("Unknown command!");
                break;
        }
    }
}