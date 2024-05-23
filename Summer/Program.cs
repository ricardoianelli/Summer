using Summer.AsyncEvents;
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static async Task Main(string[] args)
    {
        ComponentsEngine.Start();
        EventNotifier.DiscoverEventHandlers();
        
        Console.WriteLine("Hello, Summer!");

        await EventNotifier.Notify(new AsyncEvent1());
        await EventNotifier.Notify(new AsyncEvent2());
        
        Console.WriteLine("Press enter at any time to exit the program!");
        Console.ReadLine();
    }
}