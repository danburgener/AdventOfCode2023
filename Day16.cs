using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2023.Day15;

namespace AdventOfCode2023
{
    public static class Day16
    {
        private const char EmptySpace = '.';
        private const char MirrorRight = '/';
        private const char MirrorLeft = '\\';
        private const char SplitterHorizontal = '-';
        private const char SplitterVertical = '|';

        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Sixteen", "One");
            List<List<Node>> nodeMap = GetNodeMap(data);
            ShineLight(nodeMap, Direction.Right, 0, 0);

            var sum = 0;
            foreach(var row in nodeMap)
            {
                foreach(var column in row)
                {
                    if (column.VisitedFrom.Any())
                    {
                        Console.Write("#");
                        sum++;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }

            return sum;
        }

        private static void ShineLight(List<List<Node>> nodeMap, Direction currentDirection, int currentRow, int currentCol)
        {
            if (currentRow < 0 || currentCol < 0 || currentRow > nodeMap.Count-1 || currentCol > nodeMap[0].Count-1)
            {
                return;
            }
            Node currentNode = nodeMap[currentRow][currentCol];
            if (currentNode.VisitedFrom.Contains(currentDirection))
            {
                return;
            }
            currentNode.VisitedFrom.Add(currentDirection);
            List<Direction> nextDirections = GetNextDirections(currentNode.Character, currentDirection);
            foreach(var nextDirection in nextDirections)
            {
                (int, int) nextRowAndCol = nextDirection switch { 
                    Direction.Up => (currentRow-1, currentCol),
                    Direction.Right => (currentRow, currentCol+1),
                    Direction.Down => (currentRow + 1, currentCol),
                    Direction.Left => (currentRow, currentCol-1),
                };
                ShineLight(nodeMap, nextDirection, nextRowAndCol.Item1, nextRowAndCol.Item2);
            }
        }

        private static List<Direction> GetNextDirections(char currentCharacter, Direction currentDirection)
        {
            return currentDirection switch { 
                Direction.Up => GetNextDirectionsUp(currentCharacter),
                Direction.Right => GetNextDirectionsRight(currentCharacter),
                Direction.Down => GetNextDirectionsDown(currentCharacter),
                Direction.Left => GetNextDirectionsLeft(currentCharacter),
            };
        }

        private static List<Direction> GetNextDirectionsUp(char currentCharacter)
        {
            return currentCharacter switch
            {
                EmptySpace => new() { Direction.Up},
                MirrorLeft => new() { Direction.Left },
                MirrorRight => new() { Direction.Right },
                SplitterVertical => new() { Direction.Up},
                SplitterHorizontal => new() { Direction.Left, Direction.Right},
            };
        }

        private static List<Direction> GetNextDirectionsDown(char currentCharacter)
        {
            return currentCharacter switch
            {
                EmptySpace => new() { Direction.Down },
                MirrorLeft => new() { Direction.Right },
                MirrorRight => new() { Direction.Left },
                SplitterVertical => new() { Direction.Down },
                SplitterHorizontal => new() { Direction.Left, Direction.Right },
            };
        }

        private static List<Direction> GetNextDirectionsRight(char currentCharacter)
        {
            return currentCharacter switch
            {
                EmptySpace => new() { Direction.Right },
                MirrorLeft => new() { Direction.Down },
                MirrorRight => new() { Direction.Up },
                SplitterVertical => new() { Direction.Up, Direction.Down },
                SplitterHorizontal => new() { Direction.Right },
            };
        }

        private static List<Direction> GetNextDirectionsLeft(char currentCharacter)
        {
            return currentCharacter switch
            {
                EmptySpace => new() { Direction.Left },
                MirrorLeft => new() { Direction.Up },
                MirrorRight => new() { Direction.Down },
                SplitterVertical => new() { Direction.Up, Direction.Down },
                SplitterHorizontal => new() { Direction.Left },
            };
        }

        private static List<List<Node>> GetNodeMap(string[] data)
        {
            return data.Select(row => row.Select(column => new Node(column)).ToList()).ToList();
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Sixteen", "Two");
            List<List<Node>> nodeMap = GetNodeMap(data);

            int currentMax = 0;
            for(var row = 0; row < nodeMap.Count-1; row++)
            {
                int energizedNodes = GetEnergizedNodes(nodeMap, Direction.Right, row, 0);
                currentMax = energizedNodes > currentMax ? energizedNodes : currentMax;
                energizedNodes = GetEnergizedNodes(nodeMap, Direction.Left, row, nodeMap[row].Count-1);
                currentMax = energizedNodes > currentMax ? energizedNodes : currentMax;
            }

            for(var column = 0; column < nodeMap[0].Count-1; column++)
            {
                int energizedNodes = GetEnergizedNodes(nodeMap, Direction.Down, 0, column);
                currentMax = energizedNodes > currentMax ? energizedNodes : currentMax;
                energizedNodes = GetEnergizedNodes(nodeMap, Direction.Up, nodeMap.Count-1, column);
                currentMax = energizedNodes > currentMax ? energizedNodes : currentMax;
            }


            return currentMax;
        }

        private static int GetEnergizedNodes(List<List<Node>> nodeMap, Direction startingDirection, int startingRow, int startingColumn)
        {
            foreach(var row in nodeMap)
            {
                foreach(var column in row)
                {
                    column.VisitedFrom = new();
                }
            }
            ShineLight(nodeMap, startingDirection, startingRow, startingColumn);

            var sum = 0;
            foreach (var row in nodeMap)
            {
                foreach (var column in row)
                {
                    if (column.VisitedFrom.Any())
                    {
                        sum++;
                    }
                    else
                    {
                    }
                }
            }
            return sum;
        }

        public class Node
        {
            public Node(char character)
            {
                Character = character;
            }
            public char Character { get; set; }
            public List<Direction> VisitedFrom { get; set; } = new();

        }

        public enum Direction
        {
            Up, Right, Down, Left
        }
    }
}
