using System.Reflection;
using Summer.DependencyInjection.Attributes;
using Summer.DependencyInjection.Interfaces;
using Summer.Events;

namespace Summer.DependencyInjection;

public static class ComponentsEngine
{
    public static Assembly ExecutingAssembly { get; set; }

    private static readonly ComponentStore ComponentStore = new();
    private static List<object?> _temporaryComponentList;

    private static int _dependenciesInjected = 0;

    /// <summary>
    /// Discover Components and do dependency injection.
    /// </summary>
    public static void Start()
    {
        Start(Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// Discover Components and do dependency injection.
    /// </summary>
    public static void Start(Assembly executingAssembly)
    {
        ExecutingAssembly = executingAssembly;
        Console.WriteLine("===============================================");
        Console.WriteLine($"Starting ComponentsEngine...");

        try
        {
            DiscoverComponents();
            InjectDependencies();
            DiscoverEventHandlers();
            Initialize();
            Console.WriteLine($"ComponentsEngine started.");
        }
        catch (Exception e)
        {
            Console.WriteLine("There was an error starting ComponentsEngine: " + e);
            throw;
        }
    }

    private static void DiscoverEventHandlers()
    {
        EventNotifier.DiscoverEventHandlers(ExecutingAssembly);
    }

    private static void DiscoverComponents()
    {
        var startTime = DateTime.UtcNow;
        _temporaryComponentList = Discover();
        Console.WriteLine(
            $"Found {_temporaryComponentList.Count} components. (Time: {(DateTime.UtcNow - startTime).Milliseconds} ms)");
    }

    private static void InjectDependencies()
    {
        var startTime = DateTime.UtcNow;
        InjectDependencies(_temporaryComponentList);
        Console.WriteLine(
            $"Injected {_dependenciesInjected} dependencies. (Time: {(DateTime.UtcNow - startTime).Milliseconds} ms)");
    }
    
    private static void Initialize()
    {
        var startTime = DateTime.UtcNow;
        Console.WriteLine("=> Initializing Components...");

        try
        {
            Initialize(_temporaryComponentList);

            Console.WriteLine(
                $"Initialized {_temporaryComponentList.Count} components! (Time: {(DateTime.UtcNow - startTime).Milliseconds} ms)");
            
            _temporaryComponentList.Clear();
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
        Console.WriteLine("=> Discovering Components...");
        
        var componentTypes = ExecutingAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IComponent))
                        && !t.IsAbstract
                        && !Attribute.IsDefined(t, typeof(IgnoreComponent))
            ).ToList();
        
        var components = new List<object?>(componentTypes.Count);
        
        foreach (var componentType in componentTypes)
        {
            components.Add(ComponentStore.Register(componentType));
            Console.WriteLine($"--> Registered Component of type {componentType.Name}");
        }

        return components;
    }

    private static void InjectDependencies(List<object?> components)
    {
        Console.WriteLine($"=> Injecting Components...");
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
                    $"--Could not find a component to inject into {componentType.Name}.{propertyType.Name}.");
                continue;
            }

            if (!property.CanWrite)
            {
                // In the future I could just change the backing field generated by C#, but let's keep it simple for now.
                Console.WriteLine($"-- Property {componentType.Name}.{propertyType.Name} does not have a 'set' accessor.");
                continue;
            }

            property.SetValue(componentInstance, componentToBeInjected);
            _dependenciesInjected++;
            Console.WriteLine($"--> Injected component {propertyType.Name} into {componentType.Name}.");
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
                Console.WriteLine($"-- Could not find a component to inject into {componentType.Name}.{fieldType.Name}.");
                continue;
            }

            field.SetValue(componentInstance, componentToBeInjected);
            _dependenciesInjected++;
            Console.WriteLine($"--> Injected component {fieldType.Name} into {componentType.Name}.");
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