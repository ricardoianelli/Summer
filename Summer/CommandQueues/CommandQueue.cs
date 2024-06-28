using System.Collections.Concurrent;

namespace Summer.CommandQueues;

/// <summary>
/// Simple Command Queue implementation. You can add commands, start, pause and stop the queue, maintain and access a history of executed commands, change delay, etc.
/// </summary>
public class CommandQueue
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

    /// <summary>
    /// Sets the state of this queue.
    /// </summary>
    /// <param name="state">The state on which you want the queue to go to. Pause will simply pause the queue while Stop will also clean up the queue.</param>
    public void SetState(CommandQueueState state)
    {
        State = state;
    }

    /// <summary>
    /// Change how often you want commands to be executed.
    /// </summary>
    /// <param name="delayInMs">The interval you want between command consumption.</param>
    public void SetPoolingDelay(int delayInMs)
    {
        _poolingDelayInMs = delayInMs;
    }

    /// <summary>
    /// Gets the interval between commands consumption. Basically how often the commands are executed.
    /// </summary>
    /// <returns></returns>
    public int GetPoolingDelay()
    {
        return _poolingDelayInMs;
    }

    /// <summary>
    /// Adds a new command to the queue to be executed.
    /// </summary>
    /// <param name="command">The command you want to add to the queue for consumption.</param>
    public void Enqueue(ICommand command)
    {
        _commandQueue.Enqueue(command);
    }

    /// <summary>
    /// Starts the queue. If it's already started, nothing will happen. If it was stopped or paused, it will start consuming commands.
    /// </summary>
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
    /// <summary>
    /// Stop consuming commands. This will also clean up the queue, but not the commands execution history.
    /// </summary>
    public void Stop()
    {
        State = CommandQueueState.Stopped;
        Clear();
    }

    /// <summary>
    /// Pauses the queue. You can resume by calling 'Start()'.
    /// </summary>
    public void Pause()
    {
        State = CommandQueueState.Paused;
    }

    /// <summary>
    /// Remove every command from the command queue.
    /// </summary>
    public void Clear()
    {
        _commandQueue.Clear();
    }

    /// <summary>
    /// Returns the amount of commands in the queue to be executed.
    /// </summary>
    /// <returns>The amount of commands in the queue to be executed</returns>
    public int Count()
    {
        return _commandQueue.Count;
    }

    /// <summary>
    /// Get an executed command from the history based on an index IF your queue supports it.
    /// </summary>
    /// <param name="index">Index of the executed command. If you want the third command that was ever executed by this queue, your index is 2. (Indexes start at 0)</param>
    /// <returns>A command previously executed by this queue.</returns>
    /// <exception cref="Exception">Throws an exception if the index is not valid.</exception>
    public ICommand GetFromHistory(int index)
    {
        if (index < 0 || index >= GetHistoryCount())
        {
            throw new Exception(
                $"Commands history doesn't have an item at index {index}. Current count: {GetHistoryCount()}");
        }

        return _commandsHistory[index];
    }

    /// <summary>
    /// Gets the full history of executed commands IF your queue supports it.
    /// </summary>
    /// <returns>List containing all the commands executed by this queue.</returns>
    public List<ICommand> GetHistory()
    {
        return new List<ICommand>(_commandsHistory);
    }

    /// <summary>
    /// Gets the next command to be executed by the queue.
    /// </summary>
    /// <param name="remove">If set to true, it will also remove this command from the queue.</param>
    /// <returns>The command, if there was one, or null if the queue was empty.</returns>
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