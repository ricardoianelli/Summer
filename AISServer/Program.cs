using AISServer.Components.Io;
using AISServer.Components.Io.Enums;
using AISServer.Components.Io.Interfaces;

namespace AISServer;

internal class Program
{
    static Task Main(string[] args)
    {
        try
        {
            Test();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return Task.CompletedTask;
    }

    private static void Test()
    {
        IIoController ioController = new IoController();
        ioController.Initialize();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Enter command (nodes, get, on, off, exit):");
            string command = Console.ReadLine()?.ToLower();

            switch (command)
            {
                case "nodes":
                    var nodes = ioController.GetNodes();
                    Console.WriteLine($"Nodes: {string.Join(", ", nodes)}");
                    break;
                case "get":
                    var imagingLightStatus = ioController.GetBit(ProfibusOutputId.IMAGE_LIGHT_ON);
                    Console.WriteLine($"Imaging light status: {imagingLightStatus}");
                    break;
                case "on":
                    ioController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, true);
                    break;
                case "off":
                    ioController.SetBit(ProfibusOutputId.IMAGE_LIGHT_ON, false);
                    break;
                case "exit":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid command. Use 'on', 'off', or 'exit'.");
                    continue;
            }
        }

        ioController?.Shutdown();
    }
}