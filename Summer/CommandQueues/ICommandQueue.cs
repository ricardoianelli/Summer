namespace Summer.CommandQueues;

public interface ICommandQueue
{
    /// <summary>
    /// Sets the state of this queue.
    /// </summary>
    /// <param name="state">The state on which you want the queue to go to. Pause will simply pause the queue while Stop will also clean up the queue.</param>
    public void SetState(CommandQueueState state);

    /// <summary>
    /// Change how often you want commands to be executed.
    /// </summary>
    /// <param name="delayInMs">The interval you want between command consumption.</param>
    public void SetPoolingDelay(int delayInMs);

    /// <summary>
    /// Gets the interval between commands consumption. Basically how often the commands are executed.
    /// </summary>
    /// <returns></returns>
    public int GetPoolingDelay();

    /// <summary>
    /// Adds a new command to the queue to be executed.
    /// </summary>
    /// <param name="command">The command you want to add to the queue for consumption.</param>
    public void Enqueue(ICommand command);

    /// <summary>
    /// Starts the queue. If it's already started, nothing will happen. If it was stopped or paused, it will start consuming commands.
    /// </summary>
    public void Start();

    //TODO: Decide if I want to also cleanup the history or not. I probably should, but I'm too tired to think right now, my brain is not braining.
    /// <summary>
    /// Stop consuming commands. This will also clean up the queue, but not the commands execution history.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Pauses the queue. You can resume by calling 'Start()'.
    /// </summary>
    public void Pause();

    /// <summary>
    /// Remove every command from the command queue.
    /// </summary>
    public void Clear();

    /// <summary>
    /// Returns the amount of commands in the queue to be executed.
    /// </summary>
    /// <returns>The amount of commands in the queue to be executed</returns>
    public int Count();

    /// <summary>
    /// Get an executed command from the history based on an index IF your queue supports it.
    /// </summary>
    /// <param name="index">Index of the executed command. If you want the third command that was ever executed by this queue, your index is 2. (Indexes start at 0)</param>
    /// <returns>A command previously executed by this queue.</returns>
    /// <exception cref="Exception">Throws an exception if the index is not valid.</exception>
    public ICommand GetFromHistory(int index);

    /// <summary>
    /// Gets the full history of executed commands IF your queue supports it.
    /// </summary>
    /// <returns>List containing all the commands executed by this queue.</returns>
    public List<ICommand> GetHistory();

    /// <summary>
    /// Gets the next command to be executed by the queue.
    /// </summary>
    /// <param name="remove">If set to true, it will also remove this command from the queue.</param>
    /// <returns>The command, if there was one, or null if the queue was empty.</returns>
    public ICommand? GetNext(bool remove = false);
}