using Summer.AsyncEvents.Attributes;
using Summer.DependencyInjection.Interfaces;

namespace Summer.AsyncEvents;

public class EventSubscriber : IComponent
{
    public EventSubscriber()
    {
        EventNotifier.Subscribe<AsyncEvent2>(OnEvent2);    
    }
    
    [AsyncEventListener(typeof(AsyncEvent1))]
    public async Task OnEvent1(AsyncEvent1 asyncEvent)
    {
        var event1 = asyncEvent;
        if (event1.Data == "First!")
        {
            Console.WriteLine("Awesome 1!");
        }
    }
    
    public async Task OnEvent2(AsyncEvent2 asyncEvent)
    {
        if (asyncEvent.Data == "Second!")
        {
            Console.WriteLine("Awesome 2!");
        }
    }
}