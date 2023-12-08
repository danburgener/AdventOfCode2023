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
            List<string> currentKeys = networks.Keys.Where(x => x.EndsWith('A')).ToList();

            List<int> cycles = new List<int>();
            foreach (var key in currentKeys)
            {
                cycles.Add(GetSize(key, networks, directions));
            }

            var lcm = lcm_of_array_elements(cycles.ToArray());


            return steps;
        }

        public static long lcm_of_array_elements(int[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {

                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {

                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate 
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }

        private static int GetSize(string key, Dictionary<string, (string, string)> networks, char[] directions)
        {
            int directionIndex = 0;
            int count = 0;
            while (!key.EndsWith('Z'))
            {
                var currentDirection = directions[directionIndex];
                if (currentDirection == 'L')
                {
                    key = networks[key].Item1;
                }
                else
                {
                    key = networks[key].Item2;
                }
                count++;
                directionIndex = (directionIndex + 1) % directions.Length;
            }
            return count;
        }
    }
}
