using FluentAssertions;
using Summer.CommandQueues;

namespace SummerTests.CommandQueues.Commands;

public class AnonymousCommandTests
{
    [Fact]
    public void AnonymousCommand_GivenOnExecuteAndOnUndoLambdas_ShouldWorkProperly()
    {
        var x = 0;

        var anonymousCommand = new AnonymousCommand(
    () =>
            {
                x += 3;
            },
    () =>
            {
                x -= 2;
            }
        );
        
        
        anonymousCommand.Execute();
        x.Should().Be(3);
        
        anonymousCommand.Undo();
        x.Should().Be(1);
    }
}