using Summer.Components.Example;
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static void Main(string[] args)
    {
        ComponentsEngine.Start();
        Console.WriteLine("Hello, Summer!");

        var alarms = ComponentsEngine.GetComponent<Alarms>();
        var timeNow = DateTime.Now;
        alarms?.AddAlarm(timeNow.Hour, timeNow.Minute, timeNow.Second+10);
        
        Console.WriteLine("Press enter at any time to exit the program!");
        Console.ReadLine();
    }
}