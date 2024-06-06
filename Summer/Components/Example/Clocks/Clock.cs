using Summer.AsyncEventNotifier;
using Summer.AsyncEvents.Attributes;
using Summer.Components.Example.Events;
using Summer.DependencyInjection.Interfaces;

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
    
    private static async Task OnTimeChanged(ClockTime clockTime)
    {
        var timeChangedEvent = new TimeChangedEvent(clockTime);
        await EventNotifier.Notify(timeChangedEvent);
    }
    
    private static async Task ObserveTime()
    {
        while (true)
        {
            var time = DateTime.Now;
            var msUntilNextSecond = 1000 - time.Millisecond + 100; // 100ms of margin
            var nextSecDelay = Task.Delay(msUntilNextSecond);
            
            // We don't want to await this because it will add an extra delay to our 1s timer.
            // We can just fire and forget, it should be totally fine.
            OnTimeChanged(new ClockTime(time.Hour, time.Minute, time.Second));
            await nextSecDelay;
        }
    }

    [AsyncEventListener(typeof(AlarmEvent))]
    public virtual async Task OnAlarmSounded(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}! It's {alarmEvent.AlarmTime}!");
        return;
    }
}