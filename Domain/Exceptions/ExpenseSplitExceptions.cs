namespace IOU1.Domain.Exceptions;

public class CreatingExpenseSplitException(string errorMessage) : Exception(errorMessage);

public class DeletingExpenseSplitException(string errorMessage) : Exception(errorMessage);
