using SimplifyDebtsAlgorithm;

Console.WriteLine($"It has been ... since .NET 9 was released.");



var graph = new Graph<int>();

// Utworzenie nodów (wartości to id node'ów)
var fred = new Node<int>(0);
var gabe = new Node<int>(1);
var alice = new Node<int>(2); // isolated in the picture
var bob = new Node<int>(3);
var charlie = new Node<int>(4);
var david = new Node<int>(5);
var ema = new Node<int>(6);

// Dodajemy nod'y do graphu
graph.AddNode(fred);
graph.AddNode(gabe);
graph.AddNode(alice);
graph.AddNode(bob);
graph.AddNode(charlie);
graph.AddNode(david);
graph.AddNode(ema);

// Mapowanie id -> nazwa (dla czytelności podczas debugowania)
var names = new Dictionary<int, string>
{
    [fred.Value] = "Fred",
    [gabe.Value] = "Gabe",
    [alice.Value] = "Alice",
    [bob.Value] = "Bob",
    [charlie.Value] = "Charlie",
    [david.Value] = "David",
    [ema.Value] = "Ema",
};

// Dodajemy krawędzie zgodnie z rysunkiem (wartości w dolarach jako decimal)
// Gabe -> Bob $30 (pionowo)
graph.AddEdge(gabe, bob, 30m);
// Gabe -> Bob (diagonalnie) $10 (na rysunku są dwie krawędzie prowadzące od Gabe)
graph.AddEdge(gabe, bob, 10m);
// Gabe -> David $10 (diagonalnie)
graph.AddEdge(gabe, david, 10m);

// Fred -> Charlie $30 (pionowo)
graph.AddEdge(fred, charlie, 30m);
// Fred -> Bob $10 (lewy skos)
graph.AddEdge(fred, bob, 10m);
// Fred -> David $10 (prawy skos)
graph.AddEdge(fred, david, 10m);
// Fred -> Ema $10 (prawy najbardziej)
graph.AddEdge(fred, ema, 10m);

// Bob -> Charlie $40
graph.AddEdge(bob, charlie, 40m);
// Charlie -> David $20
graph.AddEdge(charlie, david, 20m);
// David -> Ema $50 (strzałka w górę do Emy)
graph.AddEdge(david, ema, 50m);

// Wyświetlenie struktury (opcjonalne)
Console.WriteLine("Graph nodes and edges:");
for (int i = 0; i < graph.NodesCount; i++)
{
    var node = graph.Nodes[i].Node;
    var displayName = names.ContainsKey(node.Value) ? names[node.Value] : node.Value.ToString();
    Console.WriteLine($"Node {i} -> {displayName}");
    foreach (var e in graph.Nodes[i].Edges)
    {
        var toName = names.ContainsKey(e.EndNode.Value) ? names[e.EndNode.Value] : e.EndNode.Value.ToString();
        Console.WriteLine($"  -> {toName} : ${e.Capacity}");
    }
    Console.WriteLine();
}

Console.WriteLine("Let's start algorithm :)");
var source = 3;
var sink = 6;
var dinicsAlgorithm = new DinicsAlgorithm(graph, source, sink);

var maxFlow = 0m;

while (dinicsAlgorithm.PerformBFS())
{
    var next = new int[graph.NodesCount];
    Array.Fill(next, 0);

    var result = dinicsAlgorithm.PerformDFS(source, [.. next], Decimal.MaxValue);
    while (result != 0)
    {
        maxFlow += result;
        result = dinicsAlgorithm.PerformDFS(source, [.. next], Decimal.MaxValue);
    }
}

//Coś z MinCut???

