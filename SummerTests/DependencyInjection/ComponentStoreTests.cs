using FluentAssertions;
using Summer.DependencyInjection;

namespace SummerTests.DependencyInjection;

public class ComponentStoreTests
{
    [Fact]
    public void GenericFind_GivenAValidComponent_ShouldReturnSingleton()
    {
        var componentStore = new ComponentStore();
        componentStore.Find<ExampleComponent>().Should().BeOfType<ExampleComponent>();
    }
    
    [Fact]
    public void GenericFind_GivenAnInvalidComponent_ShouldReturnNull()
    {
    }
    
    [Fact]
    public void Find_GivenAValidComponent_ShouldReturnSingleton()
    {
    }
    
    [Fact]
    public void Find_GivenAnInvalidComponent_ShouldReturnNull()
    {
    }
    
    [Fact]
    public void GenericRegister_GivenAValidComponent_ShouldNotThrow()
    {
    }
    
    [Fact] //TODO: Still in doubt about the behavior I want in this case.
    public void GenericRegister_GivenAValidComponentThatAlreadyExists_ShouldNotThrow()
    {
    }
    
    [Fact]
    public void GenericRegister_GivenAValidComponent_ShouldBeAbleToRetrieveItAfterwards()
    {
    }
    
    [Fact]
    public void GenericRegister_GivenAnInvalidComponent_ShouldThrow()
    {
    }
    
    [Fact]
    public void GenericRegister_GivenAnInvalidComponent_ShouldGetNullWhenRetrievingItAfterwards()
    {
    }
    
    [Fact]
    public void Register_GivenAValidComponent_ShouldNotThrow()
    {
    }
    
    [Fact] //TODO: Still in doubt about the behavior I want in this case.
    public void Register_GivenAValidComponentThatAlreadyExists_ShouldNotThrow()
    {
    }
    
    [Fact]
    public void Register_GivenAValidComponent_ShouldBeAbleToRetrieveItAfterwards()
    {
    }
    
    [Fact]
    public void Register_GivenAnInvalidComponent_ShouldThrow()
    {
    }
    
    [Fact]
    public void Register_GivenAnInvalidComponent_ShouldGetNullWhenRetrievingItAfterwards()
    {
    }
}