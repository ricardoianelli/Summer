using Summer.Events.Interfaces;

namespace ConsoleExample.Components.Example.Events;

public record AlarmEvent(ClockTime AlarmTime) : IEvent;