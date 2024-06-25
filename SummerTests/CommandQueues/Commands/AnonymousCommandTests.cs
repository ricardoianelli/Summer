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
            onExecute: () => { x += 3; },
            onUndo: () => { x -= 2; }
        );

        anonymousCommand.Execute();
        x.Should().Be(3);

        anonymousCommand.Undo();
        x.Should().Be(1);
    }
}