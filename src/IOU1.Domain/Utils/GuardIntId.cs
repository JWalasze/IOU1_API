namespace IOU1.Domain.Utils;

public static class GuardIntId
{
    public static void ForPresence<TException>(int id, string errorMessage)
        where TException : Exception
    {
        if (id <= 0)
            throw Guard.CreateException<TException>(errorMessage);
    }
}
