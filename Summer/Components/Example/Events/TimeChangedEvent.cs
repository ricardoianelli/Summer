using Summer.AsyncEvents.Interfaces;

namespace Summer.Components.Example.Events;

public record TimeChangedEvent(ClockTime Time) : IAsyncEvent;