using System.Reflection;
using AISServer.Components.ConveyorBelts;
using AISServer.Components.Hardware.Profibus;
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

        // Initialize IoController
        IIoController ioController = new IoController(1, ioMappings, logger);

        // Initialize Profibus card
        if (!ioController.Initialize(1))
        {
            Console.WriteLine("Initialization failed.");
            return;
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Enter command (on/off/exit):");
            string command = Console.ReadLine()?.ToLower();

            switch (command)
            {
                case "on":
                    // Turn on the imaging light
                    ioController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, true);
                    break;
                case "off":
                    // Turn off the imaging light
                    ioController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, false);
                    break;
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid command. Use 'on', 'off', or 'exit'.");
                    continue;
            }

            // Get the status of the imaging light
            bool imagingLightStatus = false;
            ioController.GetBit(ProfibusOutputId.IMAGE_LIGHT_ON, ref imagingLightStatus);
            Console.WriteLine($"Imaging light status: {imagingLightStatus}");
        }

        // Shutdown Profibus card
        ioController.Shutdown(1);
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