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
        private static int Million = 1000000;

        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Eleven", "One");
            var map = GenerateGalaxiesMap(data);
            List<Spot> galaxies = new List<Spot>();
            for (var i = 0; i < map.Count; i++)
            {
                galaxies.AddRange(map[i].Where(x => x.GalaxyNumber is not null));
            }
            int totalPaths = (galaxies.Count * galaxies.Count - 1) / 2;
            List<int> paths = new();
            for (var i = 0; i < galaxies.Count; i++)
            {
                for (var j = i + 1; j < galaxies.Count; j++)
                {
                    if (i != j)
                    {
                        paths.Add(ManhattanDist(galaxies[i], galaxies[j]));
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

        private static int ManhattanDist(Spot start, Spot end)
        {
            return Math.Abs(end.Row - start.Row) + Math.Abs(end.Column - start.Column);
        }
        private static int ManhattanDistTwo((int, int) start, (int, int) end)
        {
            return Math.Abs(end.Item1 - start.Item1) + Math.Abs(end.Item2 - start.Item2);
        }

        private static List<List<Spot>> GenerateGalaxiesMap(string[] data)
        {
            List<List<Spot>> map = GenerateMapFromFile(data);
            ExpandMapForRows(map);
            ExpandMapForColumns(map);

            return map;
        }
        private static List<List<char>> GenerateGalaxiesMapTwo(string[] data)
        {
            List<List<char>> map = GenerateMapFromFileTwo(data);
            ExpandMapForColumnsTwo(map);
            ExpandMapForRowsTwo(map);

            return map;
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
                        for (var columnToUpdate = column + 1; columnToUpdate < map[rowToUpdate].Count; columnToUpdate++)
                        {
                            map[rowToUpdate][columnToUpdate].Column++;
                        }
                    }
                }
            }
        }
        private static void ExpandMapForColumnsTwo(List<List<char>> map)
        {
            for (var column = 0; column < map[0].Count; column++)
            {
                bool columnHasGalaxy = false;
                for (var row = 0; row < map.Count; row++)
                {
                    if (map[row][column] == Galaxy)
                    {
                        columnHasGalaxy = true;
                        break;
                    }
                }
                if (!columnHasGalaxy)
                {
                    for (var row = 0; row < map.Count; row++)
                    {
                        map[row].InsertRange(column + 1, Enumerable.Repeat(EmptySpace, Million));
                    }
                    column += Million;
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
                    for (var rowToUpdate = row + 1; rowToUpdate < map.Count; rowToUpdate++)
                    {
                        for (var column = 0; column < map[row].Count; column++)
                        {
                            map[rowToUpdate][column].Row++;
                        }
                    }
                }
            }
        }
        private static void ExpandMapForRowsTwo(List<List<char>> map)
        {
            for (var row = 0; row < map.Count; row++)
            {
                if (!map[row].Any(m => m == Galaxy))
                {
                    map.InsertRange(row + 1, Enumerable.Repeat(Enumerable.Repeat(EmptySpace, map[row].Count).ToList(), Million));
                    row += Million;
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
        private static List<List<char>> GenerateMapFromFileTwo(string[] data)
        {
            List<List<char>> map = new();
            for (var row = 0; row < data.Length; row++)
            {
                map.Add(new List<char>());
                var rowData = data[row].ToList();
                for (var column = 0; column < rowData.Count; column++)
                {
                    map[row].Add(rowData[column]);
                }
            }

            return map;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Eleven", "Two");
            var map = GenerateGalaxiesMapTwo(data);
            List<(int, int)> galaxies = new();
            for (var i = 0; i < map.Count; i++)
            {
                var rowGalaxies = map[i].Select((value, index) => new { value, index })
                    .Where(x => x.value == Galaxy)
                    .Select(x => x.index)
                    .ToList();
                for (var j = 0; j < rowGalaxies.Count; j++)
                {
                    galaxies.Add((i, rowGalaxies[j]));
                }
            }
            int totalPaths = (galaxies.Count * galaxies.Count - 1) / 2;
            List<int> paths = new();
            for (var i = 0; i < galaxies.Count; i++)
            {
                for (var j = i + 1; j < galaxies.Count; j++)
                {
                    if (i != j)
                    {
                        paths.Add(ManhattanDistTwo(galaxies[i], galaxies[j]));
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
            public int? GalaxyNumber { get; set; }
        }
    }
}
