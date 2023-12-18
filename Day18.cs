namespace AdventOfCode2023
{
    public class Day18
    {
        public static async Task<double> One()
        {
            var fileData = await Common.ReadFile("Eighteen", "One");
            List<Data> datas = new();
            foreach(var line in fileData)
            {
                var split = line.Split(' ');
                var newData = new Data
                {
                    Distance = int.Parse(split[1]),
                    Direction = split[0] switch
                    {
                        "R" => Direction.East,
                        "L" => Direction.West,
                        "U" => Direction.North,
                        "D" => Direction.South,
                    }
                };
                datas.Add(newData);
            }

            List<Spot> path = new();
            long currentRow = 0;
            long currentColumn = 0;
            path.Add(new Spot(currentRow, currentColumn));
            foreach(Data data in datas)
            {
                if (data.Direction == Direction.North)
                {
                    path.Add(new Spot(currentRow -= data.Distance, currentColumn));
                }else if (data.Direction == Direction.South)
                {
                    path.Add(new Spot(currentRow += data.Distance, currentColumn));
                }
                else if (data.Direction == Direction.East)
                {
                    path.Add(new Spot(currentRow, currentColumn += data.Distance));
                }
                else if (data.Direction == Direction.West)
                {
                    path.Add(new Spot(currentRow, currentColumn -= data.Distance));
                }
                
            }

            //PrintMap(path);
            double interiorArea = CalculateArea(path);
            double lineCount = datas.Sum(d => d.Distance);
            double containingCount = GetContaining(interiorArea, (int)lineCount);


            return containingCount + lineCount;
        }

        //Uses Pick's Theorem (A = I + B/2 - 1)
        private static double GetContaining(double area, int pointsCount)
        {
            return -((pointsCount / 2) - 1 - area);
        }

        private static void PrintMap(List<Spot> path)
        {
            long maxRow = path.OrderByDescending(p => p.Row).First().Row + 1;
            long maxCoulmn = path.OrderByDescending(p => p.Column).First().Column + 1;
            char[,] map = new char[maxRow, maxCoulmn];
            for (var row = 0; row < maxRow; row++)
            {
                for (var column = 0; column < maxCoulmn; column++)
                {
                    map[row, column] = '.';
                }
            }
            foreach (Spot s in path)
            {
                map[s.Row, s.Column] = '#';
            }
            for(var row = 0; row < maxRow; row++)
            {
                for(var column = 0; column < maxCoulmn; column++)
                {
                    Console.Write(map[row, column]);
                }
                Console.WriteLine();
            }
                
        }   

        //Uses Shoelace method (Gauss's Area Formula)
        private static double CalculateArea(List<Spot> list)
        {
            long forwardSum = 0;
            for (var i = 0; i < list.Count - 1; i++)
            {
                forwardSum += list[i].Row * list[i + 1].Column;
            }

            long backwardsSum = 0;
            for (var i = list.Count - 1; i > 0; i--)
            {
                backwardsSum += list[i].Row * list[i - 1].Column;
            }
            ;
            return Math.Abs(forwardSum - backwardsSum) / 2;
        }

        public static async Task<double> Two()
        {
            var fileData = await Common.ReadFile("Eighteen", "Two");
            List<Data> datas = new();
            foreach (var line in fileData)
            {
                var newData = SetupData(line);
                datas.Add(newData);
            }

            List<Spot> path = new();
            long currentRow = 0;
            long currentColumn = 0;
            path.Add(new Spot(currentRow, currentColumn));
            foreach (Data data in datas)
            {
                if (data.Direction == Direction.North)
                {
                    path.Add(new Spot(currentRow -= data.Distance, currentColumn));
                }
                else if (data.Direction == Direction.South)
                {
                    path.Add(new Spot(currentRow += data.Distance, currentColumn));
                }
                else if (data.Direction == Direction.East)
                {
                    path.Add(new Spot(currentRow, currentColumn += data.Distance));
                }
                else if (data.Direction == Direction.West)
                {
                    path.Add(new Spot(currentRow, currentColumn -= data.Distance));
                }

            }

            //PrintMap(path);
            double interiorArea = CalculateArea(path);
            double lineCount = datas.Sum(d => d.Distance);
            double containingCount = GetContaining(interiorArea, (int)lineCount);


            return containingCount + lineCount;
        }

        private static Data SetupData(string lineData)
        {
            Data data = new Data();
            var split = lineData.Split(' ');
            var hexData = split[2].Where(x => x != '(' && x != ')' && x != '#').ToList();
            data = new Data
            {
                Distance = int.Parse(string.Join("", hexData.Take(5)), System.Globalization.NumberStyles.HexNumber) ,
                Direction = hexData[5] switch
                {
                    '0' => Direction.East,
                    '2' => Direction.West,
                    '3' => Direction.North,
                    '1' => Direction.South,
                }
            };
            return data;
        }

        public class Spot {
            public Spot(long row, long column)
            {
                Row = row;
                Column = column;
            }
            public long Row { get; set; }
            public long Column { get; set; }
        }


        public class Data
        {
            public Direction Direction { get; set; }
            public long Distance { get; set; }
        }

        public enum Direction
        {
            North,
            South,
            West,
            East
        }
    }
}
