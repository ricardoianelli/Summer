using Summer.AsyncEvents.Interfaces;

namespace Summer.Components.Example.Events;

public record AlarmEvent(ClockTime AlarmTime) : IAsyncEvent;