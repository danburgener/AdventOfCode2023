using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public static class Day12
    {
        private static char Operational = '.';
        private static char Damaged = '#';
        private static char Unknown = '?';
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Twelve", "One");
            var sum = 0;
            foreach(var item in data)
            {
                sum += CalculateArrangements(item);
            }

            return sum;
        }

        private static int CalculateArrangements(string item)
        {
            var splitItem = item.Split(' ');
            List<string> possibles = GeneratePossibles(splitItem[0]);

            List<int> damagedRecords = splitItem[1].Split(',').Select(x => int.Parse(x)).ToList();
            int arragements = CalculateArragements(damagedRecords, possibles);
            return arragements;
        }

        private static int CalculateArragements(List<int> damagedRecords, List<string> possibles)
        {
            Regex regex = GenerateRegex(damagedRecords);
            int arragements = 0;
            foreach (var possible in possibles)
            {
                if (regex.IsMatch(possible))
                {
                    arragements++;
                }
            }

            return arragements;
        }

        private static List<string> GeneratePossibles(string charConditionRecords)
        {
            int unknowns = charConditionRecords.Count(x => x == Unknown);
            var variations = new Combinatorics.Collections.Variations<char>(new List<char> { Operational, Damaged }, unknowns, Combinatorics.Collections.GenerateOption.WithRepetition);
            List<string> possiblesCombos = variations.ToList().Select(p => string.Join("", p)).ToList();
            List<string> possibles = new();
            foreach(var possibleCombo in possiblesCombos)
            {
                int currentUnknownIndex = 0;
                List<char> tempRecords = charConditionRecords.ToList();
                for(var i = 0; i < tempRecords.Count; i++)
                {
                    if (tempRecords[i] == Unknown)
                    {
                        tempRecords[i] = possibleCombo[currentUnknownIndex++];
                    }
                }
                possibles.Add(string.Join("", tempRecords));
            }
            return possibles;
        }

        private static Regex GenerateRegex(List<int> damagedRecords)
        {
            string regexString = "";
            for(var i = 0; i < damagedRecords.Count; i++)
            {
                if (i == 0)
                {
                    regexString = $@"^\.*\#{{{damagedRecords[i]}}}\.+";
                }
                else if (i == damagedRecords.Count-1)
                {
                    regexString += $@"\#{{{damagedRecords[i]}}}\.*$";
                }
                else
                {
                    regexString += $@"\#{{{damagedRecords[i]}}}\.+";
                }
            }
            return new Regex(regexString);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Twelve", "Two");

            var sum = 0;
            return sum;
        }
    }
}
