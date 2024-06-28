namespace Summer.CommandQueues;

public class AnonymousCommand : ICommand
{
    private Action _onExecute;
    private Action? _onUndo;

    public AnonymousCommand(Action onExecute, Action? onUndo = null)
    {
        _onExecute = onExecute;
        _onUndo = onUndo;
    }

    public void Execute()
    {
        _onExecute.Invoke();
    }

    public void Undo()
    {
        _onUndo?.Invoke();
    }
}