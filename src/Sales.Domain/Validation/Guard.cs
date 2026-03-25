namespace Sales.Domain.Validation;

public static class Guard
{
    public static void AgainstEmptyGuid(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException($"'{parameterName}' cannot be an empty GUID.", parameterName);
        }
    }
    public static void AgainstNull<T>(T value, string parameterName) where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, $"'{parameterName}' cannot be null.");
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
