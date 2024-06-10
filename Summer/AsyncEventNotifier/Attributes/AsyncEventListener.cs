namespace Summer.AsyncEvents.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AsyncEventListener : Attribute
{
    public readonly Type EventType;

    public AsyncEventListener(Type eventType)
    {
        EventType = eventType;
    }
}