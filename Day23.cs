using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class Day23
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("TwentyThree", "One");
            var map = GetMap(data);
            return GetLongestPath(map);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("TwentyThree", "Two");
            var map = GetMap(data);
            try
            {
                return GetLongestPathPartTwo(map);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        const char Path = '.';
        const char Forest = '#';
        const char Up = '^';
        const char Right = '>';
        const char Down = 'v';
        const char Left = '<';
        const char Visited = 'O';

        private static long GetLongestPath(List<List<Spot>> map)
        {
            Spot startingPoint = map[0].First(x => x.Character == Path);
            Spot endingPoint = map.Last().First(x => x.Character == Path);

            Spot currentPoint = startingPoint;
            return FollowPath(map, new List<Spot>(), startingPoint, endingPoint);
        }

        private static long GetLongestPathPartTwo(List<List<Spot>> map)
        {
            Spot startingPoint = map[0].First(x => x.Character == Path);
            Spot endingPoint = map.Last().First(x => x.Character == Path);

            Spot currentPoint = startingPoint;
            return FollowPathPartTwo(map, new List<Spot>(), startingPoint, endingPoint);
        }

        private static int FollowPath(List<List<Spot>> map, List<Spot> explored, Spot currentSpot, Spot endingPoint)
        {
            //PrintMap(map, explored);
            List<int> paths = new List<int>();
            if (currentSpot != endingPoint)
            {
                explored.Add(currentSpot);
                foreach (var neighbor in currentSpot.Neighbors.Where(n => n.Character != Forest))
                {
                    if (explored.Contains(neighbor))
                    {
                        continue;
                    }
                    if (neighbor.Character == Path ||
                        (neighbor.Character == Right && NeighborToRight(currentSpot, neighbor)) ||
                        (neighbor.Character == Left && NeighborToLeft(currentSpot, neighbor)) ||
                        (neighbor.Character == Up && NeighborToUp(currentSpot, neighbor)) ||
                        (neighbor.Character == Down && NeighborToDown(currentSpot, neighbor)))
                    {
                        var longestPath = FollowPath(map, explored.ToList(), neighbor, endingPoint);
                        if (longestPath > 0)
                        {
                            paths.Add(longestPath);
                        }
                    }
                }
            }
            paths.Add(explored.Count);
            return paths.OrderByDescending(x => x).First();
        }

        private static int FollowPathPartTwo(List<List<Spot>> map, List<Spot> explored, Spot currentSpot, Spot endingPoint)
        {
            int currentHighest = 0;
            if (currentSpot != endingPoint)
            {
                //PrintMap(map, explored, currentSpot);
                explored.Add(currentSpot);
                while (currentSpot != endingPoint && OnlyOnePath(explored, currentSpot))
                {
                    foreach (var neighbor in currentSpot.Neighbors)
                    {
                        if (neighbor.Character != Forest && !explored.Contains(neighbor))
                        {
                            currentSpot = neighbor;
                            explored.Add(currentSpot);
                            break;
                        }
                    }
                }
                if (currentSpot == endingPoint)
                {
                    if (currentHighest < explored.Count)
                    {
                        currentHighest = explored.Count;
                        if (currentHighest > 6000)
                        {
                            Console.WriteLine(currentHighest);
                        }
                    }
                }
                else
                {
                    foreach (var neighbor in currentSpot.Neighbors)
                    {
                        if (neighbor.Character == Forest || explored.Contains(neighbor))
                        {
                            continue;
                        }
                        if (neighbor.Character == Path ||
                            (neighbor.Character == Right) ||
                            (neighbor.Character == Left) ||
                            (neighbor.Character == Up) ||
                            (neighbor.Character == Down))
                        {
                            try
                            {
                                RuntimeHelpers.EnsureSufficientExecutionStack();
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                            var longestPath = FollowPathPartTwo(map, explored.ToList(), neighbor, endingPoint);
                            if (currentHighest < longestPath)
                            {
                                currentHighest = longestPath; 
                                if (currentHighest > 6000)
                                {
                                    Console.WriteLine(currentHighest);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (currentHighest < explored.Count)
                {
                    currentHighest = explored.Count;
                }
            }
            return currentHighest;
        }

        private static bool OnlyOnePath(List<Spot> explored, Spot spotToCheck)
        {
            int paths = 0;
            foreach(var neighbor in spotToCheck.Neighbors.Where(n => n.Character != Forest))
            {
                if (!explored.Contains(neighbor))
                {
                    paths++;
                }
            }
            return paths == 1;
        }

        private static void PrintMap(List<List<Spot>> map, List<Spot> explored)
        {
            Console.Clear();
            foreach(var row in map)
            {
                foreach(var column in row)
                {
                    if (explored.Contains(column))
                    {
                        Console.Write(Visited);
                    }
                    else
                    {
                        Console.Write(column.Character);
                    }
                }
                Console.WriteLine();
            }
        }

        private static void PrintMap(List<List<Spot>> map, List<Spot> explored, Spot currentSpot)
        {
            Console.Clear();
            foreach (var row in map)
            {
                foreach (var column in row)
                {
                    if (explored.Contains(column))
                    {
                        Console.Write(Visited);
                    }else if (currentSpot == column)
                    {
                        Console.Write("?"); ;
                    }
                    else
                    {
                        Console.Write(column.Character);
                    }
                }
                Console.WriteLine();
            }
        }

        private static bool NeighborToRight(Spot currentSpot, Spot neighbor)
        {
            return currentSpot.Column < neighbor.Column &&
                currentSpot.Row == neighbor.Row;
        }
        private static bool NeighborToLeft(Spot currentSpot, Spot neighbor)
        {
            return currentSpot.Column > neighbor.Column &&
                currentSpot.Row == neighbor.Row;
        }
        private static bool NeighborToUp(Spot currentSpot, Spot neighbor)
        {
            return currentSpot.Column == neighbor.Column &&
                currentSpot.Row > neighbor.Row;
        }
        private static bool NeighborToDown(Spot currentSpot, Spot neighbor)
        {
            return currentSpot.Column == neighbor.Column &&
                currentSpot.Row < neighbor.Row;
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

        private class Spot
        {
            public Spot(int row, int column, char character)
            {
                Row = row;
                Column = column;
                Character = character;
            }
            public string Key => $"{Row}-{Column}";
            public int Row { get; set; }
            public int Column { get; set; }
            public char Character { get; set; }
            public int StepsFromStart { get; set; }
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
    }
}
