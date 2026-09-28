namespace IOU1.Domain.Exceptions;

public class CreatingExpenseCategoryException(string errorMessage) : Exception(errorMessage);

public class DeletingExpenseCategoryException(string errorMessage) : Exception(errorMessage);
