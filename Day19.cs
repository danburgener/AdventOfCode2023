using Combinatorics.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day19
    {
        const string Extremely = "x";
        const string Musical = "m";
        const string Aerodynamic = "a";
        const string Shiny = "s";

        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Nineteen", "One");
            List<Workflow> workflows = new();
            List<Part> parts = new();
            bool parseWorkflows = true;
            foreach (var line in data) { 
                if (string.IsNullOrWhiteSpace(line))
                {
                    parseWorkflows = false;
                    continue;
                }
                if (parseWorkflows)
                {
                    workflows.Add(ParseWorkflow(line));
                }
                else
                {
                    parts.Add(ParsePart(line));
                }
            }

            long sum = CalculateAcceptedParts(workflows, parts);
            return sum;
        }

        private static long CalculateAcceptedParts(List<Workflow> workflows, List<Part> parts)
        {
            long sum = 0;
            foreach(var part in parts)
            {
                bool partAccepted = CheckPartForAcceptance(part, workflows);
                if (partAccepted)
                {
                    sum += part.x + part.m + part.a + part.s;
                }
            }
            return sum;
        }

        private static long CountAcceptedParts(List<Workflow> workflows, Variations<int> variations)
        {
            long sum = 0;
            foreach (var variation in variations)
            {
                Part part = new()
                {
                    x = variation[0],
                    m = variation[1],
                    a = variation[2],
                    s = variation[3]
                };
                bool partAccepted = CheckPartForAcceptance(part, workflows);
                if (partAccepted)
                {
                    sum += 1;
                }
            }
            return sum;
        }

        private static bool CheckPartForAcceptance(Part part, List<Workflow> workflows)
        {
            //We always start with workflow named "in";
            bool partAccepted = false;
            Workflow currentWorkflow = workflows.First(w => w.Name == "in");
            while (!partAccepted)
            {
                foreach(var rule in currentWorkflow.Rules)
                {
                    if (rule.PartChar is null)
                    {
                        if (rule.Accepted == true)
                        {
                            return true;
                        }
                        else if (rule.Rejected == true)
                        {
                            return false;
                        }
                        else if (rule.NewRuleName is not null)
                        {
                            currentWorkflow = workflows.First(w => w.Name == rule.NewRuleName);
                            break;
                        }
                        else {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        var partNumToCheck = rule.PartChar switch
                        {
                            Extremely => part.x,
                            Musical => part.m,
                            Aerodynamic => part.a,
                            Shiny => part.s,
                        };
                        bool ruleMet = false;
                        if (rule.GreaterThan.Value)
                        {
                            ruleMet = partNumToCheck > rule.ValueCheck;
                        }
                        else
                        {
                            ruleMet = partNumToCheck < rule.ValueCheck;
                        }
                        if (ruleMet)
                        {
                            if (rule.Accepted == true)
                            {
                                return true;
                            }
                            else if (rule.Rejected == true)
                            {
                                return false;
                            }
                            else if (rule.NewRuleName is not null)
                            {
                                currentWorkflow = workflows.First(w => w.Name == rule.NewRuleName);
                                break;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static Part ParsePart(string line)
        {
            Regex regex = new Regex(@"^\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}$");
            Match match = regex.Match(line);

            return new Part()
            {
                x = int.Parse(match.Groups[1].Value),
                m = int.Parse(match.Groups[2].Value),
                a = int.Parse(match.Groups[3].Value),
                s = int.Parse(match.Groups[4].Value)
            };
        }

        private static Workflow ParseWorkflow(string line)
        {
            Workflow workflow = new();
            var lineSplitForName = line.Split('{');
            workflow.Name = lineSplitForName[0];
            var rulesSplit = lineSplitForName[1].TrimEnd('}').Split(',');
            foreach(var ruleDetails in rulesSplit)
            {
                Rule rule;

                if (ruleDetails == "A")
                {
                    rule = new() { Accepted = true };
                }
                else if (ruleDetails == "R")
                {
                    rule = new() { Rejected = true };
                }
                else if (!ruleDetails.Contains(':'))
                {
                    rule = new() { NewRuleName = ruleDetails };
                }
                else
                {
                    rule = new();
                    var ruleDetailsSplit = ruleDetails.Split(":");
                    if (ruleDetailsSplit[1] == "R")
                    {
                        rule.Rejected = true;
                    }else if (ruleDetailsSplit[1] == "A")
                    {
                        rule.Accepted = true;
                    }
                    else
                    {
                        rule.NewRuleName = ruleDetailsSplit[1];
                    }
                    var rulePartSplit = ruleDetailsSplit[0].Split(">");
                    if (rulePartSplit.Length > 1)
                    {
                        rule.GreaterThan = true;
                        rule.PartChar = rulePartSplit[0];
                        rule.ValueCheck = int.Parse(rulePartSplit[1]);
                    }
                    else
                    {
                        rulePartSplit = ruleDetailsSplit[0].Split("<");
                        rule.GreaterThan = false;
                        rule.PartChar = rulePartSplit[0];
                        rule.ValueCheck = int.Parse(rulePartSplit[1]);
                    }
                }

                workflow.Rules.Add(rule);
            }
            return workflow;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Nineteen", "Two");
            List<Workflow> workflows = new();
            foreach (var line in data)
            {
                workflows.Add(ParseWorkflow(line));
            }

            IEnumerable<int> values = Enumerable.Range(1, 4000).ToList();

            var variations = new Combinatorics.Collections.Variations<int>(values, 4, Combinatorics.Collections.GenerateOption.WithRepetition);

            long sum = CountAcceptedParts(workflows, variations);
            return sum;
        }

        public class Workflow {
            public string Name { get; set; }
            public List<Rule> Rules { get; set; } = new();
        }

        public class Rule
        {
            public string? PartChar { get; set; }
            public bool? GreaterThan { get; set; }
            public int? ValueCheck { get; set; }
            public string? NewRuleName { get; set; }
            public bool? Accepted { get; set; }
            public bool? Rejected { get; set; }
        }

        public class Part
        {
            public int x { get; set; }
            public int m { get; set; }
            public int a { get; set; }
            public int s { get; set; }
        }

    }
}
