using System.Reflection;
using Summer.DependencyInjection.Attributes;
using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public static class ComponentsEngine
{
    private enum ComponentsEngineState
    {
        None,
        Started,
        Initialized
    }
    
    public static Assembly ExecutingAssembly { get; set; } = Assembly.GetExecutingAssembly();

    private static readonly ComponentStore ComponentStore = new();
    private static List<object?> _temporaryComponentList;
    private static DateTime _startTime;
    private static ComponentsEngineState _state = ComponentsEngineState.None;

    /// <summary>
    /// Discover Components and do dependency injection.
    /// </summary>
    public static void Start()
    {
        if (_state != ComponentsEngineState.None) return;
        
        _startTime = DateTime.UtcNow;
        Console.WriteLine($"Starting ComponentsEngine...");

        try
        {
            _temporaryComponentList = Discover();
            if (_temporaryComponentList.Count == 0)
            {
                Console.WriteLine(
                    $"Found 0 components. (Time: {(DateTime.UtcNow - _startTime).Milliseconds} ms)");
                return;
            }

            InjectDependencies(_temporaryComponentList);
            Console.WriteLine(
                $"Injected dependencies. (Time: {(DateTime.UtcNow - _startTime).Milliseconds} ms)");
            
            _state = ComponentsEngineState.Started;
            Console.WriteLine($"ComponentsEngine started. Please remember to initialize it by calling Initialize().");
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error starting ComponentsEngine: " + e);
            throw;
        }
    }

    /// <summary>
    /// Initialize Components.
    /// </summary>
    public static void Initialize()
    {
        if (_state != ComponentsEngineState.Started) return;
        
        _startTime = DateTime.UtcNow;
        Console.WriteLine($"Initializing Components...");

        try
        {
            Initialize(_temporaryComponentList);

            Console.WriteLine(
                $"Initialized {_temporaryComponentList.Count} components! (Time: {(DateTime.UtcNow - _startTime).Milliseconds} ms)");
            
            _temporaryComponentList.Clear();
            _state = ComponentsEngineState.Initialized;
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error initializing ComponentsEngine: " + e);
            throw;
        }
    }

    public static T? GetComponent<T>() where T : class, IComponent
    {
        var component = ComponentStore.Find(typeof(T));
        return component as T;
    }

    public static object? GetComponent(Type type)
    {
        return ComponentStore.Find(type);
    }

    private static List<Object?> Discover()
    {
        Console.WriteLine($"Discovering Components...");
        
        var componentTypes = ExecutingAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IComponent))
                        && !t.IsAbstract
                        && !Attribute.IsDefined(t, typeof(IgnoreComponent))
            ).ToList();
        
        var components = new List<object?>(componentTypes.Count);
        
        foreach (var componentType in componentTypes)
        {
            components.Add(ComponentStore.Register(componentType));
            Console.WriteLine($"Registered Component of type {componentType.Name}");
        }

        return components;
    }

    private static void InjectDependencies(List<object?> components)
    {
        Console.WriteLine($"Injecting Components...");
        foreach (var component in components)
        {
            if (component is null) continue;
            
            var componentType = component.GetType();
            var componentProperties =
                componentType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(property => Attribute.IsDefined(property, typeof(Inject)));

            InjectIntoProperties(component, componentType, componentProperties);

            var componentFields =
                componentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(property => Attribute.IsDefined(property, typeof(Inject)));

            InjectIntoFields(component, componentType, componentFields);
        }
    }

    private static void InjectIntoProperties(object? componentInstance, Type componentType,
        IEnumerable<PropertyInfo> componentProperties)
    {
        foreach (var property in componentProperties)
        {
            var propertyType = property.PropertyType;
            var componentToBeInjected = ComponentStore.Find(propertyType);

            if (componentToBeInjected == null)
            {
                // Could just register it again, but it should have created it during Discover(), so it shouldn't happen. 
                Console.WriteLine(
                    $"Could not find a component to inject into {componentType.Name}.{propertyType.Name}.");
                continue;
            }

            if (!property.CanWrite)
            {
                // In the future I could just change the backing field generated by C#, but let's keep it simple for now.
                Console.WriteLine($"Property {componentType.Name}.{propertyType.Name} does not have a 'set' accessor.");
                continue;
            }

            property.SetValue(componentInstance, componentToBeInjected);
            Console.WriteLine($"Injected component {propertyType.Name} into {componentType.Name}.");
        }
    }

    private static void InjectIntoFields(object? componentInstance, Type componentType,
        IEnumerable<FieldInfo> componentFields)
    {
        foreach (var field in componentFields)
        {
            var fieldType = field.FieldType;
            var componentToBeInjected = ComponentStore.Find(fieldType);

            if (componentToBeInjected == null)
            {
                // Could just register it again, but it should have created it during Discover(), so it shouldn't happen. 
                Console.WriteLine($"Could not find a component to inject into {componentType.Name}.{fieldType.Name}.");
                continue;
            }

            field.SetValue(componentInstance, componentToBeInjected);
            Console.WriteLine($"Injected component {fieldType.Name} into {componentType.Name}.");
        }
    }

    private static void Initialize(List<object?> components)
    {
        foreach (IComponent? component in components)
        {
            component?.Initialize();
        }
    }
}