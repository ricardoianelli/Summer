using System.Reflection;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Exceptions;
using Summer.EventNotifier.Attributes;
using Summer.EventNotifier.Interfaces;

namespace Summer.EventNotifier;

public static class EventNotifier
{
    private static readonly Dictionary<Type, List<EventHandlerWrapper>> EventListeners = new();

    public delegate Task AsyncEventHandler<in T>(T msg) where T : IEvent;

    public delegate void SyncEventHandler<in T>(T msg) where T : IEvent;

    private record EventHandlerWrapper(Type InstanceType, object Instance, MethodInfo Method);

    public static void DiscoverEventHandlers()
    {
        Console.WriteLine("===============================================");
        Console.WriteLine("Discovering event handlers...");
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract &&
                        t.GetMethods().Any(m => m.GetCustomAttributes(typeof(EventListener), true).Length > 0))
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
                Console.WriteLine($"- Couldn't find an instance of the component {type} during event subscription.");
                continue;
            }

            var component = Convert.ChangeType(instance, type);

            foreach (var method in type.GetMethods())
            {
                var listenerAttribute = method.GetCustomAttribute<EventListener>();
                if (listenerAttribute == null) continue;

                if (!IsAValidEventHandler(method))
                {
                    Console.WriteLine(
                        $"- Method {method.Name} couldn't be subscribed to event {listenerAttribute.EventType.Name} because of an invalid method signature.");
                    continue;
                }

                var wrapper = new EventHandlerWrapper(type, component, method);

                Subscribe(listenerAttribute.EventType, wrapper);
            }
        }

        Console.WriteLine("Finished discovering async event handlers.");
    }

    private static bool IsAValidEventHandler(MethodBase method)
    {
        var parameters = method.GetParameters();
        return parameters.Length == 1 && typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType);
    }

    public static void Subscribe<T>(AsyncEventHandler<T> handler) where T : IEvent
    {
        if (handler.Target is null)
        {
            throw new ArgumentException("Null handler during async event handler subscription.");
        }

        ;

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

    public static void Notify(IEvent @event)
    {
        var eventType = @event.GetType();
        if (!EventListeners.TryGetValue(eventType, out var handlers))
        {
            return;
        }

        try
        {
            foreach (var handler in handlers)
            {
                var callResult = handler.Method.Invoke(handler.Instance, [@event]);
                if (callResult is Task task)
                {
                    task.Wait();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Event notification exception: {e}");
            throw;
        }
    }

    public static async Task NotifyAsync(IEvent @event, bool configureAwait=false)
    {
        try
        {
            var eventType = @event.GetType();
            if (!EventListeners.TryGetValue(eventType, out var handlers))
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                try
                {
                    var task = handler.Method.Invoke(handler.Instance, [@event]) as Task ?? Task.CompletedTask;
                    tasks.Add(task);
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        $"A synchronous event handler on an async notification call threw an exception: {e}");
                }
            }

            await Task.WhenAll(tasks).ConfigureAwait(configureAwait);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Async event notification exception: {e}");
            throw;
        }
    }
}