using FluentAssertions;
using Summer.DependencyInjection;
using Summer.DependencyInjection.Exceptions;

namespace SummerTests.DependencyInjection;

/*
 * Some tests are testing the same thing, like:
 * GenericFind_GivenAPreviouslyAddedComponent_ShouldReturnSingleton
 * and
 * GenericRegister_GivenAValidComponent_ShouldBeAbleToRetrieveItAfterwards
 * But I'll just leave it like this because they're two different USE CASES, even though the code tested is the same.
 */
public class ComponentStoreTests
{
    [Fact]
    public void GenericFind_GivenAPreviouslyAddedComponent_ShouldReturnComponent()
    {
        var componentStore = new ComponentStore();
        componentStore.Register<ExampleComponent>();
        
        var component = componentStore.Find<ExampleComponent>();
        component.Should().BeOfType<ExampleComponent>();
    }
    
    [Fact]
    public void GenericFind_GivenAnInvalidComponent_ShouldReturnNull()
    {
        var componentStore = new ComponentStore();
        componentStore.Find<ExampleComponent>().Should().BeNull();
    }
    
    [Fact]
    public void Find_GivenAPreviouslyAddedComponent_ShouldReturnComponent()
    {
        var componentStore = new ComponentStore();
        componentStore.Register(typeof(ExampleComponent));
        componentStore.Find(typeof(ExampleComponent)).Should().BeOfType<ExampleComponent>();
    }
    
    [Fact]
    public void Find_GivenAnInvalidComponent_ShouldReturnNull()
    {
        var componentStore = new ComponentStore();
        componentStore.Find(typeof(ExampleComponent)).Should().BeNull();
    }
    
    [Fact]
    public void GenericRegister_GivenAValidComponent_ShouldNotThrow()
    {
        var componentStore = new ComponentStore();
        AssertExtensions.DoesNotThrow(() => componentStore.Register<ExampleComponent>());
    }
    
    [Fact] //TODO: Still in doubt about the behavior I want in this case.
    public void GenericRegister_GivenAValidComponentThatAlreadyExists_ShouldNotThrow()
    {
        var componentStore = new ComponentStore();
        componentStore.Register<ExampleComponent>();
        AssertExtensions.DoesNotThrow(() => componentStore.Register<ExampleComponent>());
    }
    
    [Fact]
    public void GenericRegister_GivenAValidComponent_ShouldBeAbleToRetrieveItAfterwards()
    {
        var componentStore = new ComponentStore();
        componentStore.Register<ExampleComponent>();
        
        var component = componentStore.Find<ExampleComponent>();
        component.Should().BeOfType<ExampleComponent>();
    }
    
    [Fact]
    public void Register_GivenAValidComponent_ShouldNotThrow()
    {
        var componentStore = new ComponentStore();
        AssertExtensions.DoesNotThrow(() => componentStore.Register(typeof(ExampleComponent)));
    }
    
    [Fact] //TODO: Still in doubt about the behavior I want in this case.
    public void Register_GivenAValidComponentThatAlreadyExists_ShouldNotThrow()
    {
        var componentStore = new ComponentStore();
        componentStore.Register(typeof(ExampleComponent));
        AssertExtensions.DoesNotThrow(() => componentStore.Register(typeof(ExampleComponent)));
    }
    
    [Fact]
    public void Register_GivenAValidComponent_ShouldBeAbleToRetrieveItAfterwards()
    {
        var componentStore = new ComponentStore();
        componentStore.Register(typeof(ExampleComponent));
        componentStore.Find(typeof(ExampleComponent)).Should().BeOfType<ExampleComponent>();
    }
    
    [Fact]
    public void Register_GivenAnInvalidComponent_ShouldThrow()
    {
        var componentStore = new ComponentStore();
        Assert.Throws<NotAValidComponentException>(() => componentStore.Register(typeof(String)));
    }
}