using Summer.Components.Example.Events;

namespace Summer.Components.Example.Clocks;

public class CucoClock : Clock
{
    protected override string GetSound()
    {
        return "Cuco, Cuco";
    }

    public override async Task OnAlarmSoundedAsync(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}! It's {alarmEvent.AlarmTime}!");
        Console.WriteLine($"[{GetFormattedDateNowString()}] Snoring...");
        await Task.Delay(1000); // Snoring
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}! Stop snoring!! Your {alarmEvent.AlarmTime} already sounded!");
        Console.WriteLine($"[{GetFormattedDateNowString()}] Snoring...");
        await Task.Delay(1000); // Snoring again
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound().ToUpper()}! WAKE UP YOUR LAZY HUMAN! {alarmEvent.AlarmTime} WAS DECADES AGO!");
    }
}