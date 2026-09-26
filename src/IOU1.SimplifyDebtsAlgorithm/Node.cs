namespace SimplifyDebtsAlgorithm;

public record Node<T>(T Value)
{
    public T Value { get; init; } = Value;
}
