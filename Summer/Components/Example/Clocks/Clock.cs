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
        return DateTime.Now.ToString("HH:mm:ss");
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
            
            // I don't want to wait here, I want to scream "Hey, stuff changed!" and not care about whoever hears it.
            // If I wait, I would have to do extra calculations to ensure the 1s timeframe between runs.
            // Think about it that way: If I lose 0.2s here waiting, and then wait for 1s, I actually had a 
            // Difference of 1.2 seconds between OnTimeChanged events, instead of 1s. After 5s, I would be 1s late.
            await OnTimeChanged(new ClockTime(time.Hour, time.Minute, time.Second)); 
            
            await Task.Delay(1000); //The clock goes 1s at a time, so let's wait for 1s.
        }
    }

    [AsyncEventListener(typeof(AlarmEvent))]
    public virtual async Task OnAlarmSounded(AlarmEvent alarmEvent)
    {
        Console.WriteLine($"[{GetFormattedDateNowString()}] {GetSound()}! It's {alarmEvent.AlarmTime}!");
        return;
    }
}