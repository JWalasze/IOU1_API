namespace SimplifyDebtsAlgorithm;

public class Edge<T>(
    Node<T> startNode,
    Node<T> endNode,
    decimal maxCapacity,
    bool isResidual)
{
    public Node<T> StartNode { get; set; } = startNode;

    public Node<T> EndNode { get; set; } = endNode;

    //Capacity krawędzi, czyli ile maksymalnie możemy przepuścić przez nią flow
    public decimal Capacity { get; set; } = maxCapacity;

    //Stan faktyczny krawędzi, czyli ile faktycznie przepływa przez nią flow. Na początku jest to 0, bo nie ma jeszcze żadnego przepływu
    public decimal Flow { get; set; } = 0;

    //Czy krawędź jest residualną krawędzią wykorzystywana do uciekania z lokalnego maksimum.
    //Jest to krawędź wsteczna, która zapamiętuje ile flow przepłynęło przez krawędź w przód i pozwala na cofnięcie tego flow, jeśli okaże się, że nie jest to optymalne rozwiązanie.
    public bool IsResidual { get; set; } = isResidual;

    public decimal GetRemainingCapacity()
    {
        return Capacity - Flow;
    }
}
