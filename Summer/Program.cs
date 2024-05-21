using Summer.DependencyInjection;

namespace Summer;

internal class Program
{
    static void Main(string[] args)
    {
        ComponentsEngine.Start();
        Console.WriteLine("Hello, Summer!");
    }
}