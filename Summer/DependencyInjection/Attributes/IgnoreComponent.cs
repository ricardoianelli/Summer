namespace Summer.DependencyInjection.Attributes;

/// <summary>
/// Attribute used on components that shouldn't be registered with the ComponentsEngine. Used mainly for testing.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreComponent : Attribute
{
}