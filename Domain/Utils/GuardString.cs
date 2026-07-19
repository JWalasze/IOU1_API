namespace IOU1.Domain.Utils;

public static class GuardString
{
    public static void ForPresence<TException>(string value, string errorMessage)
        where TException : Exception
    {
        if (string.IsNullOrWhiteSpace(value))
            throw Guard.CreateException<TException>(errorMessage);
    }

    public static void ForMaxlength<TException>(string value, int maxLength, string errorMessage)
        where TException : Exception
    {
        ForPresence<TException>(value, errorMessage);

        if (value.Length > maxLength)
            throw Guard.CreateException<TException>(errorMessage);
    }

    public static void ForMinLength<TException>(string value, int minLength, string errorMessage)
        where TException : Exception
    {
        ForPresence<TException>(value, errorMessage);

        if (value.Length < minLength)
            throw Guard.CreateException<TException>(errorMessage);
    }
}
