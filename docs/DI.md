## **Dependency Injection example**

Your Program.cs should already look like this:

```csharp
using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static void Main(string[] args)
    {
        ComponentsEngine.Start();
        Console.WriteLine("Hello, Summer!");
    }
}
```

The call to **ComponentsEngine.Start()** will already:
- Search for classes that implements **IComponent**.
- Create instances of them.
- Do dependency injection on its fields/properties marked with [Inject].
- Call **Initialize()** method on each one of them.

But if you just execute it like that, you'll probably see something similar to this:

```
Starting ComponentsEngine.
Discovering Components...       
Found 0 components! (Time: 2 ms)
Hello, Summer!    
```

Because we simply don't have any component defined yet! _(We actually do in the tests project, you should check them out, but let's pretend we don't)_

Let's start by creating a new folder Components to keep our components organized, and inside of it, add our first component (I added it also inside an Example folder):

```csharp
using Summer.DependencyInjection.Interfaces;

namespace Summer.Components.Example;

public class Alarms : IComponent
{
    private const int CheckDelayInMs = 1000;
    private record AlarmTime(int Hour, int Minute);
    private HashSet<AlarmTime> _alarmTimes;

    public void Initialize()
    {
        _alarmTimes = new HashSet<AlarmTime>();
        Task.Run(CheckAlarms);
    }

    private async Task CheckAlarms()
    {
        while (true)
        {
            var currentTime = DateTime.Now;
            var alarmTime = new AlarmTime(currentTime.Hour, currentTime.Minute);
            
            if (_alarmTimes.Remove(alarmTime))
            {
                var formattedDate = DateTime.Now.ToString("HH:mm");
                Console.WriteLine($"[{formattedDate}] PIPIPIPIPIPIPI! Your {alarmTime.Hour}:{alarmTime.Minute} alarm just sounded!");
            }
            
            await Task.Delay(CheckDelayInMs);
        }
    }
    
    public void AddAlarm(int hour, int minute)
    {
        var alarmTime = new AlarmTime(hour, minute);
        _alarmTimes.Add(alarmTime);
        var formattedDate = DateTime.Now.ToString("HH:mm");
        Console.WriteLine($"[{formattedDate}] Added a new alarm for {hour}:{minute}");
    }
}
```

Now, in our Program.cs, we can make the Main() method look like this:

```csharp
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
        alarms?.AddAlarm(timeNow.Hour, timeNow.Minute+1);
        
        Console.WriteLine("Press enter at any time to exit the program!");
        Console.ReadLine();
    }
}
```

Running our program with this we would get something like this:

```
Starting ComponentsEngine.
Discovering Components...                   
Registered Component of type Alarms         
Injecting Components...                     
Initialized 1 components! (Time: 4 ms)      
Hello, Summer!                              
[23:13] Added a new alarm for 23:14          
Press enter at any time to exit the program!
[23:14] PIPIPIPIPIPIPI! Your 23:14 alarm just sounded!

```

In this example we only made use of the singleton that is created automatically for us, but it's doing more than one thing, maybe we can improve it.
What if we also leverage dependency injection?

First, let's create a ClockTime record that can be used by both components:

```csharp
namespace Summer.Components.Example;

public record ClockTime(int Hour, int Minute, int Second)
{
    public override string ToString()
    {
        return $"{Hour}:{Minute}:{Second}";
    }
}
```

Then, let's create a Clock component:

```csharp
using Summer.DependencyInjection.Interfaces;

namespace Summer.Components.Example;

public class Clock : IComponent
{
    public event EventHandler<ClockTime> TimeChanged;

    public void Initialize()
    {
        Task.Run(ObserveTime);
    }

    private async Task ObserveTime()
    {
        while (true)
        {
            var time = DateTime.Now;
            OnTimeChanged(new ClockTime(time.Hour, time.Minute, time.Second));
            await Task.Delay(1000); //Wait one second
        }
    }

    protected virtual void OnTimeChanged(ClockTime clockTime)
    {
        TimeChanged?.Invoke(this, clockTime);
    }
}
```

Now, let's refactor our Alarms component to use the Clock component:
```csharp
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
```

And finally, since we added seconds to our alarm, let's fix it at Main():

```csharp
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
```

After running this code, we can see that it still works:

```
Starting ComponentsEngine.
Discovering Components...                   
Registered Component of type Alarms         
Registered Component of type Clock          
Injecting Components...                     
Injected component Clock into Alarms.       
Initialized 2 components! (Time: 6 ms)      
Hello, Summer!                              
[23:17:32] Added a new alarm for 23:17:42   
Press enter at any time to exit the program!
[23:17:42] PIPIPIPIPIPIPI! Your 23:17:42 alarm just sounded!

```

Now, our code looks much more organized and respects the Single-Responsibility Principle from the SOLID principles.

This is just a very simple example, but it shows how easy it is to use the DI capabilities without even having to declare each one in a config method like you have to do with .Net Core DI.





