namespace IOU1.Domain.Utils.Guards;

public static class DecimalGuard
{
    public static void ForPositive<TException>(decimal value, string errorMessage)
        where TException : Exception
    {
        if (value <= 0)
            throw Guard.CreateException<TException>(errorMessage);
    }
}
