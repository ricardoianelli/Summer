using Summer.DependencyInjection.Interfaces;

namespace Summer.DependencyInjection;

public class ComponentStore : IComponentStore
{
    public T? Find<T>() where T : class, IComponent
    {
        throw new NotImplementedException();
    }

    public object? Find(Type type)
    {
        throw new NotImplementedException();
    }

    public void Register<T>() where T : class, IComponent, new()
    {
        throw new NotImplementedException();
    }

    public void Register(Type type)
    {
        throw new NotImplementedException();
    }
}