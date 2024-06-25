using Summer.CommandQueues;

namespace SummerTests.CommandQueues;

public class MockCommand : ICommand
{
    public bool Executed { get; private set; }

    public void Execute()
    {
        Executed = true;
    }

    public void Undo()
    {
        Executed = false;
    }
}