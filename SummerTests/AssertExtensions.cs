namespace SummerTests;

public static class AssertExtensions
{
    public static void DoesNotThrow(Action testCode)
    {
        try
        {
            testCode.Invoke();
        }
        catch (Exception e)
        {
            Assert.Fail($"Should not have thrown exception, but did: {e}");
        }
    }
    
    public static void DoesNotThrow<T>(Action testCode) where T : Exception
    {
        try
        {
            testCode.Invoke();
        }
        catch (T e)
        {
            Assert.Fail($"Should not have thrown exception of type {typeof(T).Name}, but did: {e}.");
        }
    }
}