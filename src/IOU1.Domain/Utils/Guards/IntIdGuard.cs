namespace IOU1.Domain.Utils.Guards;

public static class IntIdGuard
{
    public static void ForPositivr<TException>(int id, string errorMessage)
        where TException : Exception
    {
        if (id <= 0)
            throw Guard.CreateException<TException>(errorMessage);
    }
}
