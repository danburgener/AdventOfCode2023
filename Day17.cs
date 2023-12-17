using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class Day17
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Seventeen", "One");
            var sum = AStar.FindFastestPath(data);
            var dijkstra = new Dijkstra();
            var sumDijkstra = dijkstra.Process(data);
            return sum;
        }        

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Seventeen", "Two");
            int currentMax = 0;
            return currentMax;
        }

        

        private class Dijkstra
        {
            public List<Spot> Visited { get; set; } = new();
            public List<Spot> UnVisited { get; set; } = new();

            public class Spot
            {
                public Spot(int row, int column, int cost)
                {
                    Row = row;
                    Column = column;
                    Cost = cost;
                }
                public int Row { get; set; }
                public int Column { get; set; }
                public int Cost { get; set; }
                public int DistanceFromStart { get; set; } = int.MaxValue;
                public Spot? Previous { get; set; }
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

            public int Process(string[] data)
            {
                List<List<Spot>> map = GetMap(data);
                return Process(map, map[0][0], map.Last().Last());
            }

            public int Process(List<List<Spot>> graph, Spot startVertex, Spot endVertex)
            {
                foreach (var row in graph)
                {
                    foreach (var column in row)
                    {
                        column.AddNeighbors(graph);
                        UnVisited.Add(column);
                    }
                }

                startVertex.DistanceFromStart = 0;

                Spot? currentVertex = startVertex;
                while (currentVertex is not null)
                {
                    UnVisited.Remove(currentVertex);
                    Visited.Add(currentVertex);

                    foreach (var neighbor in currentVertex.Neighbors)
                    {
                        if (UnVisited.Contains(neighbor))
                        {
                            int newDistance = currentVertex.DistanceFromStart + neighbor.Cost;
                            if (neighbor.DistanceFromStart > newDistance)
                            {
                                neighbor.DistanceFromStart = newDistance;
                                neighbor.Previous = currentVertex;
                            }
                        }
                    }

                    currentVertex = UnVisited.OrderBy(x => x.DistanceFromStart).FirstOrDefault();
                }

                PrintPath(graph, endVertex);
                return endVertex.DistanceFromStart;
            }

            private bool NotTooStraight(Spot currenSpot, Spot neighbor)
            {
                int maxInLine = 3;
                int directionCount = 0;
                var direction = GetDirection(neighbor, currenSpot);
                Spot spotToCheck = currenSpot;
                while (spotToCheck.Previous != null && directionCount < maxInLine)
                {
                    var previousDirection = GetDirection(spotToCheck, spotToCheck.Previous);
                    if (previousDirection == direction)
                    {
                        directionCount++;
                        spotToCheck = spotToCheck.Previous;
                    }
                    else
                    {
                        return true;
                    }
                }

                return directionCount < maxInLine;
            }

            private static Direction GetDirection(Spot from, Spot to)
            {
                if (from.Row == to.Row)
                {
                    if (from.Column < to.Column)
                    {
                        return Direction.Left;
                    }
                    else
                    {
                        return Direction.Right;
                    }
                }
                else if (from.Row < to.Row)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }

            public enum Direction
            {
                None, Up, Right, Down, Left
            }

            private static string GetCharToPrint(Spot from, Spot to)
            {
                return GetDirection(from, to) switch
                {
                    Direction.Up => "^",
                    Direction.Down => "v",
                    Direction.Left => "<",
                    Direction.Right => ">",
                };
            }

            private static void PrintPath(List<List<Spot>> map, Spot end)
            {
                List<List<string>> mapToPrint = map.Select(row => row.Select(column => column.Cost.ToString()).ToList()).ToList();
                Spot currentSpot = end;

                while (currentSpot != null && currentSpot.Previous != null)
                {
                    mapToPrint[currentSpot.Row][currentSpot.Column] = GetCharToPrint(currentSpot, currentSpot.Previous);
                    currentSpot = currentSpot.Previous;
                }
                for (var row = 0; row < mapToPrint.Count; row++)
                {
                    for (var column = 0; column < mapToPrint[row].Count; column++)
                    {
                        Console.Write(mapToPrint[row][column]);
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
                        rowData.Add(new Spot(row, column, int.Parse(data[row][column].ToString())));
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

        private class AStar
        {
            public static int FindFastestPath(string[] data)
            {
                List<List<Spot>> map = GetMap(data);
                List<Spot> openSet = new();
                List<Spot> closeSet = new();

                Spot start = map[0][0];
                Spot end = map.Last().Last();

                openSet.Add(start);
                while (openSet.Any())
                {
                    var currenSpot = FindLowestF(openSet);
                    if (currenSpot == end)
                    {
                        break;
                    }
                    openSet.Remove(currenSpot);
                    closeSet.Add(currenSpot);

                    var neighbors = currenSpot.Neighbors;
                    for (var i = 0; i < neighbors.Count; i++)
                    {
                        var neighbor = neighbors[i];
                        if (!closeSet.Contains(neighbor) && NotTooStraight(currenSpot, neighbor))
                        {
                            var tempG = currenSpot.G + neighbor.Value * GetDirectionCount(currenSpot, neighbor);

                            var newPath = false;
                            if (openSet.Contains(neighbor))
                            {
                                if (tempG < neighbor.G)
                                {
                                    neighbor.G = tempG;
                                    newPath = true;
                                }
                            }
                            else
                            {
                                neighbor.G = tempG;
                                openSet.Add(neighbor);
                                newPath = true;
                            }

                            if (newPath)
                            {
                                neighbor.H = GetHeuristic(neighbor, end);
                                neighbor.F = neighbor.G + neighbor.H;
                                neighbor.Previous = currenSpot;
                            }
                        }
                    }
                }

                PrintPath(map, end);

                int totalLoss = CalculateTotalLoss(end);

                return totalLoss;
            }

            private static Spot FindLowestF(List<Spot> openSet)
            {
                var winnerIndex = 0;
                for (var i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].F < openSet[winnerIndex].F)
                    {
                        winnerIndex = i;
                    }
                }
                return openSet[winnerIndex];
            }

            private static int CalculateTotalLoss(Spot end)
            {
                int totalLoss = 0;
                var currentSpot = end;
                while (currentSpot.Previous != null)
                {
                    totalLoss += currentSpot.Value;
                    currentSpot = currentSpot.Previous;
                }

                return totalLoss;
            }

            private static bool NotTooStraight(Spot currenSpot, Spot neighbor)
            {
                int maxInLine = 3;
                int directionCount = 0;
                var direction = GetDirection(neighbor, currenSpot);
                Spot spotToCheck = currenSpot;
                while (spotToCheck.Previous != null && directionCount < maxInLine)
                {
                    var previousDirection = GetDirection(spotToCheck, spotToCheck.Previous);
                    if (previousDirection == direction)
                    {
                        directionCount++;
                        spotToCheck = spotToCheck.Previous;
                    }
                    else
                    {
                        return true;
                    }
                }

                return directionCount < maxInLine;
            }

            private static int GetDirectionCount(Spot currenSpot, Spot neighbor)
            {
                int directionCount = 0;
                var direction = GetDirection(neighbor, currenSpot);
                Spot spotToCheck = currenSpot;
                while (spotToCheck.Previous != null)
                {
                    var previousDirection = GetDirection(spotToCheck, spotToCheck.Previous);
                    if (previousDirection == direction)
                    {
                        directionCount++;
                        spotToCheck = spotToCheck.Previous;
                    }
                    else
                    {
                        return directionCount;
                    }
                }
                return directionCount;
            }

            private static void PrintPath(List<List<Spot>> map, Spot end)
            {
                List<List<string>> mapToPrint = map.Select(row => row.Select(column => column.Value.ToString()).ToList()).ToList();
                Spot currentSpot = end;

                while (currentSpot != null && currentSpot.Previous != null)
                {
                    mapToPrint[currentSpot.Row][currentSpot.Column] = GetCharToPrint(currentSpot, currentSpot.Previous);
                    currentSpot = currentSpot.Previous;
                }
                for (var row = 0; row < mapToPrint.Count; row++)
                {
                    for (var column = 0; column < mapToPrint[row].Count; column++)
                    {
                        Console.Write(mapToPrint[row][column]);
                    }
                    Console.WriteLine();
                }
            }

            private static Direction GetDirection(Spot from, Spot to)
            {
                if (from.Row == to.Row)
                {
                    if (from.Column < to.Column)
                    {
                        return Direction.Left;
                    }
                    else
                    {
                        return Direction.Right;
                    }
                }
                else if (from.Row < to.Row)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }

            private static string GetCharToPrint(Spot from, Spot to)
            {
                return GetDirection(from, to) switch
                {
                    Direction.Up => "^",
                    Direction.Down => "v",
                    Direction.Left => "<",
                    Direction.Right => ">",
                };
            }

            private static int GetHeuristic(Spot currentSpot, Spot end)
            {
                var d = Math.Abs(currentSpot.Row - end.Row) + Math.Abs(currentSpot.Column - end.Column);

                return d;
            }

            private static List<List<Spot>> GetMap(string[] data)
            {
                List<List<Spot>> map = new();
                for (var row = 0; row < data.Length; row++)
                {
                    var rowData = new List<Spot>();
                    for (var column = 0; column < data[row].Length; column++)
                    {
                        rowData.Add(new Spot(row, column, int.Parse(data[row][column].ToString())));
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
                public Spot(int row, int column, int value)
                {
                    Row = row;
                    Column = column;
                    Value = value;
                }
                public int Row { get; set; }
                public int Column { get; set; }
                public int H { get; set; } //Straight line distance to end
                public int G { get; set; } //Length of path from start to this node
                public int F { get; set; } // Estimate of the total distance F = G + H
                public int Value { get; set; }
                public List<Spot> Neighbors { get; set; } = new();
                public Spot Previous { get; set; }

                public int MinCostToStart { get; set; }
                public bool Visited { get; set; }
                public Spot NearestToStart { get; set; }

                public void AddNeighbors(List<List<Spot>> map)
                {
                    if (Row > 0)
                    {
                        Neighbors.Add(map[Row - 1][Column]);
                    }
                    if (Column > 0)
                    {
                        Neighbors.Add(map[Row][Column - 1]);
                    }
                    if (Row < map.Count - 1)
                    {
                        Neighbors.Add(map[Row + 1][Column]);
                    }
                    if (Column < map[Row].Count - 1)
                    {
                        Neighbors.Add(map[Row][Column + 1]);
                    }
                }
            }

            public enum Direction
            {
                None, Up, Right, Down, Left
            }
        }
    }
}
