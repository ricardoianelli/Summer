using System.Reflection;
using Summer.AsyncEvents.Attributes;
using Summer.AsyncEvents.Interfaces;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Exceptions;

namespace Summer.AsyncEventNotifier;

public static class EventNotifier
{
    private static readonly Dictionary<Type, List<EventHandlerWrapper>> EventListeners = new();
    public delegate Task AsyncEventHandler<in T>(T msg) where T : IAsyncEvent;

    private record EventHandlerWrapper(Type InstanceType, object Instance, MethodInfo Method);

    public static void DiscoverEventHandlers(Assembly assembly)
    {
        var startTime = DateTime.UtcNow;
        
        Console.WriteLine("=> Discovering async event handlers...");

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.GetMethods().Any(m => m.GetCustomAttributes(typeof(AsyncEventListener), true).Length > 0))
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
                Console.WriteLine($"-- Couldn't find an instance of the component {type} during event subscription.");
                continue;
            }

            var component = Convert.ChangeType(instance, type);

            foreach (var method in type.GetMethods())
            {
                var listenerAttribute = method.GetCustomAttribute<AsyncEventListener>();
                if (listenerAttribute == null) continue;

                if (!IsAValidAsyncEventHandler(method))
                {
                    Console.WriteLine(
                        $"-- Method {method.Name} couldn't be subscribed to event {listenerAttribute.EventType.Name} because of an invalid method signature.");
                    continue;
                }

                var wrapper = new EventHandlerWrapper(type, component, method);

                Subscribe(listenerAttribute.EventType, wrapper);
            }
        }
        
        Console.WriteLine(
            $"Found {EventListeners.Count} components. (Time: {(DateTime.UtcNow - startTime).Milliseconds} ms)");
    }

    private static bool IsAValidAsyncEventHandler(MethodBase method)
    {
        var parameters = method.GetParameters();
        return parameters.Length == 1 && typeof(IAsyncEvent).IsAssignableFrom(parameters[0].ParameterType);
    }

    public static void Subscribe<T>(AsyncEventHandler<T> handler) where T : IAsyncEvent
    {
        if (handler.Target is null)
        {
            throw new ArgumentException("Null handler during async event handler subscription.");
        };
        
        var type = typeof(T);
        
        if (!EventListeners.ContainsKey(type))
        {
            EventListeners[type] = new List<EventHandlerWrapper>();
        }
        
        var handlerWrapper = new EventHandlerWrapper(handler.Target.GetType(), handler.Target, handler.Method);

        Console.WriteLine(
            $"- Adding event listener for {type.Name} - {handlerWrapper.InstanceType.Name}.{handlerWrapper.Method.Name}");
        EventListeners[type].Add(handlerWrapper);
    }

    private static void Subscribe(Type eventType, EventHandlerWrapper handlerWrapper)
    {
        if (!EventListeners.ContainsKey(eventType))
        {
            EventListeners[eventType] = new List<EventHandlerWrapper>();
        }

        Console.WriteLine(
            $"- Adding event listener for {eventType.Name} - {handlerWrapper.InstanceType.Name}.{handlerWrapper.Method.Name}");
        EventListeners[eventType].Add(handlerWrapper);
    }

    public static async Task Notify(IAsyncEvent asyncEvent)
    {
        try
        {
            var eventType = asyncEvent.GetType();
            if (!EventListeners.TryGetValue(eventType, out var handlers))
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                var task = handler.Method.Invoke(handler.Instance, [asyncEvent]) as Task ?? Task.CompletedTask;
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}