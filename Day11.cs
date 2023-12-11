using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023
{
    public static class Day11
    {
        private static char EmptySpace = '.';
        private static char Galaxy = '#';

        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Eleven", "One");
            var map = GenerateGalaxiesMap(data);
            List<Spot> galaxies = new List<Spot>();
            for (var i = 0; i < map.Count; i++)
            {
                galaxies.AddRange(map[i].Where(x => x.GalaxyNumber is not null));
            }
            int totalPaths = (galaxies.Count * galaxies.Count -1)/2;
            List<int> paths = new();
            for (var i = 0; i < galaxies.Count; i++)
            {
                for (var j = i + 1; j < galaxies.Count; j++)
                {
                    if (i != j)
                    {
                        paths.Add(FindPath(galaxies[i], galaxies[j]));
                        if (paths.Count % 10 == 0)
                        {
                            Console.WriteLine($"{paths.Count} of out {totalPaths} complete");
                        }
                    }
                }
            }

            var sum = paths.Sum();
            return sum;
        }

        private static List<List<Spot>> GenerateGalaxiesMap(string[] data)
        {
            List<List<Spot>> map = GenerateMapFromFile(data);
            ExpandMapForRows(map);
            ExpandMapForColumns(map);
            AddNeighbors(map);
            
            return map;
        }

        private static void ResetData(List<List<Spot>> map)
        {
            for (var row = 0; row < map.Count; row++)
            {
                for (var column = 0; column < map[row].Count; column++)
                {
                    map[row][column].Reset();
                }
            }
        }

        private static int FindPath(Spot startSpot, Spot endSpot)
        {
            startSpot.Reset();
            endSpot.Reset();
            List<Spot> openSet = new List<Spot>() { startSpot };
            List<Spot> closedSet = new();
            Spot current = null;

            do
            {
                current = GetLowestFScore(openSet);
                if (current == endSpot)
                {
                    break;
                }

                openSet.Remove(current);
                closedSet.Add(current);
                for (var i = 0; i < current.Neighbors.Count; i++)
                {
                    var neighbor = current.Neighbors[i];
                    if (!closedSet.Contains(neighbor))
                    {
                        var tentativeGScore = current.G + 1;
                        if (openSet.Contains(neighbor))
                        {
                            if (tentativeGScore < neighbor.G)
                            {
                                neighbor.G = tentativeGScore;
                            }
                        }
                        else
                        {
                            neighbor.Reset();
                            neighbor.G = tentativeGScore;
                            openSet.Add(neighbor);
                        }

                        neighbor.H = GetHScore(neighbor, endSpot);
                        neighbor.F = neighbor.G + neighbor.H;
                        neighbor.PreviousNode = current;
                    }
                    //if (closedSet.Contains(neighbor))
                    //{
                    //    continue;
                    //}
                    //var tentativeGScore = current.G + 1;
                    //if (!openSet.Contains(neighbor) || tentativeGScore < neighbor.G)
                    //{
                    //    neighbor.PreviousNode = current;
                    //    neighbor.G = tentativeGScore;
                    //    neighbor.F = tentativeGScore + neighbor.H;
                    //    if (!openSet.Contains(neighbor))
                    //    {
                    //        openSet.Add(neighbor);
                    //    }
                    //}
                }
            } while (openSet.Count > 0);
            return current.G;   
        }

        private static Spot GetLowestFScore(List<Spot> openSet)
        {
            var lowest = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F < lowest.F)
                {
                    lowest = openSet[i];
                }
            }
            return lowest;
        }

        private static void AddNeighbors(List<List<Spot>> map)
        {
            for (var row = 0; row < map.Count; row++)
            {
                for (var column = 0; column < map[row].Count; column++)
                {
                    map[row][column].AddNeighbors(map);
                }
            }
        }

        private static void ExpandMapForColumns(List<List<Spot>> map)
        {
            for (var column = 0; column < map[0].Count; column++)
            {
                bool columnHasGalaxy = false;
                for (var row = 0; row < map.Count; row++)
                {
                    if (map[row][column].GalaxyNumber is not null)
                    {
                        columnHasGalaxy = true;
                        break;
                    }
                }
                if (!columnHasGalaxy)
                {
                    for (var row = 0; row < map.Count; row++)
                    {
                        map[row].Insert(column + 1, new Spot(row, column + 1));
                    }
                    column++;
                    for (var rowToUpdate = 0; rowToUpdate < map.Count; rowToUpdate++)
                    {
                        for (var columnToUpdate = column+1; columnToUpdate < map[rowToUpdate].Count; columnToUpdate++)
                        {
                            map[rowToUpdate][columnToUpdate].Column++;
                        }
                    }
                }
            }
        }

        private static void ExpandMapForRows(List<List<Spot>> map)
        {
            for (var row = 0; row < map.Count; row++)
            {
                if (!map[row].Any(m => m.GalaxyNumber is not null))
                {
                    map.Insert(row + 1, new List<Spot>());
                    for (var column = 0; column < map[row].Count; column++)
                    {
                        map[row + 1].Add(new Spot(row + 1, column));
                    }
                    row++;
                    for (var rowToUpdate = row+1; rowToUpdate < map.Count; rowToUpdate++)
                    {
                        for (var column = 0; column < map[row].Count; column++)
                        {
                            map[rowToUpdate][column].Row++;
                        }
                    }
                }
            }
        }

        private static List<List<Spot>> GenerateMapFromFile(string[] data)
        {
            List<List<Spot>> map = new();
            int galaxyNumber = 1;
            for (var row = 0; row < data.Length; row++)
            {
                map.Add(new List<Spot>());
                var rowData = data[row].ToList();
                for (var column = 0; column < rowData.Count; column++)
                {
                    map[row].Add(new Spot(row, column, rowData[column] == Galaxy ? galaxyNumber++ : null));
                }
            }

            return map;
        }

        private static int GetHScore(Spot start, Spot end)
        {
            //var y = start.Row - end.Row;
            //var x = start.Column - end.Column;

            //return (int)Math.Sqrt(x * x + y * y);

            return Math.Abs(start.Row - end.Row) + Math.Abs(start.Column - end.Column);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Eleven", "Two");

            return 0;
        }

        private class Spot
        {
            public Spot(int row, int column, int? galaxyNumber = null)
            {
                Row = row;
                Column = column;
                GalaxyNumber = galaxyNumber;
            }
            public int Row { get; set; }
            public int Column { get; set; }
            public int H { get; set; }
            public int F { get; set; }
            public int G { get; set; }
            public List<Spot> Neighbors { get; set; } = new();
            public Spot PreviousNode { get; set; }
            public int? GalaxyNumber { get; set; }

            public void Reset()
            {
                F = 0;
                H = 0;
                G = 0;
                PreviousNode = null;
            }

            public void AddNeighbors(List<List<Spot>> map)
            {
                if (Row > 0)
                {//UP
                    Neighbors.Add(map[Row - 1][Column]);
                }
                if (Column > 0)
                {//LEFT
                    Neighbors.Add(map[Row][Column - 1]);
                }
                if (Row < map.Count - 1)
                {//DOWN
                    Neighbors.Add(map[Row + 1][Column]);
                }
                if (Column < map[0].Count - 1)
                {//RIGHT
                    Neighbors.Add(map[Row][Column + 1]);
                }
            }
        }
    }
}
