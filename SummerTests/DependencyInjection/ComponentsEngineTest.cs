using System.Reflection;
using FluentAssertions;
using Summer.AsyncEvents;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Attributes;
using Summer.DependencyInjection.Interfaces;

namespace SummerTests.DependencyInjection;

public class ComponentsEngineTest
{
    public ComponentsEngineTest()
    {
        ComponentsEngine.ExecutingAssembly = Assembly.GetExecutingAssembly();
    }
    
    [Fact]
    public void Start_GivenTwoComponentsWithAnInjection_ShouldInstantiateAndInjectCorrectly()
    {
        ComponentsEngine.Start();
        var component1 = ComponentsEngine.GetComponent<Component1>();
        component1.Should().NotBeNull();
        component1?.Component2.Should().NotBeNull();
    }
    
    [Fact]
    public void Start_GivenTwoComponentsWithAnPrivateFieldInjection_ShouldInstantiateAndInjectCorrectly()
    {
        ComponentsEngine.Start();
        var component3 = ComponentsEngine.GetComponent<Component3>();
        component3.Should().NotBeNull();
        var component1 = component3?.GetComponent1();
        component1.Should().NotBeNull();
        component1?.Component2.Should().NotBeNull();
    }
    
    [Fact]
    public void Start_GivenAnInjectionOfPropertyWithNoSet_ShouldNotInject()
    {
        ComponentsEngine.Start();
        var component4 = ComponentsEngine.GetComponent<Component4>();
        component4.Should().NotBeNull();
        var component2 = component4?.Component2;
        component2.Should().BeNull();
    }
    
    [Fact]
    public void Start_GivenAnInjectionOfUnregisteredComponent_ShouldNotInject()
    {
        ComponentsEngine.Start();
        var component5 = ComponentsEngine.GetComponent<Component5>();
        component5.Should().NotBeNull();
        var component6 = component5?.Component6;
        component6.Should().BeNull();
    }
    
    private class Component1 : IComponent
    {
        [Inject]
        public Component2 Component2;
        
    }
    
    private class Component2 : IComponent
    {
    }
    
    private class Component3 : IComponent
    {
        [Inject]
        private Component1 _component1;

        public Component1 GetComponent1() => _component1;
    }
    
    private class Component4 : IComponent
    {
        [Inject]
        public Component2 Component2 { get; }
    }
    
    private class Component5 : IComponent
    {
        [Inject]
        public Component6 Component6 { get; }
    }
    
    [IgnoreComponent]
    private class Component6 : IComponent
    {
    }
}