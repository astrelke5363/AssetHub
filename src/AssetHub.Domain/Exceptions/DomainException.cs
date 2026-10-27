namespace AssetHub.Domain.Exceptions;

//Base class that carries an ErrorCode
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    //Stable, machine-readable code surfaced to clients in the ProblemDetails body
    public abstract string ErrorCode { get; }    
}
