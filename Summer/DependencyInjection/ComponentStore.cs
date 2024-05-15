using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public class ComponentStore : IComponentStore
{
    private readonly IDictionary<Type, object> _components = new Dictionary<Type, object>();
    
    public T? Find<T>() where T : class, IComponent
    {
        if (!_components.ContainsKey(typeof(T))) 
            return null;
        
        return _components[typeof(T)] as T;
    }

    public object? Find(Type type)
    {
        // I could simplify this with TryGetValue, but I want to make it easier for people who are still learning so they feel empowered.
        if (!_components.ContainsKey(type)) 
            return null;
        
        return _components[type];
    }

    public void Register<T>() where T : class, IComponent, new()
    {
        if (_components.ContainsKey(typeof(T))) 
            return;
        
        _components.Add(typeof(T), new T());
    }

    public void Register(Type type)
    {
        if (_components.ContainsKey(type)) 
            return;
        
        var instance = Activator.CreateInstance(type);
        if (instance is null)
        {
            throw new Exception($"There was an error creating an instance of {type}.");
        }
        
        _components.Add(type, instance);
    }
}