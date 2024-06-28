using System.Collections.Concurrent;

namespace Summer.CommandQueues;

/// <summary>
/// Simple Command Queue implementation. You can add commands, start, pause and stop the queue, maintain and access a history of executed commands, change delay, etc.
/// </summary>
public class CommandQueue : ICommandQueue
{
    public CommandQueueState State { get; protected set; }

    private int _poolingDelayInMs;
    private Guid _poolingGuid;

    private readonly bool _keepHistory;
    private readonly ConcurrentQueue<ICommand> _commandQueue;
    private readonly List<ICommand> _commandsHistory;

    /// <summary>
    /// Creates a new queue.
    /// </summary>
    /// <param name="poolingDelayInMs">How often you want commands to be executed.</param>
    /// <param name="keepHistory">If you want to keep a history of the executed commands.</param>
    public CommandQueue(int poolingDelayInMs = 100, bool keepHistory = false)
    {
        _poolingGuid = new Guid();
        _commandQueue = new ConcurrentQueue<ICommand>();

        _keepHistory = keepHistory;
        _commandsHistory = new List<ICommand>();
        
        SetState(CommandQueueState.Stopped);
        SetPoolingDelay(poolingDelayInMs);
    }

    public void SetState(CommandQueueState state)
    {
        State = state;
    }

    public void SetPoolingDelay(int delayInMs)
    {
        _poolingDelayInMs = delayInMs;
    }

    public int GetPoolingDelay()
    {
        return _poolingDelayInMs;
    }

    public void Enqueue(ICommand command)
    {
        _commandQueue.Enqueue(command);
    }

    public void Start()
    {
        if (State == CommandQueueState.Started) return;

        if (State == CommandQueueState.Stopped)
        {
            _poolingGuid = new Guid();
        }

        State = CommandQueueState.Started;
        Task.Run(() => ProcessQueue(_poolingGuid));
    }

    //TODO: Decide if I want to also cleanup the history or not. I probably should, but I'm too tired to think right now, my brain is not braining.
    public void Stop()
    {
        State = CommandQueueState.Stopped;
        Clear();
    }

    public void Pause()
    {
        State = CommandQueueState.Paused;
    }

    public void Clear()
    {
        _commandQueue.Clear();
    }

    public int Count()
    {
        return _commandQueue.Count;
    }

    public ICommand GetFromHistory(int index)
    {
        if (index < 0 || index >= GetHistoryCount())
        {
            throw new Exception(
                $"Commands history doesn't have an item at index {index}. Current count: {GetHistoryCount()}");
        }

        return _commandsHistory[index];
    }

    public List<ICommand> GetHistory()
    {
        return new List<ICommand>(_commandsHistory);
    }

    public ICommand? GetNext(bool remove = false)
    {
        ICommand? command;

        if (remove)
        {
            _commandQueue.TryDequeue(out command);
        }
        else
        {
            _commandQueue.TryPeek(out command);
        }

        return command;
    }

    private int GetHistoryCount()
    {
        return _commandsHistory.Count;
    }

    private async Task ProcessQueue(Guid poolingGuid)
    {
        while (State == CommandQueueState.Started && poolingGuid == _poolingGuid)
        {
            var command = GetNext(true);
            if (command is null) continue;

            try
            {
                command.Execute();
                if (_keepHistory)
                {
                    _commandsHistory.Add(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred trying to execute command {command.GetType().Name}: {e}");
            }

            await Task.Delay(_poolingDelayInMs);
        }
    }
}