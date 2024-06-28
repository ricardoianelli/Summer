using System.Reflection;
using FluentAssertions;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Interfaces;
using Summer.Events;
using Summer.Events.Attributes;
using Summer.Events.Interfaces;

namespace SummerTests.AsyncEventNotifier;

// This needs to be passed so that we execute tests synchronously.
// If we don't, since we're dealing with ComponentEngine which is static,
// the tests between two different files would override themselves and fail.
[Collection("ComponentsEngineTests")]
public class EventNotifierTests
{
    public EventNotifierTests()
    {
        ComponentsEngine.Start(Assembly.GetExecutingAssembly());
    }
    
    [Fact]
    public async Task NotifyAsync_GivenAnEventWithAttributeSubscription_ShouldNotifyCorrectly()
    {
        var component1 = ComponentsEngine.GetComponent<Component1>();
        component1.Should().NotBeNull();
        component1.number.Should().Be(-1);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent(randomNum);
        await EventNotifier.NotifyAsync(randomNumEvent);
        component1.number.Should().Be(randomNum);
    }
    
    [Fact]
    public async Task NotifyAsync_GivenAManualSubscription_ShouldNotifyCorrectly()
    {
        var component2 = ComponentsEngine.GetComponent<Component2>();
        component2.Should().NotBeNull();
        component2.number.Should().Be(-1);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent2(randomNum);
        
        EventNotifier.Subscribe<RandomNumEvent2>(async randomNumEvent2 =>
        {
            component2.number = randomNumEvent2.Number;
        });
        
        await EventNotifier.NotifyAsync(randomNumEvent);
        component2.number.Should().Be(randomNum);
    }
    
    [Fact]
    public void Notify_GivenAnEventWithAttributeSubscriptionNotIgnoringAsync_ShouldNotifyCorrectly()
    {
        var component3 = ComponentsEngine.GetComponent<Component3>();
        component3.Should().NotBeNull();
        component3.number.Should().Be(0);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent3(randomNum);
        EventNotifier.Notify(randomNumEvent);
        component3.number.Should().Be(randomNum*2);
    }
    
    [Fact]
    public void Notify_GivenAnEventWithAttributeSubscriptionIgnoringAsync_ShouldNotifyCorrectly()
    {
        var component4 = ComponentsEngine.GetComponent<Component4>();
        component4.Should().NotBeNull();
        component4.number.Should().Be(0);

        var randomNum = new Random().Next(0, 999);
        var randomNumEvent = new RandomNumEvent4(randomNum);
        EventNotifier.Notify(randomNumEvent, true);
        component4.number.Should().Be(randomNum);
    }
    
    private record RandomNumEvent(int Number) : IEvent;
    private class Component1 : IComponent
    {
        public int number = -1;
        
        [EventListener(typeof(RandomNumEvent))]
        public async Task OnRandomNumber(RandomNumEvent randomNumEvent)
        {
            number = randomNumEvent.Number;
        }
    }
    
    private record RandomNumEvent2(int Number) : IEvent;
    private class Component2 : IComponent
    {
        public int number = -1;
    }
    
    private record RandomNumEvent3(int Number) : IEvent;
    private class Component3 : IComponent
    {
        public int number = 0;
        
        [EventListener(typeof(RandomNumEvent3))]
        public async Task OnRandomNumberAsync(RandomNumEvent3 randomNumEvent)
        {
            number += randomNumEvent.Number;
        }
        
        [EventListener(typeof(RandomNumEvent3))]
        public void OnRandomNumber(RandomNumEvent3 randomNumEvent)
        {
            number += randomNumEvent.Number;
        }
    }
    
    private record RandomNumEvent4(int Number) : IEvent;
    private class Component4 : IComponent
    {
        public int number = 0;
        
        [EventListener(typeof(RandomNumEvent4))]
        public async Task OnRandomNumberAsync(RandomNumEvent4 randomNumEvent)
        {
            number += randomNumEvent.Number;
        }
        
        [EventListener(typeof(RandomNumEvent4))]
        public void OnRandomNumber(RandomNumEvent4 randomNumEvent)
        {
            number += randomNumEvent.Number;
        }
    }
}