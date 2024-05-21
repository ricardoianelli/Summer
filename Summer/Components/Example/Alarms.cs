using Summer.DependencyInjection.Attributes;
using Summer.DependencyInjection.Interfaces;

namespace Summer.Components.Example;

public class Alarms : IComponent
{
    [Inject]
    private Clock _clock;
    
    private HashSet<ClockTime> _alarmTimes;

    public void Initialize()
    {
        _alarmTimes = new HashSet<ClockTime>();
        _clock.TimeChanged += OnClockTimeChanged;
    }

    private void OnClockTimeChanged(object? sender, ClockTime clockTime)
    {
        if (_alarmTimes.Remove(clockTime))
        {
            Console.WriteLine($"[{clockTime}] PIPIPIPIPIPIPI! Your {clockTime} alarm just sounded!");
        }
    }
    
    public void AddAlarm(int hour, int minute, int second)
    {
        var alarmTime = new ClockTime(hour, minute, second);
        _alarmTimes.Add(alarmTime);
        var formattedDate = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[{formattedDate}] Added a new alarm for {alarmTime}");
    }
}