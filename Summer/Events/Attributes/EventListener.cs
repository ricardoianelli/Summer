﻿namespace Summer.Events.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class EventListener : Attribute
{
    public readonly Type EventType;

    public EventListener(Type eventType)
    {
        EventType = eventType;
    }
}