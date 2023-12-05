using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day1
    {
        public static async Task<long> One()
        {
            var data = await GetDayOneData();
            string regex = @"(\d)";
            long sum = 0;
            foreach (var item in data)
            {
                var matches = Regex.Matches(item, regex);
                long rowValue = long.Parse(matches[0].Value + matches[matches.Count - 1].Value);
                sum += rowValue;
            }
            return sum;
        }

        public static async Task<long> Two()
        {
            var data = await GetDayOneData();
            string regex = @"(\d|one|two|three|four|five|six|seven|eight|nine)";
            long sum = 0;
            foreach (var item in data)
            {
                var matches = Regex.Matches(item, regex);
                long rowValue = long.Parse(ConvertValue(matches[0].Value) + ConvertValue(matches[matches.Count - 1].Value));
                sum += rowValue;
            }
            return sum;
        }


        private static async Task<string[]> GetDayOneData()
        {
            return await File.ReadAllLinesAsync("DayOneOneData.txt");
        }

        private static string ConvertValue(string value)
        {
            if (!int.TryParse(value, out int result))
            {
                return value switch
                {
                    "one" => "1",
                    "two" => "2",
                    "three" => "3",
                    "four" => "4",
                    "five" => "5",
                    "six" => "6",
                    "seven" => "7",
                    "eight" => "8",
                    "nine" => "9",
                    _ => throw new NotImplementedException()
                };
            }
            return result.ToString();
        }
    }
}
