using System.Reflection;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Exceptions;
using Summer.Events.Attributes;
using Summer.Events.Interfaces;

namespace Summer.Events;

public static class EventNotifier
{
    private static readonly Dictionary<Type, List<EventHandlerWrapper>> AsyncEventListeners = new();
    private static readonly Dictionary<Type, List<EventHandlerWrapper>> EventListeners = new();

    public delegate Task AsyncEventHandler<in T>(T msg) where T : IEvent;

    public delegate void SyncEventHandler<in T>(T msg) where T : IEvent;

    private record EventHandlerWrapper(Type InstanceType, object Instance, MethodInfo Method);

    public static void DiscoverEventHandlers(Assembly assembly)
    {
        Console.WriteLine("===============================================");
        Console.WriteLine("Discovering event handlers...");

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
            catch (NotAValidComponentException e)
            {
                Console.WriteLine(e.Message);
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

    public static void Subscribe<T>(SyncEventHandler<T> handler) where T : IEvent
    {
        if (handler.Target is null)
        {
            throw new ArgumentException("Null handler during event handler subscription.");
        }

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

    public static void Subscribe<T>(AsyncEventHandler<T> handler) where T : IEvent
    {
        if (handler.Target is null)
        {
            throw new ArgumentException("Null handler during async event handler subscription.");
        }

        var type = typeof(T);

        if (!AsyncEventListeners.ContainsKey(type))
        {
            AsyncEventListeners[type] = new List<EventHandlerWrapper>();
        }

        var handlerWrapper = new EventHandlerWrapper(handler.Target.GetType(), handler.Target, handler.Method);

        Console.WriteLine(
            $"- Adding async event listener for {type.Name} - {handlerWrapper.InstanceType.Name}.{handlerWrapper.Method.Name}");

        AsyncEventListeners[type].Add(handlerWrapper);
    }

    private static void Subscribe(Type eventType, EventHandlerWrapper handlerWrapper)
    {
        var dictionary = IsAsync(handlerWrapper.Method) ? AsyncEventListeners : EventListeners;

        if (!dictionary.ContainsKey(eventType))
        {
            dictionary[eventType] = new List<EventHandlerWrapper>();
        }

        Console.WriteLine(
            $"- Adding event listener for {eventType.Name} - {handlerWrapper.InstanceType.Name}.{handlerWrapper.Method.Name}");
        dictionary[eventType].Add(handlerWrapper);
    }

    private static void ExecuteSyncEventHandlers(IEvent @event)
    {
        var eventType = @event.GetType();
        if (!EventListeners.TryGetValue(eventType, out var handlers)) return;

        foreach (var handler in handlers)
        {
            handler.Method.Invoke(handler.Instance, [@event]);
        }
    }

    private static async Task ExecuteAsyncEventHandlers(IEvent @event)
    {
        var eventType = @event.GetType();
        if (!AsyncEventListeners.TryGetValue(eventType, out var handlers)) return;

        var exceptions = new List<Exception>();
        var exceptionsLock = new object();

        await Parallel.ForEachAsync(handlers, async (handler, cancellationToken) =>
        {
            try
            {
                await (handler.Method.Invoke(handler.Instance, [@event]) as Task ?? Task.CompletedTask);
            }
            catch (Exception e)
            {
                lock (exceptionsLock)
                {
                    exceptions.Add(e);
                }
            }
        });
        
        switch (exceptions.Count)
        {
            case 1:
                throw exceptions[0];
            case > 1:
                throw new AggregateException(exceptions);
        }
    }

    public static void Notify(IEvent @event, bool ignoreAsync = false)
    {
        try
        {
            ExecuteSyncEventHandlers(@event);

            if (ignoreAsync) return;
            ExecuteAsyncEventHandlers(@event).Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Event notification exception: {e}");
            throw;
        }
    }

    public static async Task NotifyAsync(IEvent @event, bool ignoreSync = false)
    {
        try
        {
            var tasks = new List<Task>
            {
                ExecuteAsyncEventHandlers(@event)
            };

            if (!ignoreSync)
            {
                tasks.Add(Task.Run(() => ExecuteSyncEventHandlers(@event)));
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Async event notification exception: {e}");
            throw;
        }
    }

    private static bool IsAsync(MethodInfo method)
    {
        return method.ReturnType == typeof(Task);
    }
}