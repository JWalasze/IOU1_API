namespace SimplifyDebtsAlgorithm;

public class Graph<T>
{
    private readonly List<(Node<T> Node, List<Edge<T>> Edges)> _nodes = [];

    public int NodesCount => _nodes.Count;
    public List<(Node<T> Node, List<Edge<T>> Edges)> Nodes => _nodes;

    public Graph() { }

    public Graph(IEnumerable<Node<T>> nodes)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }
    }

    public void AddNode(Node<T> node)
    {
        _nodes.Add((node, new List<Edge<T>>()));
    }

    public void AddEdge(Node<T> startNode, Node<T> endNode, decimal capacity)
    {
        var edge = new Edge<T>(startNode, endNode, capacity, isResidual: false);
        var residualEdge = new Edge<T>(endNode, startNode, 0, isResidual: true);

        var startNodeIndex = _nodes.FindIndex(n => n.Node == startNode);
        if (startNodeIndex < 0)
            throw new Exception($"Couldn't find node with ID: {startNode.Value}");

        var endNodeIndex = _nodes.FindIndex(n => n.Node == endNode);
        if (endNodeIndex < 0)
            throw new Exception($"Couldn't find node with ID: {endNode.Value}");

        _nodes[startNodeIndex].Edges.Add(edge);
        _nodes[endNodeIndex].Edges.Add(residualEdge);
    }

    public int GetEdgesCountFor(Node<T> node)
    {
        return _nodes.FirstOrDefault(n => n.Node == node).Edges.Count;
    }

    public Edge<T> GetEdgeFor(Node<T> node, int edgeNumber)
    {
        var foundNode = _nodes.FirstOrDefault(n => n.Node == node);
        return foundNode.Edges[edgeNumber];
    }

    public void AugmentEdgeWithResidual(Edge<T> edge, decimal value)
    {
        edge.Flow += value;
        var residualEdge = _nodes
            .FirstOrDefault(n => n.Node == edge.EndNode)
            .Edges
            .FirstOrDefault(e => e.IsResidual && e.EndNode == edge.StartNode)
            ?? throw new Exception($"Nie znaleziono krawędzi w DFS: {edge}, {value}");

        residualEdge.Flow -= value;
    }
}
