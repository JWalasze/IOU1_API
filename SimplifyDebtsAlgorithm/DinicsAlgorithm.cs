namespace SimplifyDebtsAlgorithm;

public class DinicsAlgorithm
{
    private readonly int _source;
    private readonly int _sink;

    private Graph<int> _graph;

    private List<int> _levels;

    public DinicsAlgorithm(Graph<int> graph, int source, int sink)
    {
        _graph = graph;
        _source = source;
        _sink = sink;
        _levels = [.. new int[_graph.NodesCount]];


    }

    //Musimy mieć listę odnośnie leveli -> to nam mówi czy dany node był odwiedzony, bo chodzi o to
    //żeby się nie zapętlać i nie iść do Node który nie kieruje nas w stronę celu
    public bool PerformBFS()
    {
        var visitedNodes = new HashSet<int>(_levels.Count);
        var nodesQueue = new Queue<int>();

        nodesQueue.Enqueue(_source);
        while (nodesQueue.Count > 0)
        {
            var currentNode = nodesQueue.Dequeue();
            if (!visitedNodes.Contains(currentNode))
            {
                Console.WriteLine($"Nieodwiedzony node: {currentNode}");
                visitedNodes.Add(currentNode);

                foreach (var niegbour in _graph.Nodes[currentNode].Edges)
                {
                    Console.WriteLine($"Sąsiad node: {niegbour.EndNode.Value}");
                    if (!visitedNodes.Contains(niegbour.EndNode.Value) && niegbour.GetRemainingCapacity() > 0)
                    {
                        Console.WriteLine($"Nieodwiedzony sąsiad node: {niegbour.EndNode.Value}");
                        nodesQueue.Enqueue(niegbour.EndNode.Value);
                        _levels[niegbour.EndNode.Value] = _levels[currentNode] + 1;
                    }
                    else
                    {
                        Console.WriteLine($"Odwiedzony sąsiad node: {niegbour.EndNode.Value}. Skipujemy.");
                    }
                }
            }
        }

        //SPrawdzamy czy przy użyciu BFS doszliśmy w ogóle do ostatniego node (sink). Może być tak, że z wybranego node nie
        //ma połączenia do wybranego docelowego node
        return _levels[_sink] != 0;
    }

    //at czyli w którym node obecnie jesteśmy, next to lista kolejnych node do których możemy się udać, flow to ile przepływu możemy jeszcze przepuścić 
    //dla przepływu wyliczamy MIN, bo on nam powie ile możemy przepuścić przez tą ścieżkę.
    public decimal PerformDFS(int at, List<int> next, decimal flow)
    {
        //Jeśli doszliśmy do końca, to zwracamy przepływ, który może pójść przez tą ścieżkę.
        if (at == _sink)
            return flow;

        var node = new Node<int>(at);
        var edgesCount = _graph.GetEdgesCountFor(node);
        while (next[at] > edgesCount)
        {
            var edge = _graph.GetEdgeFor(node, next[at]);
            var remainingCapacity = edge.GetRemainingCapacity();

            //Musimy sprawdzić zgodność z BFS, czyli czy idziemy w kierunku wyznaczonym przez BFS
            if (remainingCapacity > 0 && _levels[edge.EndNode.Value] == _levels[next[at]] + 1)
            {
                //Rekurencyjnie, aż w końcu zwrócimy maksymalny flow na ścieżce (maks flow jaki możemy puścić na ścieżce)
                var bottleNeck = PerformDFS(edge.EndNode.Value, next, Math.Min(remainingCapacity, flow));
                if (bottleNeck > 0)
                {
                    _graph.AugmentEdgeWithResidual(edge, bottleNeck);
                    return bottleNeck;
                }
                //Jeśli jest 0 to znaczy, że nie doszliśmy do końca i mamy ścieżkę 'nasyconą'
            }

            next[at]++;
        }

        return 0;
    }
}
