using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day3
    {
        public static async Task<long> One()
        {
            int sum = 0;
            var dataString = await GetDayOneData();
            var dataDict = GetDataDict(dataString);
            foreach (var row in dataDict)
            {
                foreach (var col in row.Value.Where(v => !v.IsCharacter))
                {
                    if (CheckLeft(col, dataDict) ||
                        CheckRight(col, dataDict) ||
                        CheckUp(col, dataDict) ||
                        CheckDown(col, dataDict) ||
                        CheckLeftUp(col, dataDict) ||
                        CheckLeftDown(col, dataDict) ||
                        CheckRightUp(col, dataDict) ||
                        CheckRightDown(col, dataDict))
                    {
                        sum += col.Number.Value;
                    }
                }
            }

            return sum;
        }

        public static async Task<long> Two()
        {
            int sum = 0;
            var dataString = await GetDayTwoData();
            var dataDict = GetStarDataDict(dataString);
            foreach (var row in dataDict)
            {
                foreach (var col in row.Value.Where(v => v.IsCharacter))
                {
                    List<int> matches = new List<int>();

                    var leftNum = CheckStarLeft(col, dataDict);
                    if (leftNum is not null)
                    {
                        matches.Add(leftNum.Value);
                    }
                    var rightNum = CheckStarRight(col, dataDict);
                    if (rightNum is not null)
                    {
                        matches.Add(rightNum.Value);
                    }
                    var upNum = CheckStarUp(col, dataDict);
                    if (upNum is not null)
                    {
                        matches.AddRange(upNum);
                    }
                    var downNum = CheckStarDown(col, dataDict);
                    if (downNum is not null)
                    {
                        matches.AddRange(downNum);
                    }
                    var leftUpNum = CheckStarLeftUp(col, dataDict);
                    if (leftUpNum is not null)
                    {
                        matches.Add(leftUpNum.Value);
                    }
                    var leftDownNum = CheckStarLeftDown(col, dataDict);
                    if (leftDownNum is not null)
                    {
                        matches.Add(leftDownNum.Value);
                    }
                    var rightUpNum = CheckStarRightUp(col, dataDict);
                    if (rightUpNum is not null)
                    {
                        matches.Add(rightUpNum.Value);
                    }
                    var rightDownNum = CheckStarRightDown(col, dataDict);
                    if (rightDownNum is not null)
                    {
                        matches.Add(rightDownNum.Value);
                    }
                    if (matches.Count == 2)
                    {
                        sum += (matches[0] * matches[1]);
                    }
                }
            }

            return sum;
        }


        private static bool CheckLeft(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.StartIndex > 0 && dataDict[col.Row].Any(row => row.EndIndex == col.StartIndex - 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }

        private static bool CheckRight(Data col, Dictionary<int, List<Data>> dataDict)
        {

            if (dataDict[col.Row].Any(row => row.StartIndex == col.EndIndex + 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }

        private static bool CheckUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && dataDict[col.Row - 1].Any(row => row.IsCharacter && (row.StartIndex >= col.StartIndex && row.EndIndex <= col.EndIndex)))
            {
                return true;
            }
            return false;
        }

        private static bool CheckDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && dataDict[col.Row + 1].Any(row => row.IsCharacter && (row.StartIndex >= col.StartIndex && row.EndIndex <= col.EndIndex)))
            {
                return true;
            }
            return false;
        }

        private static bool CheckLeftUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && col.StartIndex > 0 && dataDict[col.Row - 1].Any(row => row.EndIndex == col.StartIndex - 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }

        private static bool CheckLeftDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && col.StartIndex > 0 && dataDict[col.Row + 1].Any(row => row.EndIndex == col.StartIndex - 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }

        private static bool CheckRightUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && dataDict[col.Row - 1].Any(row => row.StartIndex == col.EndIndex + 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }

        private static bool CheckRightDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && dataDict[col.Row + 1].Any(row => row.StartIndex == col.EndIndex + 1 && row.IsCharacter))
            {
                return true;
            }
            return false;
        }


        private static int? CheckStarLeft(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.StartIndex > 0 && dataDict[col.Row].Any(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter))
            {
                return dataDict[col.Row].First(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static int? CheckStarRight(Data col, Dictionary<int, List<Data>> dataDict)
        {

            if (dataDict[col.Row].Any(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter))
            {
                return dataDict[col.Row].First(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static List<int> CheckStarUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && dataDict[col.Row - 1].Any(row => !row.IsCharacter && (col.StartIndex >= row.StartIndex && col.StartIndex <= row.EndIndex)))
            {
                return dataDict[col.Row - 1].Where(row => !row.IsCharacter && (col.StartIndex >= row.StartIndex && col.StartIndex <= row.EndIndex)).Select(row => row.Number.Value).ToList();
            }
            return null;
        }

        private static List<int> CheckStarDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && dataDict[col.Row + 1].Any(row => !row.IsCharacter && (col.StartIndex >= row.StartIndex && col.StartIndex <= row.EndIndex)))
            {
                return dataDict[col.Row + 1].Where(row => !row.IsCharacter && (col.StartIndex >= row.StartIndex && col.StartIndex <= row.EndIndex)).Select(row => row.Number.Value).ToList();
            }
            return null;
        }

        private static int? CheckStarLeftUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && col.StartIndex > 0 && dataDict[col.Row - 1].Any(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter))
            {
                return dataDict[col.Row - 1].First(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static int? CheckStarLeftDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && col.StartIndex > 0 && dataDict[col.Row + 1].Any(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter))
            {
                return dataDict[col.Row + 1].First(row => row.EndIndex == col.StartIndex - 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static int? CheckStarRightUp(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (col.Row > 1 && dataDict[col.Row - 1].Any(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter))
            {
                return dataDict[col.Row - 1].First(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static int? CheckStarRightDown(Data col, Dictionary<int, List<Data>> dataDict)
        {
            if (dataDict.ContainsKey(col.Row + 1) && dataDict[col.Row + 1].Any(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter))
            {
                return dataDict[col.Row + 1].First(row => row.StartIndex == col.EndIndex + 1 && !row.IsCharacter).Number;
            }
            return null;
        }

        private static Dictionary<int, List<Data>> GetDataDict(string[] dataString)
        {
            int row = 1;
            Regex rx = new Regex(@"\d+|[^\d\.]+");
            Dictionary<int, List<Data>> dataDict = new Dictionary<int, List<Data>>();
            foreach (var item in dataString)
            {
                dataDict[row] = new List<Data>();
                var matches = rx.Matches(item);
                foreach (Match match in matches)
                {
                    Data data = new Data
                    {
                        Row = row,
                        StartIndex = match.Index,
                        EndIndex = match.Index + (match.Value.Length - 1),
                    };

                    if (int.TryParse(match.Value, out int value))
                    {
                        data.Number = value;
                    }
                    else
                    {
                        data.IsCharacter = true;
                    }
                    dataDict[row].Add(data);
                }
                row++;
            }

            return dataDict;
        }

        private static Dictionary<int, List<Data>> GetStarDataDict(string[] dataString)
        {
            int row = 1;
            Regex rx = new Regex(@"\d+|\*+");
            Dictionary<int, List<Data>> dataDict = new Dictionary<int, List<Data>>();
            foreach (var item in dataString)
            {
                dataDict[row] = new List<Data>();
                var matches = rx.Matches(item);
                foreach (Match match in matches)
                {
                    Data data = new Data
                    {
                        Row = row,
                        StartIndex = match.Index,
                        EndIndex = match.Index + (match.Value.Length - 1),
                    };

                    if (int.TryParse(match.Value, out int value))
                    {
                        data.Number = value;
                    }
                    else
                    {
                        data.IsCharacter = true;
                    }
                    dataDict[row].Add(data);
                }
                row++;
            }

            return dataDict;
        }


        private static async Task<string[]> GetDayOneData()
        {
            return await File.ReadAllLinesAsync("DayThreeOneData.txt");
        }

        private static async Task<string[]> GetDayTwoData()
        {
            return await File.ReadAllLinesAsync("DayThreeTwoData.txt");
        }

        private class Data
        {
            public int Row { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int? Number { get; set; }
            public bool IsCharacter { get; set; }
        }
    }
}
