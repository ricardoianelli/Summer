using ConsoleExample.Components.Example.Events;
using Summer.DependencyInjection.Interfaces;
using Summer.Events;
using Summer.Events.Attributes;

namespace ConsoleExample.Components.Example;

public class Alarm : IComponent
{
    private HashSet<ClockTime> _alarmTimes;
    
    public void Initialize()
    {
        _alarmTimes = [];
    }
    
    public void AddAlarm(DateTime dateTime)
    {
        var alarmTime = new ClockTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
        _alarmTimes.Add(alarmTime);
        var formattedDate = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[{formattedDate}] Added a new alarm for {alarmTime}");
    }
    
    public void AddAlarm(int hour, int minute, int second)
    {
        var alarmTime = new ClockTime(hour, minute, second);
        _alarmTimes.Add(alarmTime);
        var formattedDate = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[{formattedDate}] Added a new alarm for {alarmTime}");
    }

    [EventListener(typeof(TimeChangedEvent))]
    public async Task OnClockTimeChanged(TimeChangedEvent timeChangedEvent)
    {
        if (!_alarmTimes.Remove(timeChangedEvent.Time)) return;
        
        var alarmSounded = new AlarmEvent(timeChangedEvent.Time);
        await EventNotifier.NotifyAsync(alarmSounded, true);
    }
}