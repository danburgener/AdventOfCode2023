namespace AdventOfCode2023
{
    public static class Day14
    {
        private static char RoundRock = 'O';
        private static char CubedRock = '#';
        private static char EmptySpace = '.';
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Fourteen", "One");
            List<List<char>> verticals = GenerateVerticals(data);
            var sum = 0;
            foreach(var vertical in verticals)
            {
                RollTheRocksUp(vertical);
                sum += GetVerticalLoad(vertical);
            }

            return sum;
        }

        private static int GetVerticalLoad(List<char> vertical)
        {
            int verticalLoad = 0;
            int rowValue = vertical.Count;
            for (var i = 0; i < vertical.Count; i++)
            {
                if (vertical[i] == RoundRock)
                {
                    verticalLoad += rowValue;
                }
                rowValue--;
            }
            return verticalLoad;
        }

        private static void RollTheRocksUp(List<char> line)
        {
            bool somethingMoved;
            do
            {
                somethingMoved = false;
                for (var i = 0; i < line.Count - 1; i++)
                {
                    if (line[i] == EmptySpace && line[i + 1] == RoundRock)
                    {
                        line[i] = RoundRock;
                        line[i + 1] = EmptySpace;
                        somethingMoved = true;
                    }
                }

            } while (somethingMoved);
        }

        private static List<List<char>> GenerateVerticals(string[] data)
        {
            List<List<char>> map = new();
            List<List<char>> verticals = new();
            for (int row = 0; row < data.Length; row++)
            {
                List<char> rowData = data[row].ToList();
                map.Add(rowData);
            }
            for(var column = 0; column < map[0].Count; column++)
            {
                verticals.Add(map.Select(x => x.ElementAt(column)).ToList());
            }
            return verticals;
        }

        private static List<List<char>> GetVerticals(List<List<char>> map)
        {
            List<List<char>> verticals = new();
            for (var column = 0; column < map[0].Count; column++)
            {
                verticals.Add(map.Select(x => x.ElementAt(column)).ToList());
            }
            return verticals;
        }

        private static List<List<char>> GenerateMap(string[] data)
        {
            List<List<char>> map = new();
            for (int row = 0; row < data.Length; row++)
            {
                List<char> rowData = data[row].ToList();
                map.Add(rowData);
            }
            return map;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Fourteen", "Two");
            
            List<List<char>> map = GenerateMap(data);
            int iterations = 1000;
            List<List<char>> verticals = RollAndGetVerticals(map, iterations);
            var sum = 0;
            foreach(var vertical in verticals)
            {
                sum += GetVerticalLoad(vertical);
            }

            return sum;
        }

        private static List<List<char>> RollAndGetVerticals(List<List<char>> map, int cycles)
        {
            while (cycles > 0)
            {
                RollNorth(map);
                RollWest(map);
                RollSouth(map);
                RollEast(map);


                cycles--;
            }

            return GetVerticals(map);
        }

        private static void RollEast(List<List<char>> map)
        {
            foreach (var line in map)
            {
                bool somethingMoved;
                do
                {
                    somethingMoved = false;
                    for (var i = line.Count - 1; i > 0; i--)
                    {
                        if (line[i] == EmptySpace && line[i - 1] == RoundRock)
                        {
                            line[i] = RoundRock;
                            line[i - 1] = EmptySpace;
                            somethingMoved = true;
                            i--;
                        }
                    }

                } while (somethingMoved);
            }
        }

        private static void RollSouth(List<List<char>> map)
        {
            for (var column = map[0].Count-1; column >= 0; column--)
            {
                bool somethingMoved;
                do
                {
                    somethingMoved = false;
                    for (var row = map.Count - 1; row > 0; row--)
                    {
                        if (map[row][column] == EmptySpace && map[row - 1][column] == RoundRock)
                        {
                            map[row][column] = RoundRock;
                            map[row - 1][column] = EmptySpace;
                            somethingMoved = true;
                            row--;
                        }
                    }

                } while (somethingMoved);
            }
        }

        private static void RollWest(List<List<char>> map)
        {
            foreach (var line in map)
            {
                bool somethingMoved;
                do
                {
                    somethingMoved = false;
                    for (var i = 0; i < line.Count - 1; i++)
                    {
                        if (line[i] == EmptySpace && line[i + 1] == RoundRock)
                        {
                            line[i] = RoundRock;
                            line[i + 1] = EmptySpace;
                            somethingMoved = true;
                            i++;
                        }
                    }

                } while (somethingMoved);
            }
        }

        private static void RollNorth(List<List<char>> map)
        {
            for (var column = 0; column < map[0].Count; column++)
            {
                bool somethingMoved;
                do
                {
                    somethingMoved = false;
                    for (var row = 0; row < map.Count - 1; row++)
                    {
                        if (map[row][column] == EmptySpace && map[row + 1][column] == RoundRock)
                        {
                            map[row][column] = RoundRock;
                            map[row+1][column] = EmptySpace;
                            somethingMoved = true;
                            row++;
                        }
                    }

                } while (somethingMoved);
            }
        }
    }
}
