using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

public class DirectedGraph
{
    private Dictionary<string, Dictionary<string, int>> adjacencyList;

    public DirectedGraph()
    {
        adjacencyList = new Dictionary<string, Dictionary<string, int>>();
    }

    // Él hozzáadása a gráfhoz
    public void AddDirectedEdge(string source, string destination, int weight)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            adjacencyList[source] = new Dictionary<string, int>();
        }

        if (!adjacencyList.ContainsKey(destination))
        {
            adjacencyList[destination] = new Dictionary<string, int>();
        }

        // Az él hozzáadása az egyik irányba
        adjacencyList[source][destination] = weight;
    }

    // Dijkstra algoritmus implementációja a legrövidebb út megtalálására
    public List<string> CalculateShortestPath(string start, string end)
    {
        Dictionary<string, int> distances = new Dictionary<string, int>(); // Távolságok tárolása
        Dictionary<string, string> previous = new Dictionary<string, string>(); // Előző csúcsok tárolása
        List<string> nodes = new List<string>(); // Csúcsok listája

        List<string> path = null; // Leírás az útvonalnak

        // Csúcsok inicializálása a gráfban
        foreach (var vertex in adjacencyList)
        {
            if (vertex.Key == start)
            {
                distances[vertex.Key] = 0; // Kezdőcsúcs távolsága 0
            }
            else
            {
                distances[vertex.Key] = int.MaxValue; // Egyéb csúcsok távolsága végtelen (maximum érték)
            }

            nodes.Add(vertex.Key); // Csúcsok hozzáadása a listához
        }

        // Dijkstra algoritmus
        while (nodes.Count != 0)
        {
            nodes.Sort((x, y) => distances[x] - distances[y]); // Csúcsok rendezése a távolság alapján

            string smallest = nodes[0]; // Legkisebb távolságú csúcs
            nodes.Remove(smallest);

            // Ha elérjük a célcsúcsot, visszafejtjük az útvonalat
            if (smallest == end)
            {
                path = new List<string>();
                while (previous.ContainsKey(smallest))
                {
                    path.Insert(0, smallest);
                    smallest = previous[smallest];
                }
                break;
            }

            // Ha a kis csúcs távolsága végtelen, kilépünk a ciklusból
            if (distances[smallest] == int.MaxValue)
            {
                break;
            }

            // Szomszédos csúcsok vizsgálata és távolságok frissítése
            foreach (var neighbor in adjacencyList[smallest])
            {
                int alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                }
            }
        }

        // Ha találtunk útvonalat, visszaadjuk azt
        if (path != null)
        {
            path.Insert(0, start);
            path.Add(end);
        }

        return path; // Visszaadjuk az útvonalat
    }
    public List<string> BellmanFord(string start, string end)
    {
        BellmanFordAlgorithm bellmanFordAlgorithm = new BellmanFordAlgorithm(adjacencyList);
        return bellmanFordAlgorithm.FindShortestPath(start, end);
    }

    // A* algoritmus implementációja
    public List<string> AStar(string start, string end)
    {
        AStarAlgorithm aStarAlgorithm = new AStarAlgorithm(adjacencyList);
        return aStarAlgorithm.FindShortestPath(start, end);
    }
}

class Program
{
    static void Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        string fileName = "graphdata3.xlsx"; // Az Excel fájl neve
        string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
        string filePath = Path.Combine(downloadsPath, fileName); // Az Excel fájl teljes elérési útja

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // Az adatok az "Sheet1" nevű lapban vannak

            DirectedGraph directedGraph = new DirectedGraph();

            // Adatok beolvasása az Excel-fájlból és gráf felépítése
            for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Kezdés a második sorról
            {
                string source = worksheet.Cells[row, 1].Text;
                string destination = worksheet.Cells[row, 2].Text;
                int weight = int.Parse(worksheet.Cells[row, 3].Text); // A súly beolvasása

                directedGraph.AddDirectedEdge(source, destination, weight);
            }

            Console.WriteLine("Choose an algorithm to find the shortest path:");
            Console.WriteLine("1: Dijkstra's Algorithm");
            Console.WriteLine("2: Bellman-Ford Algorithm");
            Console.WriteLine("3: A* Algorithm");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                string startPoint = "B"; // Kiindulási pont megadása
                string endPoint = "E"; // Célpont megadása

                List<string> shortestPath = null;

                switch (choice)
                {
                    case 1:
                        // Dijkstra-algoritmus futtatása és legrövidebb út megtalálása
                        shortestPath = directedGraph.CalculateShortestPath(startPoint, endPoint);
                        break;
                    case 2:
                        // Bellman-Ford-algoritmus futtatása és legrövidebb út megtalálása
                        shortestPath = directedGraph.BellmanFord(startPoint, endPoint);
                        break;
                    case 3:
                        // A*-algoritmus futtatása és legrövidebb út megtalálása
                        shortestPath = directedGraph.AStar(startPoint, endPoint);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                        break;
                }

                // Útvonal kiíratása
                if (shortestPath != null)
                {
                    Console.WriteLine("Shortest path from A to H:");
                    foreach (var node in shortestPath)
                    {
                        Console.Write(node + " ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("No path found!");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}
