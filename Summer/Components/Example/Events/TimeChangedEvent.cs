using Summer.Events.Interfaces;

namespace Summer.Components.Example.Events;

public record TimeChangedEvent(ClockTime Time) : IEvent;