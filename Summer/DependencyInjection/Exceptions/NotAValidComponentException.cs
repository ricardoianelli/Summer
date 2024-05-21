namespace Summer.DependencyInjection.Exceptions
{
    public class NotAValidComponentException : Exception
    {
        public NotAValidComponentException(string message) : base(message)
        {
        }
    }
}