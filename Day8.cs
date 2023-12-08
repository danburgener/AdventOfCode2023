namespace AdventOfCode2023
{
    public static class Day8
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Eight", "One");
            var directions = data[0].ToCharArray();
            Dictionary<string, (string, string)> networks = new();
            foreach (var row in data[2..])
            {
                var splits = row.Split("=");
                var networkStr = splits[1].Trim().Substring(1).Substring(0,8).Split(',').Select(x => x.Trim()).ToList();
                networks.Add(splits[0].Trim(), (networkStr[0], networkStr[1]));
            }
            int steps = 0;
            int directionIndex = 0;
            string currentKey = "AAA";
            string endingKey = "ZZZ";
            while (currentKey != endingKey)
            {
                var network = networks[currentKey];
                var currentDirection = directions[directionIndex];
                if (currentDirection == 'L')
                {
                    currentKey = network.Item1;
                }
                else
                {
                    currentKey = network.Item2;
                }
                directionIndex++;
                if (directionIndex >= directions.Length)
                {
                    directionIndex = 0;
                }
                steps++;
                
            }
            return steps;
        }

        

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Eight", "Two");
            var directions = data[0].ToCharArray();
            Dictionary<string, (string, string)> networks = new();
            foreach (var row in data[2..])
            {
                var splits = row.Split("=");
                var networkStr = splits[1].Trim().Substring(1).Substring(0, 8).Split(',').Select(x => x.Trim()).ToList();
                networks.Add(splits[0].Trim(), (networkStr[0], networkStr[1]));
            }
            long steps = 0;
            int directionIndex = 0;
            List<string> currentKeys = networks.Keys.Where(x => x.EndsWith('A')).ToList();
            int currentKeysCount = currentKeys.Count;
            bool allAtTheSameNode = false;
            while (!allAtTheSameNode)
            {
                allAtTheSameNode = true;
                for (int i = 0; i < currentKeysCount; i++)
                {
                    var network = networks[currentKeys[i]];
                    var currentDirection = directions[directionIndex];
                    if (currentDirection == 'L')
                    {
                        currentKeys[i] = network.Item1;
                        if (allAtTheSameNode && network.Item1[2] != 'Z')
                        {
                            allAtTheSameNode = false;
                        }
                    }
                    else
                    {
                        currentKeys[i] = network.Item2;
                        if (allAtTheSameNode && network.Item2[2] != 'Z')
                        {
                            allAtTheSameNode = false;
                        }
                    }
                }
                directionIndex++;
                if (directionIndex >= directions.Length)
                {
                    directionIndex = 0;
                }
                steps++;
            }
            return steps;
        }
    }
}
