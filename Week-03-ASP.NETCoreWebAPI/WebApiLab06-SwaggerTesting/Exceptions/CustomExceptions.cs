namespace WebApiLab06.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceName, int id)
        : base($"{resourceName} with Id={id} was not found.") { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
