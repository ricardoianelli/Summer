using System.Reflection;
using FluentAssertions;
using Summer.AsyncEventNotifier;
using Summer.AsyncEvents.Attributes;
using Summer.AsyncEvents.Interfaces;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Interfaces;

namespace SummerTests.AsyncEventNotifier;

// This needs to be passed so that we execute tests synchronously.
// If we don't, since we're dealing with ComponentEngine which is static,
// the tests between two different files would override themselves and fail.
[Collection("ComponentsEngineTests")]
public class EventNotifierTests
{
    public EventNotifierTests()
    {
        ComponentsEngine.ExecutingAssembly = Assembly.GetExecutingAssembly();
    }
    
    [Fact]
    public async Task Notify_GivenAnEventWithAttributeSubscription_ShouldNotifyCorrectly()
    {
        ComponentsEngine.Start();
        var component1 = ComponentsEngine.GetComponent<Component1>();
        component1.Should().NotBeNull();
        component1.number.Should().Be(-1);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent(randomNum);
        await EventNotifier.Notify(randomNumEvent);
        component1.number.Should().Be(randomNum);
    }
    
    [Fact]
    public async Task Notify_GivenAManualSubscription_ShouldNotifyCorrectly()
    {
        ComponentsEngine.Start();
        var component2 = ComponentsEngine.GetComponent<Component2>();
        component2.Should().NotBeNull();
        component2.number.Should().Be(-1);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent2(randomNum);
        
        EventNotifier.Subscribe<RandomNumEvent2>(async randomNumEvent2 =>
        {
            component2.number = randomNumEvent2.Number;
        });
        
        await EventNotifier.Notify(randomNumEvent);
        component2.number.Should().Be(randomNum);
    }
    
    private record RandomNumEvent(int Number) : IAsyncEvent;
    private class Component1 : IComponent
    {
        public int number = -1;
        
        [AsyncEventListener(typeof(RandomNumEvent))]
        public async Task OnRandomNumber(RandomNumEvent randomNumEvent)
        {
            number = randomNumEvent.Number;
        }
    }
    
    private record RandomNumEvent2(int Number) : IAsyncEvent;
    private class Component2 : IComponent
    {
        public int number = -1;
    }
}