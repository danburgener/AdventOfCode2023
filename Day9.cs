namespace AdventOfCode2023
{
    public static class Day9
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Nine", "One");
            int sum = 0;
            foreach(var line in data)
            {
                sum += GetLineSeq(line);
            }
            return sum;
        }

        private static int GetLineSeq(string line)
        {
            var endSeq = 0;
            List<int> nums = line.Split(" ").Select(x => int.Parse(x)).ToList();
            List<List<int>> allNums = new List<List<int>>
            {
                nums
            };
            while (nums.Any(n => n != 0))
            {
                nums = GetNextSeq(nums);
                allNums.Add(nums);
            }
            allNums.Last().Add(0);
            for(int i = allNums.Count-1; i > 0; i--)
            {
                var nextNumber = GetNextNumber(allNums[i].Last(), allNums[i - 1].Last());
                endSeq = nextNumber;
                allNums[i-1].Add(nextNumber);
            }
            return endSeq;
        }

        private static List<int> GetNextSeq(List<int> nums)
        {
            List<int> nextNums = new();
            for (int i = 0; i < nums.Count - 1; i++)
            {
                nextNums.Add(nums[i+1] - nums[i]);
            }
            return nextNums;
        }

        private static int GetNextNumber(int bottomNum, int topNum)
        {
            return bottomNum + topNum;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Nine", "Two");
            int sum = 0;
            foreach (var line in data)
            {
                sum += GetLineSeqTwo(line);
            }
            return sum;
        }

        private static int GetLineSeqTwo(string line)
        {
            var endSeq = 0;
            List<int> nums = line.Split(" ").Select(x => int.Parse(x)).ToList();
            List<List<int>> allNums = new List<List<int>>
            {
                nums
            };
            while (nums.Any(n => n != 0))
            {
                nums = GetNextSeq(nums);
                allNums.Add(nums);
            }
            allNums.Last().Insert(0, 0);
            for (int i = allNums.Count - 1; i > 0; i--)
            {
                var nextNumber = GetNextNumberTwo(allNums[i].First(), allNums[i - 1].First());
                endSeq = nextNumber;
                allNums[i - 1].Insert(0, nextNumber);
            }
            return endSeq;
        }

        private static int GetNextNumberTwo(int bottomNum, int topNum)
        {
            return -(bottomNum - topNum);
        }
    }
}
