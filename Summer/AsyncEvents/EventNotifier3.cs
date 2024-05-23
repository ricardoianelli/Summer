using System.Reflection;
using Summer.AsyncEvents.Attributes;
using Summer.AsyncEvents.Interfaces;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Exceptions;

namespace Summer.AsyncEvents;

public static class EventNotifier3
{
    private static readonly Dictionary<Type, AsyncEventHandler<IAsyncEvent>> EventListeners = new();
    public delegate Task AsyncEventHandler<in T>(T msg) where T : IAsyncEvent;

    public static void DiscoverEventHandlers()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetMethods().Any(m => m.GetCustomAttributes(typeof(AsyncEventListener), false).Length > 0))
            .ToList();

        foreach (var type in handlerTypes)
        {
            object? instance = null;
            
            try
            {
                instance = ComponentsEngine.GetComponent(type);
            }
            catch (NotAValidComponentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (instance is null || instance.GetType() != type)
            {
                Console.WriteLine($"Couldn't find an instance of the component {type} during event subscription.");
                continue;
            }

            var component = Convert.ChangeType(instance, type);
            
            foreach (var method in type.GetMethods())
            {
                var listenerAttribute = method.GetCustomAttribute<AsyncEventListener>();
                if (listenerAttribute == null) continue;
                
                if (!IsAValidAsyncEventHandler(method))
                {
                    Console.WriteLine($"Method {method.Name} couldn't be subscribed to event {listenerAttribute.EventType.Name} because of an invalid method signature.");
                    continue;
                }

                AsyncEventHandler<IAsyncEvent> handler = async (msg) =>
                {
                    await (Task)method.Invoke(component, [msg]);
                };

                Subscribe(listenerAttribute.EventType, handler);
            }
        }
    }

    private static bool IsAValidAsyncEventHandler(MethodBase method)
    {
        var parameters = method.GetParameters();
        return parameters.Length == 1 &&
               typeof(IAsyncEvent).IsAssignableFrom(parameters[0].ParameterType);
    }

    public static void Subscribe(Type eventType, AsyncEventHandler<IAsyncEvent> handler)
    {
        if (!IsAValidAsyncEventHandler(handler.Method)) return;

        if (!EventListeners.ContainsKey(eventType))
        {
            EventListeners[eventType] = (_) => Task.CompletedTask;
        }
        
        Console.WriteLine($"Adding event listener for {eventType.Name} - {handler.Method.Name}");
        EventListeners[eventType] += handler;
    }
    
    public static async Task Notify(IAsyncEvent asyncEvent)
    {
        try
        {
            var eventType = asyncEvent.GetType();
            if (!EventListeners.TryGetValue(eventType, out var handler))
            {
                return;
            }

            Console.WriteLine($"Notifying about event {eventType.Name}");
            await handler.Invoke(asyncEvent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}