namespace AdventOfCode2023
{
    public static class Day6
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Six", "One");
            var timeRow = data[0];
            var distanceRow = data[01];

            List<int> times = timeRow.Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
            List<int> distances = distanceRow.Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
            int total = 0;
            for(int i = 0; i < times.Count; i++)
            {
                var waysToWinThisGame = GetWaysToWin(times[i], distances[i]);
                if (total == 0)
                {
                    total = waysToWinThisGame;
                }
                else
                {
                    total *= waysToWinThisGame;
                }
            }
            return total;
        }

        private static int GetWaysToWin(int raceTime, int recordDistance)
        {
            int waysToWin = 0;
            for(int timePressed = 1; timePressed < raceTime; timePressed++)
            {
                int totalDistance = timePressed * (raceTime - timePressed);
                if (totalDistance > recordDistance)
                {
                    waysToWin++;
                }
            }
            return waysToWin;
        }
        private static long GetWaysToWin(long raceTime, long recordDistance)
        {
            long waysToWin = 0;
            for (long timePressed = 1; timePressed < raceTime; timePressed++)
            {
                long totalDistance = timePressed * (raceTime - timePressed);
                if (totalDistance > recordDistance)
                {
                    waysToWin++;
                }
            }
            return waysToWin;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Six", "Two");
            var timeRow = data[0];
            var distanceRow = data[01];

            long time = long.Parse(string.Join("", timeRow.Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x)));
            long distance = long.Parse(string.Join("", distanceRow.Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x)));
            return GetWaysToWin(time, distance);
        }
    }
}
