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