using FluentAssertions;
using Summer.CommandQueues;

namespace SummerTests.CommandQueues;

public class CommandQueueTests
{
    private readonly CommandQueue _queue;

    public CommandQueueTests()
    {
        _queue = new CommandQueue(keepHistory: true);
    }

    [Fact]
    public void CommandQueue_ShouldInitializeWithStoppedState()
    {
        _queue.State.Should().Be(CommandQueueState.Stopped);
    }

    [Fact]
    public void SetState_GivenNewState_ShouldChangeState()
    {
        _queue.SetState(CommandQueueState.Paused);
        _queue.State.Should().Be(CommandQueueState.Paused);
    }

    [Fact]
    public void SetPoolingDelay_GivenNewDelay_ShouldChangeDelay()
    {
        var newDelay = new Random().Next(0, 5000);
        
        _queue.SetPoolingDelay(newDelay);
        _queue.GetPoolingDelay().Should().Be(newDelay);
    }

    [Fact]
    public void Enqueue_GivenCommand_ShouldAddCommandToQueue()
    {
        var command = new MockCommand();
        
        var lastCommand = _queue.GetNext();
        _queue.Enqueue(command);
        _queue.GetNext().Should().NotBe(lastCommand);
    }

    [Fact]
    public void Start_GivenStoppedState_ShouldChangeStateToStarted()
    {
        _queue.Start();
        _queue.State.Should().Be(CommandQueueState.Started);
    }
    
    [Fact]
    public void Start_GivenPausedState_ShouldChangeStateToStarted()
    {
        _queue.SetState(CommandQueueState.Paused);
        _queue.Start();
        _queue.State.Should().Be(CommandQueueState.Started);
    }
    
    [Fact]
    public async Task ProcessQueue_GivingStartAndStopMultipleTimes_ShouldOnlyRunOneInstanceOfMethod()
    {
        _queue.Start();
        _queue.Enqueue(new MockCommand());
        _queue.Enqueue(new MockCommand());
        _queue.Enqueue(new MockCommand());
        await Task.Delay(_queue.GetPoolingDelay() /4);
        _queue.Stop();
        
        _queue.Start();
        _queue.Enqueue(new AnonymousCommand(() => { }));
        _queue.Enqueue(new AnonymousCommand(() => { }));
        _queue.Enqueue(new AnonymousCommand(() => { }));
        await Task.Delay(_queue.GetPoolingDelay() /4);
        _queue.Stop();
        
        _queue.Start();
        _queue.Enqueue(new MockCommand());
        await Task.Delay(_queue.GetPoolingDelay() * 2);

        var history = _queue.GetHistory();
        history.Count.Should().Be(3);
        history[0].Should().BeOfType(typeof(MockCommand));
        history[1].Should().BeOfType(typeof(AnonymousCommand));
        history[2].Should().BeOfType(typeof(MockCommand));
    }

    [Fact]
    public void Stop_GivenStartedState_ShouldChangeStateToStoppedAndClearQueue()
    {
        _queue.Start();
        _queue.Enqueue(new MockCommand());
        _queue.Count().Should().Be(1);
        _queue.Stop();
        _queue.State.Should().Be(CommandQueueState.Stopped);
        _queue.Count().Should().Be(0);
    }

    [Fact]
    public void Pause_GivenStartedState_ShouldChangeStateToPaused()
    {
        _queue.Start();
        _queue.Enqueue(new MockCommand());
        _queue.Count().Should().Be(1);
        _queue.Pause();
        _queue.State.Should().Be(CommandQueueState.Paused);
        _queue.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetFromHistory_GivenValidIndex_ShouldReturnCommandFromHistory()
    {
        var firstCommand = new MockCommand();
        var secondCommand = new AnonymousCommand(() => { });
        _queue.Enqueue(firstCommand);
        _queue.Enqueue(secondCommand);
        _queue.Start();

        await Task.Delay(_queue.GetPoolingDelay()*4);
        
        _queue.GetFromHistory(0).Should().Be(firstCommand);
        _queue.GetFromHistory(1).Should().Be(secondCommand);
    }

    [Fact]
    public void GetFromHistory_GivenInvalidIndex_ShouldThrowException()
    {
        Assert.Throws<Exception>(() => _queue.GetFromHistory(0));
    }

    [Fact]
    public async Task GetHistory_GivenExecutedCommands_ShouldReturnAllCommandsFromHistory()
    {
        var command = new MockCommand();
        _queue.Enqueue(command);
        _queue.Start();

        await Task.Delay(_queue.GetPoolingDelay()*2);

        var history = _queue.GetHistory();
        history.Count.Should().Be(1);
        history[0].Should().Be(command);
    }

    [Fact]
    public async Task CommandExecution_GivenStartedQueue_ShouldExecuteCommandsInQueue()
    {
        var command = new MockCommand();
        _queue.Enqueue(command);
        _queue.Start();

        await Task.Delay(_queue.GetPoolingDelay()*2);

        command.Executed.Should().BeTrue();
    }
}