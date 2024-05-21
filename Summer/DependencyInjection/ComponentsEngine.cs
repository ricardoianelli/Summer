using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public static class ComponentsEngine
{
    private static readonly ComponentStore ComponentStore = new();
    
    /// <summary>
    /// Discover and initialize Components.
    /// </summary>
    public static void Start()
    {
        var componentTypes = Discover();
        InjectDependencies(componentTypes);
        Initialize(componentTypes);
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

    private static List<Type> Discover()
    {
        return new List<Type>();
    }

    private static void InjectDependencies(List<Type> componentTypes)
    {
        
    }

    private static void Initialize(List<Type> componentTypes)
    {
        
    }
}