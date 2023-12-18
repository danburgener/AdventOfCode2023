namespace AdventOfCode2023
{
    public static class Day10
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Ten", "One");
            List<List<Node>> list = new();
            for(int i = 0; i < data.Length; i++)
            {
                list.Add(data[i].Select(x => new Node(x)).ToList());
            }
            Node starting = new('S');
            for(int row = 0; row < list.Count; row++)
            {
                for(int column = 0; column < list[row].Count; column++)
                {
                    SetNeighbors(row, column, list);
                    if (list[row][column].Character == 'S')
                    {
                        starting = list[row][column];
                    }
                }
            }
            starting.DistanceFromStart = 0;
            var neighborToCheck = starting.Neighbors.First();
            var previous = starting;
            while(neighborToCheck is not null)
            {
                neighborToCheck.DistanceFromStart = previous.DistanceFromStart + 1;
                var newNeightbor = neighborToCheck.Neighbors.Where(n => n != previous && n.Character != 'S').FirstOrDefault();
                previous = neighborToCheck;
                neighborToCheck = newNeightbor;
            }

            neighborToCheck = starting.Neighbors.Last();
            previous = starting;
            while (neighborToCheck is not null)
            {
                neighborToCheck.DistanceFromStart = previous.DistanceFromStart + 1;
                var newNeightbor = neighborToCheck.Neighbors.Where(n => n != previous && n.Character != 'S').FirstOrDefault();
                if (newNeightbor.DistanceFromStart < neighborToCheck.DistanceFromStart)
                {
                    return neighborToCheck.DistanceFromStart.Value;
                }
                else
                {
                    previous = neighborToCheck;
                    neighborToCheck = newNeightbor;
                }
            }
            
            return 0;
        }

        private static void CalculateNeighborsDistance(Node node, Node previous)
        {
            foreach (var neighbor in node.Neighbors)
            {
                if (neighbor != previous && neighbor.Character != 'S' && neighbor.DistanceFromStart is null)
                {
                    neighbor.DistanceFromStart = node.DistanceFromStart + 1;
                    CalculateNeighborsDistance(neighbor, node);
                }
            }
        }

        private static void SetNeighbors(int row, int column, List<List<Node>> list)
        {
            Node node = list[row][column];
            if (node.Character == '|')
            {
                if (row != 0)
                {
                    var neighbor = list[row - 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (row != list.Count - 1)
                {
                    var neighbor = list[row + 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }else if (node.Character == '-')
            {
                if (column != 0)
                {
                    var neighbor = list[row][column - 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (column != list[row].Count - 1)
                {
                    var neighbor = list[row][column + 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }
            else if (node.Character == 'L')
            {
                if (row != 0)
                {
                    var neighbor = list[row - 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (column != list[row].Count - 1)
                {
                    var neighbor = list[row][column + 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }
            else if (node.Character == 'J')
            {
                if (row != 0)
                {
                    var neighbor = list[row - 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (column != 0)
                {
                    var neighbor = list[row][column - 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }
            else if (node.Character == '7')
            {
                if (column != 0)
                {
                    var neighbor = list[row][column - 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (row != list.Count - 1)
                {
                    var neighbor = list[row + 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }
            else if (node.Character == 'F')
            {
                if (column != list[row].Count - 1)
                {
                    var neighbor = list[row][column + 1];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
                if (row != list.Count - 1)
                {
                    var neighbor = list[row + 1][column];
                    node.Neighbors.Add(neighbor);
                    if (neighbor.Character == 'S')
                    {
                        neighbor.Neighbors.Add(node);
                    }
                }
            }
            else if (node.Character == '.')
            {
                return;
            }
            else if (node.Character == 'S')
            {
                return;
            }
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Ten", "Two");
            Node starting = GetStartingNodeAndSetNeighbors(data);

            List<Node> path = GetPath(starting);

            double area = CalculateArea(path);
            double containing = GetContaining(area, path.Count - 1);

            return (int)containing;
        }

        private static Node GetStartingNodeAndSetNeighbors(string[] data)
        {
            List<List<Node>> list = new();
            for (int i = 0; i < data.Length; i++)
            {
                list.Add(data[i].Select((x, index) => new Node(x, i, index)).ToList());
            }
            Node starting = new('S');
            for (int row = 0; row < list.Count; row++)
            {
                for (int column = 0; column < list[row].Count; column++)
                {
                    SetNeighbors(row, column, list);
                    if (list[row][column].Character == 'S')
                    {
                        starting = list[row][column];
                    }
                }
            }

            return starting;
        }

        private static List<Node> GetPath(Node starting)
        {
            List<Node> path = new()
            {
                starting
            };
            var neighborToCheck = starting.Neighbors.First();
            var previous = starting;
            do
            {
                path.Add(neighborToCheck);
                if (neighborToCheck.Neighbors[0] == previous)
                {
                    previous = neighborToCheck;
                    neighborToCheck = neighborToCheck.Neighbors[1];
                }
                else
                {
                    previous = neighborToCheck;
                    neighborToCheck = neighborToCheck.Neighbors[0];

                }
            } while (starting != neighborToCheck);
            path.Add(starting);
            return path;
        }

        //Uses Pick's Theorem (A = I + B/2 - 1)
        private static double GetContaining(double area, int pointsCount)
        {
            return -((pointsCount / 2) - 1 - area);
        }

        //Uses Shoelace method (Gauss's Area Formula)
        private static double CalculateArea(List<Node> list)
        {
            int forwardSum = 0;
            for (var i = 0; i < list.Count-1; i++)
            {
                forwardSum += list[i].Row * list[i + 1].Column;
            }

            int backwardsSum = 0;
            for (var i = list.Count - 1; i > 0; i--)
            {
                backwardsSum += list[i].Row * list[i - 1].Column;
            }
            ;
            return Math.Abs(forwardSum - backwardsSum) / 2;
        }

        private class Node
        {
            public Node(char character)
            {
                Character = character;
            }
            public Node(char character, int row, int column)
            {
                Character = character;
                Row = row;
                Column = column;
            }
            public int Row { get; set; }
            public int Column { get; set; }
            public char Character { get; set; }
            public List<Node> Neighbors { get; set; } = new();
            public int? DistanceFromStart { get; set; }
        }
    }
}
