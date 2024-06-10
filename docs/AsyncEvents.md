 ## **Async Events example**

Make your Program.cs look like this:

```csharp
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static async Task Main(string[] args)
    {
        ComponentsEngine.Start();
        
        Console.WriteLine("===============================================");
        Console.WriteLine("Hello, Summer!");
        Console.WriteLine("- Press enter at any time to exit the program!");
        Console.WriteLine("===============================================\n");
        
        var alarm = ComponentsEngine.GetComponent<Alarm>();
        if (alarm is null)
        {
            throw new Exception("Couldn't find Alarm component!");
        }

        alarm.AddAlarm(DateTime.Now.AddSeconds(5));
        
        Console.ReadLine();
    }
}
```

As you can see, it's not too different from the [Dependency Injection example](https://github.com/ricardoianelli/Summer/blob/main/docs/DI.md).

The new thing happening here is that now your components engine will also look for async event handlers.

We've changed a little bit the previous example, now we have something like this:

** Alarm.cs **

This class is responsible for setting up alarms that will sound at specific times.
An alarm is seen by all clocks that are working, so you don't need to associate alarms to clocks manually.


```csharp
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
    }
}
```

A few points to note about this:
- It's a component, so one instance will already be created for you and will be running automatically.
- It has methods for adding a new alarm.
- It has a method marked with **[AsyncEventListener(typeof(TimeChangedEvent))]**. That's how you define a method that is an event handler for an async event.
You basically add **[AsyncEventListener(typeof(EVENT_TYPE))]** to the top of the method, and replace *EVENT_TYPE* with the type of the async event you're listening to.
Then, your method signature MUST obey a few rules:
  - Needs to be public.
  - Needs have the async Task return type.
  - Needs to have only one parameter, being of the type of the async event you're listening to.
- It has an await EventNotifier.Notify(alarmSounded) call. This is how you notify an async event to all of its listeners.

Some benefits that we can already see in here over the default .Net event system is:
- Since our events are notified Asynchronously, there's no issues if one of them fail somehow, and we don't need to block the thread if we don't want to.
- As you can see, there's little to no decoupling. You just need to know about the event you want to subscribe/notify, nothing else.

*** ClockTime.cs ***

We have this simple record to keep our time data.
```csharp
namespace Summer.Components.Example;
public record ClockTime(int Hour, int Minute, int Second)
{
    public override string ToString() => $"{Hour}:{Minute}:{Second}";
}
```

And the two events that are being used in the Alarm component:

*** AlarmEvent.cs ***

```csharp
using Summer.AsyncEvents.Interfaces;

namespace Summer.Components.Example.Events;

public record AlarmEvent(ClockTime AlarmTime) : IAsyncEvent;
```

*** TimeChangedEvent.cs ***

```csharp
using Summer.AsyncEvents.Interfaces;

namespace Summer.Components.Example.Events;

public record TimeChangedEvent(ClockTime Time) : IAsyncEvent;
```

Note that both implement the IAsyncEvent interface. Every async event that you want to create needs to implement this interface.

We also have clocks in our example. Clocks are responsible for counting the time and sounding any alarms that may happen.

We have the base class for all clock:

*** Clock.cs ***

```csharp
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
            
            // We don't want to await this because it could take more than a sec and that would be problematic.
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
```

And two clock implementations:

*** RegularClock.cs ***

```csharp
namespace Summer.Components.Example.Clocks;

public class RegularClock : Clock
{
    protected override string GetSound()
    {
        return "Pi Pi Pi Pi";
    }
}
```

*** TimeChangedEvent.cs ***

```csharp
using Summer.Components.Example.Events;

namespace Summer.Components.Example.Clocks;

public class CucoClock : Clock
{
    protected override string GetSound()
    {
        return "Cuco, Cuco";
    }

    public override async Task OnAlarmSounded(AlarmEvent alarmEvent)
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
```

CucoClock adds some snoring to make so that the async operation will take several seconds to complete.
This was done in order to demonstrate the asynchronous nature of the events.

Looking at the flow, we can see that:

- Clocks will report every second using an async event called TimeChangedEvent.
- Alarm is subscribing to this, and will check to see if there's any alarm set up for the current time.
- If there is, Alarm will raise an AlarmEvent.
- Clocks are subscribing to AlarmEvents, and will display an alarm message when a new AlarmEvent is notified.

If we look at our Program.cs again, we can see that we're:
- Getting a reference to the Alarm component.
- Checking if it's not null.
- Adding an alarm for 5s in the future.

So if we run our program now, we should expect the clocks to sound 5s after the program initialization.
Let's run it and see the output:

```
===============================================
Starting ComponentsEngine...
=> Discovering Components...
--> Registered Component of type Alarm
--> Registered Component of type CucoClock
--> Registered Component of type RegularClock
Found 3 components. (Time: 3 ms)
=> Injecting Components...
Injected 0 dependencies. (Time: 1 ms)
=> Discovering async event handlers...
- Adding event listener for TimeChangedEvent - Alarm.OnClockTimeChanged
- Adding event listener for AlarmEvent - CucoClock.OnAlarmSounded
- Adding event listener for AlarmEvent - RegularClock.OnAlarmSounded
Found 2 components. (Time: 2 ms)
=> Initializing Components...
Initialized 3 components! (Time: 0 ms)
ComponentsEngine started.
===============================================
Hello, Summer!
- Press enter at any time to exit the program!
===============================================

[13:23:01] Added a new alarm for 13:23:06
[13:23:06.104] Cuco, Cuco! It's 13:23:06!
[13:23:06.105] Snoring...
[13:23:06.105] Pi Pi Pi Pi! It's 13:23:06!
[13:23:07.113] Cuco, Cuco! Stop snoring!! Your 13:23:06 already sounded!
[13:23:07.114] Snoring...
[13:23:08.113] CUCO, CUCO! WAKE UP YOUR LAZY HUMAN! 13:23:06 WAS DECADES AGO!

Process finished with exit code 0.
```

Let's focus just on the last part of it, after the ComponentsEngine initialization:

```
[13:23:01] Added a new alarm for 13:23:06
[13:23:06.104] Cuco, Cuco! It's 13:23:06!
[13:23:06.105] Snoring...
[13:23:06.105] Pi Pi Pi Pi! It's 13:23:06!
[13:23:07.113] Cuco, Cuco! Stop snoring!! Your 13:23:06 already sounded!
[13:23:07.114] Snoring...
[13:23:08.113] CUCO, CUCO! WAKE UP YOUR LAZY HUMAN! 13:23:06 WAS DECADES AGO!
```

At 13:23:01, we can see an alarm being set up for 13:23:06, which is 5s in the future.
At 13:23:06, the Cuco clock starts sounding and then go to snoring.
At the same time, the Regular clock sounds, and you can see that the Cuco clock is still not finished, so the asynchronous nature is working as intended.
After a few more snores, the Cuco clock finishes by 13:23:08.

As you can see, it's really simple and powerful.

An important point is: 
ANY object can subscribe to and notify events, but only Components can make use of the [AsyncEventListener(typeof(EVENT_TYPE))] attribute on methods. 
If you want to subscribe to events in objects that aren't Components, you can do so by using the EventNotifier.Subscribe<T>(handler) method found in the EventNotifier class:
```csharp
public static void Subscribe<T>(AsyncEventHandler<T> handler) where T : IAsyncEvent
```

As long as T implements the IAsyncEvent interface and the handler you pass to this method respects the signature mentioned before, which is:
- It needs to be public.
- It needs have the async Task return type.
- It needs to have only one parameter, being of the type of the async event you're listening to.

