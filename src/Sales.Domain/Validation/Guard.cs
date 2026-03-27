using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Validation;

public static class Guard
{
    public static void AgainstEmptyGuid(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException($"'{parameterName}' cannot be an empty GUID.");
        }
    }
    public static void AgainstNull<T>(T value, string parameterName) where T : class
    {
        if (value == null)
        {
            throw new DomainException($"'{parameterName}' cannot be null.");
        }
    }
    public static void AgainstNull<T>(T value, string parameterName, string message) where T : class
    {
        if (value == null)
        {
            throw new DomainException(message);
        }
    }
    public static void AgainstNullOrWhitespace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"'{parameterName}' cannot be null or whitespace.");
        }
    }
    public static void Against<TException>(bool condition, string message) where TException : Exception
    {
        if (condition)
        {
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
        }
    }
}
