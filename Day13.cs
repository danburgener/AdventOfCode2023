namespace AdventOfCode2023
{
    public static class Day13
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Thirteen", "One");
            List<Pattern> patterns = GeneratePatterns(data);
            var sum = 0;
            foreach(var pattern in patterns)
            {
                sum += FindReflection(pattern);
            }

            return sum;
        }

        private static int FindReflection(Pattern pattern)
        {
            int horizontal = FindReflectionV2(pattern.Horizontals);
            int vertical = FindReflectionV2(pattern.Verticals);
            return (horizontal * 100) + vertical;
        }

        private static int FindReflectionV2(List<string> lines)
        {
            bool even = lines.Count % 2 == 0;
            int indexToCheck = 0;
            do
            {
                bool perfectReflection = true;
                for (var i = 0; i + indexToCheck < lines.Count-1 && indexToCheck-i >= 0; i++)
                {
                    if (lines[indexToCheck - i] != lines[indexToCheck + i + 1])
                    {
                        perfectReflection = false;
                        break;
                    }
                }
                if (perfectReflection)
                {
                    return indexToCheck + 1;
                }
                indexToCheck++;
            } while (indexToCheck < lines.Count-1);
            return 0;
        }

        private static List<Pattern> GeneratePatterns(string[] data)
        {
            List<Pattern> patterns = new();
            Pattern pattern = new();
            foreach (string line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    patterns.Add(pattern);
                    pattern = new();
                }
                else
                {
                    pattern.Horizontals.Add(line);
                }
            }
            patterns.Add(pattern);
            foreach(var patternToCheck in patterns)
            {
                for(var column = 0; column < patternToCheck.Horizontals.First().Length; column++)
                {
                    patternToCheck.Verticals.Add(string.Join("", patternToCheck.Horizontals.Select(x => x.ElementAt(column))));
                }
            }
            return patterns;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Thirteen", "Two");
            var sum = 0;

            return sum;
        }

        private class Pattern
        {
            public List<string> Horizontals { get; set; } = new();
            public List<string> Verticals { get; set; } = new();
        }
    }
}
