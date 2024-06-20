using Summer.Components.Example;
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static async Task Main(string[] args)
    {
        ComponentsEngine.Start();
        EventNotifier.EventNotifier.DiscoverEventHandlers();
        ComponentsEngine.Initialize();
        
        Console.WriteLine("===============================================");
        Console.WriteLine("Hello, Summer!");
        Console.WriteLine("- Press enter at any time to exit the program!");
        Console.WriteLine("===============================================\n");
        
        var alarm = ComponentsEngine.GetComponent<Alarm>();
        if (alarm is null)
        {
            throw new Exception("Couldn't find Alarm component!");
        }

        alarm.AddAlarm(DateTime.Now.AddSeconds(5));
        
        Console.ReadLine();
    }
}