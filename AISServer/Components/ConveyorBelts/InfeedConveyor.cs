using Summer.DependencyInjection.Interfaces;

namespace AISServer.Components.ConveyorBelts;

public class InfeedConveyor : IComponent
{
    public bool IsInitialized { get; private set; }
    public bool IsMoving { get; private set; }
    
    public void Initialize()
    {
        if (IsInitialized) return;
        
        Console.WriteLine("Initializing InfeedConveyor...");
        IsInitialized = true;
    }

    public void Forward()
    {
        if (!IsInitialized)
        {
            Console.WriteLine("InfeedConveyor needs to be initialized to act.");
            return;
        }

        IsMoving = true;
        Console.WriteLine("Moving InfeedConveyor forward.");
    }

    public void Backward()
    {
        if (!IsInitialized)
        {
            Console.WriteLine("InfeedConveyor needs to be initialized to act.");
            return;
        }

        IsMoving = true;
        Console.WriteLine("Moving InfeedConveyor backward.");
    }

    public void Stop()
    {
        if (!IsMoving) return;
        
        Console.WriteLine("Stopping InfeedConveyor.");
        IsMoving = false;
    }
}