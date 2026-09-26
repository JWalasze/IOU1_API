namespace IOU1.Domain.Exceptions;

public class UserSessionException(string errorMessage) : Exception(errorMessage);
