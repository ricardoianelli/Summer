using Summer.AsyncEventNotifier.Interfaces;

namespace Summer.Components.Example.Events;

public record AlarmEvent(ClockTime AlarmTime) : IEvent;