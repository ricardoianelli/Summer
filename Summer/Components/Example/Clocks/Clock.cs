using Summer.Components.Example.Events;
using Summer.DependencyInjection.Interfaces;
using Summer.EventNotifier.Attributes;

namespace Summer.Components.Example.Clocks;

public abstract class Clock : IComponent
{
    private static bool _isObservingTime = false;
    protected abstract string GetSound();
    
    public void Initialize()
    {
        if (_isObservingTime) return;
        
        _isObservingTime = true; 
        Task.Run(ObserveTime);
    }

    protected static string GetFormattedDateNowString()
    {
        return DateTime.Now.ToString("HH:mm:ss.fff");
    }
    
    private static void OnTimeChanged(ClockTime clockTime)
    {
        var timeChangedEvent = new TimeChangedEvent(clockTime);
        EventNotifier.EventNotifier.Notify(timeChangedEvent);
    }
    
    private static async Task ObserveTime()
    {
        while (true)
        {
            var time = DateTime.Now;
            var msUntilNextSecond = 1000 - time.Millisecond + 100; // 100ms of margin
            var nextSecDelay = Task.Delay(msUntilNextSecond);
            
            // We don't want to await this because it could take more than a sec and that would be problematic.
            // We can just fire and forget, it should be totally fine.
            OnTimeChanged(new ClockTime(time.Hour, time.Minute, time.Second));
            await nextSecDelay;
        }
    }

    [EventListener(typeof(AlarmEvent))]
    public virtual async Task OnAlarmSoundedAsync(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"ASYNC -> [{GetFormattedDateNowString()}] {GetSound()}! It's {alarmEvent.AlarmTime}!");
        throw new Exception("Oh no, the clock fell and broke!");
    }
    
    [EventListener(typeof(AlarmEvent))]
    public virtual void OnAlarmSounded(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"SYNC -> [{GetFormattedDateNowString()}] {GetSound()}! It's {alarmEvent.AlarmTime}!");
    }
}