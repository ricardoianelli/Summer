using FluentAssertions;
using Summer.CommandQueues;

namespace SummerTests.CommandQueues;

public class CommandQueueTests
{
    [Fact]
    public void CommandQueue_ShouldInitializeWithStoppedState()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.State.Should().Be(CommandQueueState.Stopped);
    }

    [Fact]
    public void SetState_GivenNewState_ShouldChangeState()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.SetState(CommandQueueState.Paused);
        queue.State.Should().Be(CommandQueueState.Paused);
    }

    [Fact]
    public void SetPoolingDelay_GivenNewDelay_ShouldChangeDelay()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        var newDelay = new Random().Next(0, 5000);
        
        queue.SetPoolingDelay(newDelay);
        queue.GetPoolingDelay().Should().Be(newDelay);
    }

    [Fact]
    public void Enqueue_GivenCommand_ShouldAddCommandToQueue()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        var command = new MockCommand();
        
        var lastCommand = queue.GetNext();
        queue.Enqueue(command);
        queue.GetNext().Should().NotBe(lastCommand);
    }

    [Fact]
    public void Start_GivenStoppedState_ShouldChangeStateToStarted()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.Start();
        queue.State.Should().Be(CommandQueueState.Started);
    }
    
    [Fact]
    public void Start_GivenPausedState_ShouldChangeStateToStarted()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.SetState(CommandQueueState.Paused);
        queue.Start();
        queue.State.Should().Be(CommandQueueState.Started);
    }
    
    [Fact]
    public async Task ProcessQueue_GivingStartAndStopMultipleTimes_ShouldOnlyRunOneInstanceOfMethod()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.Start();
        queue.Enqueue(new MockCommand());
        queue.Enqueue(new MockCommand());
        queue.Enqueue(new MockCommand());
        await Task.Delay(queue.GetPoolingDelay() /4);
        queue.Stop();
        
        queue.Start();
        queue.Enqueue(new AnonymousCommand(() => { }));
        queue.Enqueue(new AnonymousCommand(() => { }));
        queue.Enqueue(new AnonymousCommand(() => { }));
        await Task.Delay(queue.GetPoolingDelay() /4);
        queue.Stop();
        
        queue.Start();
        queue.Enqueue(new MockCommand());
        await Task.Delay(queue.GetPoolingDelay() * 2);

        var history = queue.GetHistory();
        history.Count.Should().Be(3);
        history[0].Should().BeOfType(typeof(MockCommand));
        history[1].Should().BeOfType(typeof(AnonymousCommand));
        history[2].Should().BeOfType(typeof(MockCommand));
    }

    [Fact]
    public void Stop_GivenStartedState_ShouldChangeStateToStoppedAndClearQueue()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.Start();
        queue.Enqueue(new MockCommand());
        queue.Count().Should().Be(1);
        queue.Stop();
        queue.State.Should().Be(CommandQueueState.Stopped);
        queue.Count().Should().Be(0);
    }

    [Fact]
    public void Pause_GivenStartedState_ShouldChangeStateToPaused()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        queue.Start();
        queue.Enqueue(new MockCommand());
        queue.Count().Should().Be(1);
        queue.Pause();
        queue.State.Should().Be(CommandQueueState.Paused);
        queue.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetFromHistory_GivenValidIndex_ShouldReturnCommandFromHistory()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        var firstCommand = new MockCommand();
        var secondCommand = new AnonymousCommand(() => { });
        queue.Enqueue(firstCommand);
        queue.Enqueue(secondCommand);
        queue.Start();

        await Task.Delay(queue.GetPoolingDelay()*5);
        
        queue.GetFromHistory(0).Should().Be(firstCommand);
        queue.GetFromHistory(1).Should().Be(secondCommand);
    }

    [Fact]
    public void GetFromHistory_GivenInvalidIndex_ShouldThrowException()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        Assert.Throws<Exception>(() => queue.GetFromHistory(0));
    }

    [Fact]
    public async Task GetHistory_GivenExecutedCommands_ShouldReturnAllCommandsFromHistory()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        var command = new MockCommand();
        queue.Enqueue(command);
        queue.Start();

        await Task.Delay(queue.GetPoolingDelay()*2);

        var history = queue.GetHistory();
        history.Count.Should().Be(1);
        history[0].Should().Be(command);
    }

    [Fact]
    public async Task CommandExecution_GivenStartedQueue_ShouldExecuteCommandsInQueue()
    {
        CommandQueue queue = new CommandQueue(keepHistory: true);
        var command = new MockCommand();
        queue.Enqueue(command);
        queue.Start();

        await Task.Delay(queue.GetPoolingDelay()*2);

        command.Executed.Should().BeTrue();
    }
}