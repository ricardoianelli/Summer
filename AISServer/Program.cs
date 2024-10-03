using AISServer.Components.Io;
using AISServer.Components.Io.Enums;
using AISServer.Components.Io.Interfaces;

namespace AISServer;

internal class Program
{
    public static IIoController IoController = new IoController();

    static Task Main(string[] args)
    {
        try
        {
            Test();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

        }
        finally
        {
            IoController?.Shutdown();
        }

        return Task.CompletedTask;
    }

    private static void Test()
    {
        IoController.Initialize();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Enter command (list, nodes, get, get-all, set, exit):");
            string command = Console.ReadLine()?.ToLower();

            switch (command)
            {
                case "list":
                    ListMappings();
                    break;

                case "nodes":
                    var nodes = IoController.GetNodes();
                    Console.WriteLine($"Nodes: {string.Join(", ", nodes)}");
                    break;

                case "get":
                    Console.WriteLine("Input (i) or Output (o)? ");
                    var ioDecision = Console.ReadLine()?.ToLower();
                    if (ioDecision == "i" || ioDecision == "input")
                    {
                        GetInputValue();
                    }
                    else if (ioDecision == "o" || ioDecision == "output")
                    {
                        GetOutputValue();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'i' or 'o'.");
                    }

                    break;

                case "get-all":
                    GetAllInputValues();
                    break;

                case "set":
                    Console.WriteLine("Enter output ID:");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var outputId = (ProfibusOutputId)id;
                        Console.WriteLine("Enter state (on/off):");
                        var state = Console.ReadLine()?.ToLower();
                        bool newState = state == "on";

                        IoController.SetBit(outputId, newState);
                        Console.WriteLine($"Output {outputId} set to {newState}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid output ID.");
                    }

                    break;

                case "exit":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid command. Use 'list', 'nodes', 'get', 'get-all', 'set', or 'exit'.");
                    break;
            }
        }

        IoController?.Shutdown();
    }

    private static void GetInputValue()
    {
        Console.WriteLine("Enter input ID:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            ProfibusInputId inputId = (ProfibusInputId)id;
            var status = IoController.GetBit(inputId);
            Console.WriteLine($"Status for input {inputId}: {status}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid input ID.");
        }
    }

    private static void GetOutputValue()
    {
        Console.WriteLine("Enter output ID:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            ProfibusOutputId outputId = (ProfibusOutputId)id;
            var status = IoController.GetBit(outputId);
            Console.WriteLine($"Status for output {outputId}: {status}");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid output ID.");
        }
    }

    private static void GetAllInputValues()
    {
        Console.WriteLine("Getting the state of all inputs:");
        foreach (var mapping in IoMappings.InputMappings)
        {
            var inputId = mapping.Key;
            var status = IoController.GetBit(inputId);
            Console.WriteLine($"ID: {(int)inputId}, Name: {inputId}, Status: {status}");
        }
    }

    public static void ListMappings()
    {
        Console.WriteLine("Input Mappings:");
        foreach (var mapping in IoMappings.InputMappings)
        {
            Console.WriteLine($"ID: {(int)mapping.Key}, Name: {mapping.Key}, Node: {mapping.Value.Node}, Offset: {mapping.Value.Offset}");
        }

        Console.WriteLine("\nOutput Mappings:");
        foreach (var mapping in IoMappings.OutputMappings)
        {
            Console.WriteLine($"ID: {(int)mapping.Key}, Name: {mapping.Key}, Node: {mapping.Value.Node}, Offset: {mapping.Value.Offset}");
        }
    }
}
