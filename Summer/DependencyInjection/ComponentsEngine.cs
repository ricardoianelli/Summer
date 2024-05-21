using System.Reflection;
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
        Console.WriteLine($"Starting ComponentsEngine.");
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
        Console.WriteLine($"Discovering Components...");
        var assembly = Assembly.GetExecutingAssembly();
        
        var componentTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IComponent)) && !t.IsAbstract)
            .ToList();
        
        foreach (var componentType in componentTypes)
        {
            ComponentStore.Register(componentType);
            Console.WriteLine($"Registered Component of type {componentType.Name}");
        }
        
        return componentTypes;
    }

    private static void InjectDependencies(List<Type> componentTypes)
    {
        
    }

    private static void Initialize(List<Type> componentTypes)
    {
        
    }
}