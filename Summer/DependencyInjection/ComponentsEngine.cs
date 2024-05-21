using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public static class ComponentsEngine
{
    private static readonly ComponentStore ComponentStore = new();
    
    /// <summary>
    /// Discover and initialize Components.
    /// </summary>
    public static async Task Start()
    {
        var componentTypes = await Discover();
        await InjectDependencies(componentTypes);
        await Initialize(componentTypes);
    }

    public static T? GetComponent<T>() where T : class, IComponent
    {
        var component = ComponentStore.Find(typeof(T));
        return component as T;
    }

    public static object? Find(Type type)
    {
        return ComponentStore.Find(type);
    }

    private static async Task<List<Type>> Discover()
    {
        return new List<Type>();
    }

    private static Task InjectDependencies(List<Type> componentTypes)
    {
        return Task.CompletedTask;
    }

    private static Task Initialize(List<Type> componentTypes)
    {
        return Task.CompletedTask;
    }
}