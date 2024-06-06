using Summer.AsyncEventNotifier;
using Summer.Components.Example;
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static async Task Main(string[] args)
    {
        ComponentsEngine.Start();
        EventNotifier.DiscoverEventHandlers();
        ComponentsEngine.Initialize();
        
        Console.WriteLine("Hello, Summer!");
        Console.WriteLine("Press enter at any time to exit the program!");
        
        var alarm = ComponentsEngine.GetComponent<Alarm>();
        if (alarm is null)
        {
            throw new Exception("Couldn't find Alarm component!");
        }

        var alarmTime = DateTime.Now.AddSeconds(5);
        Console.WriteLine($"alarmTime: {alarmTime}");
        alarm.AddAlarm(alarmTime);
        
        Console.ReadLine();
    }
}