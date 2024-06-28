
using Summer.Events.Interfaces;

namespace ConsoleExample.Components.Example.Events;

public record TimeChangedEvent(ClockTime Time) : IEvent;