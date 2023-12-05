namespace AdventOfCode2023
{
    public static class Day4
    {
        public static async Task<long> One()
        {
            int sum = 0;
            var data = await GetDayFourData();

            foreach (var item in data)
            {
                var gameWorth = GetGameWorth(item);
                sum += gameWorth;
            }
            return sum;
        }
        public static async Task<long> Two()
        {
            int sum = 0;
            var data = await GetDayFourTwoData();
            SortedDictionary<int, int> gameCardsWinnings = new SortedDictionary<int, int>();
            SortedDictionary<int, int> gameCardsCopies = new SortedDictionary<int, int>();

            foreach (var item in data)
            {
                var winningNumbers = ScratchCardCalc(item);
                gameCardsWinnings.Add(winningNumbers.gameNumber, winningNumbers.winningNumbersCount);
            }
            foreach (var gameCardKey in gameCardsWinnings.Keys.ToList())
            {
                if (gameCardsCopies.ContainsKey(gameCardKey))
                {
                    gameCardsCopies[gameCardKey]++;
                }
                else
                {
                    gameCardsCopies.Add(gameCardKey, 1);
                }
                if (gameCardsWinnings[gameCardKey] > 0)
                {
                    for (int k = 1; k <= gameCardsCopies[gameCardKey]; k++)
                    {
                        for (int i = 1; i <= gameCardsWinnings[gameCardKey]; i++)
                        {
                            int nextGameCardKey = gameCardKey + i;
                            if (gameCardsWinnings.ContainsKey(nextGameCardKey))
                            {
                                if (gameCardsCopies.ContainsKey(nextGameCardKey))
                                {
                                    gameCardsCopies[nextGameCardKey] += 1;
                                }
                                else
                                {
                                    gameCardsCopies.Add(nextGameCardKey, 1);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var count in gameCardsCopies.Select(gc => gc.Value))
            {
                sum += count;
            }
            return sum;
        }

        private static int GetGameWorth(string text)
        {
            var colonSplit = text.Split(":");

            int gameNumber = int.Parse(colonSplit[0].Split(" ").Last());

            var semiColonSplit = colonSplit[1].Split("|");

            var myNumbers = semiColonSplit[0].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => int.Parse(n.Trim()));
            var winningNumbers = semiColonSplit[1].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => int.Parse(n.Trim()));

            int gameWorth = 0;

            foreach (var myNumber in myNumbers)
            {
                if (winningNumbers.Contains(myNumber))
                {
                    if (gameWorth == 0)
                    {
                        gameWorth = 1;
                    }
                    else
                    {
                        gameWorth *= 2;
                    }
                }
            }

            return gameWorth;
        }

        private static (int gameNumber, int winningNumbersCount) ScratchCardCalc(string text)
        {
            var colonSplit = text.Split(":");

            int gameNumber = int.Parse(colonSplit[0].Split(" ").Last());

            var semiColonSplit = colonSplit[1].Split("|");

            var myNumbers = semiColonSplit[0].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => int.Parse(n.Trim()));
            var winningNumbers = semiColonSplit[1].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => int.Parse(n.Trim()));

            int winningNumbersCount = 0;

            foreach (var myNumber in myNumbers)
            {
                if (winningNumbers.Contains(myNumber))
                {
                    winningNumbersCount++;
                }
            }

            return (gameNumber, winningNumbersCount);
        }

        private static async Task<string[]> GetDayFourData()
        {
            return await File.ReadAllLinesAsync("DayFourOneData.txt");
        }

        private static async Task<string[]> GetDayFourTwoData()
        {
            return await File.ReadAllLinesAsync("DayFourTwoData.txt");
        }
    }
}
