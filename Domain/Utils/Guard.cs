namespace IOU1.Domain.Utils;

public static class Guard
{
    public static T CreateException<T>(string errorMessage) where T : Exception
        => (T)Activator.CreateInstance(typeof(T), errorMessage)!;
}
