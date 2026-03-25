namespace Sales.Domain.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public static void When(bool condition, string message)
    {
        if (condition)
        {
            throw new DomainException(message);
        }
    }

}
