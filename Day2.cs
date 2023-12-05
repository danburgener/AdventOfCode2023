namespace AdventOfCode2023
{
    public static class Day2
    {
        public static async Task<int> One()
        {
            const int red = 12;
            const int green = 13;
            const int blue = 14;

            int sum = 0;
            var data = await GetDayTwoData();

            foreach (var item in data)
            {
                var gamePossible = IsGamePossible(item, red, green, blue);
                if (gamePossible.Item2)
                {
                    sum += gamePossible.Item1;
                }
            }

            return sum;
        }

        public static async Task<int> Two()
        {
            int sum = 0;
            var data = await GetDayTwoTwoData();

            foreach (var item in data)
            {
                var gamePower = GamePowers(item);
                sum += gamePower;
            }

            return sum;
        }

        private static (int, bool) IsGamePossible(string text, int maxRed, int maxGreen, int maxBlue)
        {
            var colonSplit = text.Split(":");

            int gameNumber = int.Parse(colonSplit[0].Split(" ")[1]);

            var semiColonSplit = colonSplit[1].Split(";");
            foreach (var round in semiColonSplit)
            {
                var roundSplit = round.Split(",");
                foreach (var colorInRound in roundSplit)
                {
                    var colorCount = GetColorAndCount(colorInRound.Trim());
                    var gamePossible = colorCount.Item2 switch
                    {
                        "red" => colorCount.Item1 <= maxRed,
                        "blue" => colorCount.Item1 <= maxBlue,
                        "green" => colorCount.Item1 <= maxGreen,
                        _ => false
                    };
                    if (!gamePossible)
                    {
                        return (gameNumber, false);
                    }
                }
            }
            return (gameNumber, true);
        }

        private static int GamePowers(string text)
        {
            var colonSplit = text.Split(":");

            int gameNumber = int.Parse(colonSplit[0].Split(" ")[1]);
            int biggestRed = 1;
            int biggestGreen = 1;
            int biggestBlue = 1;

            var semiColonSplit = colonSplit[1].Split(";");
            foreach (var round in semiColonSplit)
            {
                var roundSplit = round.Split(",");
                foreach (var colorInRound in roundSplit)
                {
                    var colorCount = GetColorAndCount(colorInRound.Trim());
                    if (colorCount.Item2 == "red")
                    {
                        if (colorCount.Item1 > biggestRed)
                        {
                            biggestRed = colorCount.Item1;
                        }
                    }
                    else if (colorCount.Item2 == "green")
                    {
                        if (colorCount.Item1 > biggestGreen)
                        {
                            biggestGreen = colorCount.Item1;
                        }
                    }
                    else if (colorCount.Item2 == "blue")
                    {
                        if (colorCount.Item1 > biggestBlue)
                        {
                            biggestBlue = colorCount.Item1;
                        }
                    }
                }
            }
            return biggestBlue * biggestGreen * biggestRed;
        }



        private static (int, string) GetColorAndCount(string text)
        {
            var splitText = text.Split(" ");
            return (int.Parse(splitText[0].Trim()), splitText[1].Trim());
        }

        private static async Task<string[]> GetDayTwoData()
        {
            return await File.ReadAllLinesAsync("DayTwoOneData.txt");
        }

        private static async Task<string[]> GetDayTwoTwoData()
        {
            return await File.ReadAllLinesAsync("DayTwoTwoData.txt");
        }
    }
}
