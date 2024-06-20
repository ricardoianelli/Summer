using Summer.Components.Example.Events;

namespace Summer.Components.Example.Clocks;

public class RegularClock : Clock
{
    protected override string GetSound()
    {
        return "Pi Pi Pi Pi";
    }

    public override void OnAlarmSounded(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}!");
        Console.WriteLine($"[{GetFormattedDateNowString()}] Snoring...");
        Thread.Sleep(2000);
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}!!!!");
        Console.WriteLine($"[{GetFormattedDateNowString()}] Oh no, I'm late!");
        
    }
}