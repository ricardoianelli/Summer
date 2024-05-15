using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public class ComponentStore : IComponentStore
{
    private readonly IDictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();
    
    public T? Find<T>() where T : class, IComponent
    {
        if (!_components.ContainsKey(typeof(T))) 
            return null;
        
        return _components[typeof(T)] as T;
    }

    public object? Find(Type type)
    {
        throw new NotImplementedException();
    }

    public void Register<T>() where T : class, IComponent, new()
    {
        if (_components.ContainsKey(typeof(T))) 
            return;
        
        _components.Add(typeof(T), new T());
    }

    public void Register(Type type)
    {
        throw new NotImplementedException();
    }
}