namespace AdventOfCode2023
{
    public static class Day25
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("TwentyFive", "One");
            (List<KeyValuePair<string, string>>, List<string>) components = GetComponents(data);
            int total = ChangeConnections(components.Item1, components.Item2);
            return total;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("TwentyFive", "Two");
            int currentMax = 0;
            return currentMax;
        }

        private static int ChangeConnections(List<KeyValuePair<string, string>> originalConnections, List<string> nodes)
        {
            int result = 0;
            var variations = new Combinatorics.Collections.Variations<KeyValuePair<string, string>>(originalConnections, 3, Combinatorics.Collections.GenerateOption.WithoutRepetition);
            int variationsChecked = 0;
            foreach (var variation in variations.Where(x => !(x[0].Key == x[1].Value && x[0].Value == x[1].Key) &&
                                             !(x[1].Key == x[2].Value && x[1].Value == x[2].Key) &&
                                             !(x[2].Key == x[0].Value && x[2].Value == x[0].Key)))
            {
                List<KeyValuePair<string, string>> newConnections = originalConnections.ToList();
                List<(string, string)> connectionsToBreak = new(){
                    (variation[0].Key, variation[0].Value),
                    (variation[1].Key, variation[1].Value),
                    (variation[2].Key, variation[2].Value),
                };
                RemoveConnections(newConnections, connectionsToBreak);
                result = GetDisconnectedGroupValue(newConnections);
                if (result > 0)
                {
                    break;
                }
                variationsChecked++;
                if (variationsChecked % 100 == 0)
                {
                    Console.WriteLine($"{variationsChecked} variations Checked");
                }
            }
            return result;
        }

        private static void RemoveConnections(List<KeyValuePair<string, string>> connections, List<(string, string)> connectionsToBreak)
        {
            List<KeyValuePair<string, string>> connectionsToRemove = new();
            foreach (var connectionToBreak in connectionsToBreak)
            {
                connectionsToRemove.Add(new KeyValuePair<string, string>(connectionToBreak.Item1, connectionToBreak.Item2));
                connectionsToRemove.Add(new KeyValuePair<string, string>(connectionToBreak.Item2, connectionToBreak.Item1));
            }

            connections.RemoveAll(x => connectionsToRemove.Contains(x));
        }

        private static int GetDisconnectedGroupValue(List<KeyValuePair<string, string>> graph)
        {
            Dictionary<string, List<string>> ls = new();
            List<string> nodes = new();

            for (int i = 0; i < graph.Count; i++)
            {
                if (!nodes.Contains(graph[i].Key))
                {
                    nodes.Add(graph[i].Key);
                }
                if (!ls.ContainsKey(graph[i].Key))
                {
                    ls.Add(graph[i].Key, new List<string>());
                }
                ls[graph[i].Key].Add(graph[i].Value);
            }

            int totalVisited = 0;
            List<int> groups = new();
            List<string> notVisited = nodes.ToList();
            while (totalVisited < nodes.Count)
            {
                var node = notVisited[0];
                notVisited.RemoveAt(0);
                var visited = DFS(ls, node, new List<string>());
                totalVisited += visited.Count;
                foreach (var visitedNode in visited)
                {
                    notVisited.Remove(visitedNode);
                }
                groups.Add(visited.Count);
            }
            int total = 0;
            if (groups.Count == 2)
            {
                total = groups[0] * groups[1];
            }else if (groups.Count > 2)
            {
                Console.WriteLine($"Oops: {groups.Count}");
            }
            return total;
        }

        private static List<string> DFS(Dictionary<string, List<string>> graph, string currentNode, List<string> visited)
        {
            if (visited.Contains(currentNode))
            {
                return visited;
            }
            visited.Add(currentNode);
            foreach (var item in graph[currentNode])
            {
                DFS(graph, item, visited);
            }
            return visited;
        }

        private static (List<KeyValuePair<string, string>>, List<string>) GetComponents(string[] data)
        {
            List<string> nodeNames = new();
            List<KeyValuePair<string, string>> components = new();
            foreach (var line in data)
            {
                var split = line.Split(": ");
                string componentA = split[0];
                nodeNames.Add(componentA);
                foreach (var componentB in split[1].Split(" "))
                {
                    nodeNames.Add(componentB);
                    if (!components.Any(row => row.Key == componentA && row.Value == componentB))
                    {
                        components.Add(new KeyValuePair<string, string>(componentA, componentB));
                        components.Add(new KeyValuePair<string, string>(componentB, componentA));
                    }
                }
            }
            return (components, nodeNames.Distinct().ToList());
        }
    }
}
