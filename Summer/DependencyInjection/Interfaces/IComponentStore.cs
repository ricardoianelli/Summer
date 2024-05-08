namespace Summer.DependencyInjection.Interfaces;

/// <summary>
/// Stores Component instances
/// </summary>
public interface IComponentStore
{
    /// <summary>
    /// Returns a singleton instance of a component.
    /// </summary>
    /// <typeparam name="T">Type of the component you're looking for. Must implement the IComponent interface.</typeparam>
    /// <returns>A singleton instance of the specified type or Null in case it can't find it.</returns>
    T? Find<T>() where T : class, IComponent;

    /// <summary>
    /// Returns a reference to an object that can be casted to the type of the component you wanted.
    /// </summary>
    /// <param name="type">Type of the component you're looking for. Must implement the IComponent interface.</param>
    /// <returns></returns>
    object? Find(Type type);
        
    /// <summary>
    /// Creates a Singleton instance of a component. It must contain a parameterless constructor.
    /// </summary>
    /// <typeparam name="T">Type of the component you want to create. Must implement the IComponent interface.</typeparam>
    void Register<T>() where T : class, IComponent, new();
        
    /// <summary>
    /// Creates a Singleton instance of a component. It must contain a parameterless constructor.
    /// </summary>
    /// <param name="type">Type of the component you want to create. Must implement the IComponent interface.</param>
    void Register(Type type);
}