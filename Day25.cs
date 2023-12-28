using static AdventOfCode2023.Day25.Kargers;

namespace AdventOfCode2023
{
    public static class Day25
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("TwentyFive", "One");

            UndirectedGraph undirectedGraph = GetUndirectedGraph(data);
            List<int> values = new();
            for (var i = 0; i < 5; i++)
            {
                values.Add(MinCut(undirectedGraph, 0));
            }
            return values.OrderByDescending(x => x).First();
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("TwentyFive", "Two");
            int currentMax = 0;
            return currentMax;
        }

        private static UndirectedGraph GetUndirectedGraph(string[] data)
        {
            UndirectedGraph undirectedGraphObject = new()
            {
                Vertices = new()
            };
            foreach (var line in data)
            {
                var split = line.Split(": ");
                string srcVerticeName = split[0];
                foreach (var destVerticeName in split[1].Split(" "))
                {
                    var srcVertice = undirectedGraphObject.Vertices.FirstOrDefault(x => x.Name == srcVerticeName);
                    if (srcVertice == null)
                    {
                        srcVertice = new Vertex(srcVerticeName);
                        undirectedGraphObject.Vertices.Add(srcVertice);
                    }
                    if (!srcVertice.Edges.Contains(destVerticeName))
                    {
                        srcVertice.Edges.Add(destVerticeName);
                    }

                    var destVertice = undirectedGraphObject.Vertices.FirstOrDefault(x => x.Name == destVerticeName);
                    if (destVertice == null)
                    {
                        destVertice = new Vertex(destVerticeName);
                        undirectedGraphObject.Vertices.Add(destVertice);
                    }
                    if (!destVertice.Edges.Contains(srcVerticeName))
                    {
                        destVertice.Edges.Add(srcVerticeName);
                    }
                }
            }

            return undirectedGraphObject;
        }

        public class Kargers
        {
            public static int MinCut(UndirectedGraph originalGraph, int samplingCount = 0)
            {
                if (originalGraph is null)
                {
                    return default;
                }

                var repeatCount = samplingCount;

                if (repeatCount == 0)
                {
                    var numberOfVertices = originalGraph.Vertices.Count;
                    repeatCount = (int)((2 ^ numberOfVertices) * Math.Log(numberOfVertices));
                }

                var minCuts = new List<int>(samplingCount);

                while (repeatCount > 0)
                {
                    List<Vertex> vertices = new();
                    foreach (var vertex in originalGraph.Vertices)
                    {
                        vertices.Add(new Vertex(vertex.Name)
                        {
                            Edges = vertex.Edges.ToList()
                        });
                    }

                    while (vertices.Count > 2)
                    {
                        var (from, to) = GetRandomEdge(vertices);

                        var fromVertex = vertices.Single(x => x.Name == from);
                        var toVertex = vertices.Single(x => x.Name == to);

                        fromVertex.MergedVertices.Add(toVertex.Name);
                        fromVertex.MergedVertices.AddRange(toVertex.MergedVertices);

                        for (var i = 0; i < toVertex.Edges.Count; i++)
                        {
                            var vertex = toVertex.Edges[i];

                            if (vertex != from)
                            {
                                fromVertex.Edges.Add(vertex);
                            }

                            var adjacentVertex = vertices.Single(x => x.Name == vertex);
                            adjacentVertex.Edges.Remove(to);

                            if (vertex != from)
                            {
                                adjacentVertex.Edges.Add(from);
                            }
                        }
                        if (!vertices.Remove(toVertex))
                        {
                            throw new Exception();
                        }
                    }

                    if (vertices.First().Edges.Count == 3)
                    {
                        return (vertices.First().MergedVertices.Count + 1) * (vertices.Last().MergedVertices.Count + 1);
                    }
                    minCuts.Add(vertices.First().Edges.Count);
                    repeatCount--;
                }

                return minCuts.Min();
            }

            private static (string from, string to) GetRandomEdge(List<Vertex> vertices)
            {
                var r = new Random();
                var from = "";
                var to = "";

                while (from == to)
                {
                    int fromIndex = r.Next(vertices.Count);
                    Vertex fromVertex = vertices[fromIndex];
                    from = fromVertex.Name;
                    int toIndex = r.Next(fromVertex.Edges.Count);
                    to = fromVertex.Edges[toIndex];
                }
                return (from, to);
            }

            public class Vertex
            {
                public Vertex(string name)
                {
                    Name = name;
                }
                public string Name { get; set; }
                public List<string> Edges { get; set; } = new();
                public List<string> MergedVertices { get; set; } = new();
            }

            public class UndirectedGraph
            {
                public List<Vertex> Vertices { get; set; } = new();
            }
        }
    }
}
