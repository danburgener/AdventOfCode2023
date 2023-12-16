using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day15
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Fifteen", "One");
            var line = data[0];
            var sequences = line.Split(',');
            var sum = 0;
            foreach (var sequence in sequences)
            {
                sum += GetHash(sequence);
            }

            return sum;
        }

        private static int GetHash(string sequence)
        {
            var value = 0;
            foreach(var character in sequence)
            {
                value += (int)character;
                value *= 17;
                value = value % 256;
            }
            return value;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Fifteen", "Two");
            List<Box> boxes = Enumerable.Range(0, 256).Select(x => new Box()).ToList();
            var sum = 0;
            var sequences = data[0].Split(',');
            foreach (var sequence in sequences)
            {
                ProcessSequence(sequence, boxes);
            }
            for(var i = 0; i < boxes.Count; i++)
            {
                for(var j = 0; j < boxes[i].Lenses.Count; j++)
                {
                    var lensValue = (i + 1) * (j + 1) * boxes[i].Lenses[j].FocalLength;
                    sum += lensValue; ;
                }
            }

            return sum;
        }

        private static void ProcessSequence(string sequence, List<Box> boxes)
        {
            var len = new Lens(sequence);
            if (len.Operation == "=")
            {
                var lensToPutInBox = len;
                if (lensToPutInBox is not null)
                {
                    Box box = boxes[lensToPutInBox.HashValue];
                    var lensToRemove = box.Lenses.SingleOrDefault(l => l.Label == lensToPutInBox.Label);
                    if (lensToRemove is not null)
                    {
                        var index = box.Lenses.IndexOf(lensToRemove);
                        box.Lenses.Insert(index, lensToPutInBox);
                        box.Lenses.Remove(lensToRemove);
                    }
                    else
                    {
                        box.Lenses.Add(lensToPutInBox);
                    }
                }
            }
            else
            {
                var box = boxes[len.HashValue];
                var lensInBox = box.Lenses.SingleOrDefault(l => l.Label == len.Label);
                if (lensInBox is not null)
                {
                    box.Lenses.Remove(lensInBox);
                }
            }
        }

        public class Box
        {
            public List<Lens> Lenses { get; set; } = new();
        }
        static Regex regex = new Regex(@"^([a-z]+)([-=])(\d*)$");
        public class Lens
        {
            public Lens(string sequence)
            {
                Sequence = sequence;
                var matches = regex.Matches(sequence);
                var match = matches.First();
                Label = match.Groups[1].Value;
                Operation = match.Groups[2].Value;
                FocalLength = Operation == "-" ? 0 : int.Parse(match.Groups[3].Value);
                HashValue = GetHash(Label);
            }
            public string Sequence { get; set; }
            public string Label { get; set; }
            public int FocalLength { get; set; }
            public string Operation { get; set; }
            public int HashValue { get; set; }
        }
    }
}
