namespace IOU1.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public override string Message => "User not found!";
}
