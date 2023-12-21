namespace AdventOfCode2023
{
    public static class Day21
    {

        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Twentyone", "One");
            Stepper stepper = new Stepper();
            return stepper.Process(data, 64);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Twentyone", "Two");
            int currentMax = 0;
            return currentMax;
        }

        private class Stepper
        {
            const char Rock = '#';
            const char Plot = '.';

            public class Spot
            {
                public Spot(int row, int column, char character)
                {
                    Row = row;
                    Column = column;
                    Character = character;
                }
                public int Row { get; set; }
                public int Column { get; set; }
                public char Character { get; set; }
                public List<Spot> Neighbors { get; set; } = new();

                public void AddNeighbors(List<List<Spot>> graph)
                {
                    if (Row > 0)
                    {
                        Neighbors.Add(graph[Row - 1][Column]);
                    }
                    if (Column > 0)
                    {
                        Neighbors.Add(graph[Row][Column - 1]);
                    }
                    if (Row < graph.Count - 1)
                    {
                        Neighbors.Add(graph[Row + 1][Column]);
                    }
                    if (Column < graph[Row].Count - 1)
                    {
                        Neighbors.Add(graph[Row][Column + 1]);
                    }
                }
            }

            public int Process(string[] data, int maxSteps)
            {
                List<List<Spot>> map = GetMap(data);
                Spot start = null;
                foreach(var row in map)
                {
                    foreach(var column in row)
                    {
                        if (column.Character == 'S')
                        {
                            start = column;
                            break;
                        }
                    }
                }
                return Process(map, start, maxSteps);
            }

            public int Process(List<List<Spot>> map, Spot start, int maxSteps)
            {
                int stepsFromStart = 1;
                List<Spot> currentNeighbors = start.Neighbors.Where(n => n.Character != Rock).ToList();
                List<Spot> reachables = start.Neighbors.Where(n => n.Character != Rock).ToList();
                while (stepsFromStart < maxSteps)
                {
                    stepsFromStart++;
                    currentNeighbors.Clear();
                    foreach (var reachable in reachables)
                    {
                        foreach(var neighbor in reachable.Neighbors.Where(n => n.Character != Rock))
                        {
                            if (!currentNeighbors.Contains(neighbor))
                            {
                                currentNeighbors.Add(neighbor);
                            }
                        }
                    }
                    reachables.Clear();
                    reachables.AddRange(currentNeighbors);
                    //PrintMap(map, reachables, stepsFromStart);
                }

                return reachables.Count;
            }

            private void PrintMap(List<List<Spot>> map, List<Spot> reachables, int currentSteps)
            {
                Console.WriteLine($"Current Steps: {currentSteps}");
                foreach(var row in map)
                {
                    foreach(var column in row)
                    {
                        if (reachables.Contains(column))
                        {
                            Console.Write("O");
                        }
                        else
                        {
                            Console.Write(column.Character);
                        }
                    }
                    Console.WriteLine();
                }
            }

            private static List<List<Spot>> GetMap(string[] data)
            {
                List<List<Spot>> map = new();
                for (var row = 0; row < data.Length; row++)
                {
                    var rowData = new List<Spot>();
                    for (var column = 0; column < data[row].Length; column++)
                    {
                        rowData.Add(new Spot(row, column, data[row][column]));
                    }
                    map.Add(rowData);
                }
                for (var row = 0; row < data.Length; row++)
                {
                    for (var column = 0; column < data[row].Length; column++)
                    {
                        map[row][column].AddNeighbors(map);
                    }
                }
                return map;
            }
        }

    }
}
