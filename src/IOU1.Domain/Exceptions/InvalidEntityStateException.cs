namespace IOU1.Domain.Exceptions;

public sealed class InvalidEntityStateException(string message) : Exception(message);
