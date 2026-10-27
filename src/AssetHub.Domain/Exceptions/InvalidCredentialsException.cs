namespace AssetHub.Domain.Exceptions;

//Uses one message for every failture (AUTH-01)
public sealed class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException(string mesage = "The email address or password is incorrect.") : base(mesage)
    {
    }

    public override string ErrorCode => "unauthenticated";
}
