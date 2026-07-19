namespace IOU1.Domain.Exceptions;

public class CreatingExpenseCategoryException(string errorMessage) : Exception(errorMessage);

public class DeletingExpensecategoryException(string errorMessage) : Exception(errorMessage);
