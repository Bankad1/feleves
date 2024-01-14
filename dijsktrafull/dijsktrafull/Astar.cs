using System;
using System.Collections.Generic;

public class AStarAlgorithm
{
    private Dictionary<string, Dictionary<string, int>> adjacencyList;

    public AStarAlgorithm(Dictionary<string, Dictionary<string, int>> graph)
    {
        adjacencyList = graph;
    }

    public List<string> FindShortestPath(string start, string end)
    {
        Dictionary<string, int> distances = new Dictionary<string, int>();
        Dictionary<string, string> previous = new Dictionary<string, string>();
        HashSet<string> visited = new HashSet<string>();
        List<string> path = null;

        foreach (var vertex in adjacencyList)
        {
            distances[vertex.Key] = vertex.Key == start ? 0 : int.MaxValue;
        }

        PriorityQueue<(string, int), int> priorityQueue = new PriorityQueue<(string, int), int>();
        priorityQueue.Enqueue((start, 0), 0);

        while (priorityQueue.Count > 0)
        {
            var (current, _) = priorityQueue.Dequeue();

            if (current == end)
            {
                path = new List<string>();
                while (previous.ContainsKey(current))
                {
                    path.Insert(0, current);
                    current = previous[current];
                }
                path.Insert(0, start);
                break;
            }

            if (!visited.Contains(current))
            {
                visited.Add(current);

                foreach (var neighbor in adjacencyList[current])
                {
                    string next = neighbor.Key;
                    int cost = neighbor.Value;
                    int heuristicCost = CalculateHeuristic(current, end); // Heurisztikus becslés hozzáadása
                    int totalCost = distances[current] + cost + heuristicCost;

                    if (totalCost < distances[next])
                    {
                        distances[next] = totalCost;
                        previous[next] = current;
                        priorityQueue.Enqueue((next, totalCost), totalCost);
                    }
                }
            }
        }

        return path;
    }

    private int CalculateHeuristic(string current, string end)
    {
        
        // Euklideszi távolság a csúcsok között

        
        return EuclideanDistance(current, end);
    }

    private int EuclideanDistance(string current, string end)
    {
        

        Dictionary<string, (int x, int y)> nodeCoordinates = new Dictionary<string, (int x, int y)>();

       

        if (nodeCoordinates.ContainsKey(current) && nodeCoordinates.ContainsKey(end))
        {
            int x1 = nodeCoordinates[current].x;
            int y1 = nodeCoordinates[current].y;
            int x2 = nodeCoordinates[end].x;
            int y2 = nodeCoordinates[end].y;

            // Euklideszi távolság kiszámítása
            return (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
        else
        {
            
            return 0;
        }
    }
}