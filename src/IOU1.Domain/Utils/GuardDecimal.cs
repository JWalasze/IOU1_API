namespace IOU1.Domain.Utils;

public static class GuardDecimal
{
    public static void ForPositive<TException>(decimal value, string errorMessage)
        where TException : Exception
    {
        if (value <= 0)
            throw Guard.CreateException<TException>(errorMessage);
    }
}
