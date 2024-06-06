using Summer.AsyncEventNotifier;
using Summer.AsyncEvents.Attributes;
using Summer.Components.Example.Events;
using Summer.DependencyInjection.Interfaces;

namespace Summer.Components.Example;

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

    [AsyncEventListener(typeof(TimeChangedEvent))]
    public async Task OnClockTimeChanged(TimeChangedEvent timeChangedEvent)
    {
        if (!_alarmTimes.Remove(timeChangedEvent.Time)) return;
        
        var alarmSounded = new AlarmEvent(timeChangedEvent.Time);
        await EventNotifier.Notify(alarmSounded);
        Console.WriteLine($"[{timeChangedEvent.Time}] Your {timeChangedEvent.Time} alarms should have sounded!");
    }
}