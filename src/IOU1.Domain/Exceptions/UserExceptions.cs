namespace IOU1.Domain.Exceptions;

public class CreatingUserException(string errorMessage) : Exception(errorMessage);

public class UserNotFoundException : Exception
{
    public override string Message => "User not found!";
}


