using System.Reflection;
using MonsantoAutomation.Devices.Applicom;
using Summer.DependencyInjection;

namespace IoTests;

internal class Program
{
    static Task Main(string[] args)
    {
        ComponentsEngine.Start(Assembly.GetExecutingAssembly());
        
        Console.WriteLine("===============================================");
        Console.WriteLine("Hello, Summer!");
        Console.WriteLine("- Press enter at any time to exit the program!");
        Console.WriteLine("===============================================\n");

        Profibus profibus = new Profibus();
        profibus.Connect(1);

        var nodes = new List<ushort>();
        Console.WriteLine("Reading nodes...");
        profibus.GetNodes(nodes);
        Console.WriteLine("Finished reading nodes!");
        
        Console.WriteLine($"Node count: {nodes.Count}");
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            Console.WriteLine($"Node index {i}, value: {node}");
        }
        
        Console.ReadLine();
        return Task.CompletedTask;
    }
}