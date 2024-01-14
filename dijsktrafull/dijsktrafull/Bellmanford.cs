using System;
using System.Collections.Generic;

public class BellmanFordAlgorithm
{
    private Dictionary<string, Dictionary<string, int>> adjacencyList;

    public BellmanFordAlgorithm(Dictionary<string, Dictionary<string, int>> graph)
    {
        adjacencyList = graph;
    }

    public List<string> FindShortestPath(string start, string end)
    {
        Dictionary<string, int> distances = new Dictionary<string, int>();
        Dictionary<string, string> previous = new Dictionary<string, string>();
        List<string> path = null;

        foreach (var vertex in adjacencyList)
        {
            distances[vertex.Key] = vertex.Key == start ? 0 : int.MaxValue;
        }

        for (int i = 0; i < adjacencyList.Count - 1; i++)
        {
            foreach (var edge in adjacencyList)
            {
                foreach (var neighbor in edge.Value)
                {
                    if (distances[edge.Key] != int.MaxValue &&
                        distances[edge.Key] + neighbor.Value < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = distances[edge.Key] + neighbor.Value;
                        previous[neighbor.Key] = edge.Key;
                    }
                }
            }
        }

        foreach (var edge in adjacencyList)
        {
            foreach (var neighbor in edge.Value)
            {
                if (distances[edge.Key] != int.MaxValue &&
                    distances[edge.Key] + neighbor.Value < distances[neighbor.Key])
                {
                    Console.WriteLine("A gráf tartalmaz negatív súlyú ciklust.");
                    return new List<string>();
                }
            }
        }

        string smallest = end;
        path = new List<string>();
        while (previous.ContainsKey(smallest))
        {
            path.Insert(0, smallest);
            smallest = previous[smallest];
        }
        path.Insert(0, start);

        return path;
    }
}