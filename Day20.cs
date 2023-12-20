namespace AdventOfCode2023
{
    public class Day20
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Twenty", "One");
            Dictionary<string, Module> modules = GenerateModules(data);
            (long low, long high) pulses = (0,0);
            for(int i = 0; i < 1000; i++)
            {
                PushButton(modules, ref pulses);
            }
            var sum = pulses.low * pulses.high;
            return sum;
        }

        private static void PushButton(Dictionary<string, Module> modules, ref (long low, long high) pulses)
        {
            Module broadcasterModule = modules["broadcaster"];
            //Console.WriteLine("button -low-> broadcaster");
            pulses.low++;
            broadcasterModule.ReceivePulse(null, Pulse.Low);
            broadcasterModule.ExecuteReceivedPulse();
            List<string> destinationModuleNames = new() { broadcasterModule.Name };
            while (destinationModuleNames.Any())
            {
                List<string> newDestinationModules = new();
                foreach (var destinationModuleName in destinationModuleNames)
                {
                    var destinationModule = modules[destinationModuleName];
                    newDestinationModules.AddRange(SendPulse(modules, destinationModule, ref pulses));
                }
                foreach (var destinationModuleName in newDestinationModules)
                {
                    var destinationModule = modules[destinationModuleName];
                    destinationModule.ExecuteReceivedPulse();
                }

                destinationModuleNames = newDestinationModules;
            }
        }

        private static List<string> SendPulse(Dictionary<string, Module> modules, Module module, ref (long low, long high) pulses)
        {
            Pulse currentPulse = module.GeneratePulse();
            if (currentPulse == Pulse.None)
            {
                return new();
            }
            foreach (var destinationModule in module.DestinationModules)
            {
                if (currentPulse == Pulse.Low)
                {
                    pulses.low++;
                }
                else
                {
                    pulses.high++;
                }
                if (destinationModule != "rx" && destinationModule != "output")
                {
                    modules[destinationModule].ReceivePulse(module, currentPulse);
                }
                //Console.WriteLine($"{module.Name} -{currentPulse}-> {destinationModule}");
            }
            return module.DestinationModules.Where(x => x != "rx" && x != "output").ToList();
        }

        private static Dictionary<string, Module> GenerateModules(string[] data)
        {
            Dictionary<string, Module> modules = new();
            foreach (var line in data) {
                Module module;
                var lineSplit = line.Split(" -> ");
                var destinations = lineSplit[1].Split(", ");
                if (lineSplit[0].StartsWith("%"))
                {
                    module = new FlipFlop(lineSplit[0].TrimStart('%'), destinations.ToList());
                }
                else if (lineSplit[0].StartsWith("&"))
                {
                    module = new Conjunction(lineSplit[0].TrimStart('&'), destinations.ToList());
                }
                else {
                    module = new Broadcast("broadcaster", destinations.ToList());
                }
                modules.Add(module.Name, module);
            }
            foreach(Conjunction conjunctionModule in modules.Where(m => m.Value.GetType() == typeof(Conjunction)).Select(m => m.Value))
            {
                var inputs = modules.Where(m => m.Value.DestinationModules.Contains(conjunctionModule.Name));
                conjunctionModule.SetInput(inputs.Select(i => i.Key).ToList());
            }
            return modules;
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Twenty", "Two");
            int currentMax = 0;
            return currentMax;
        }

        private abstract class Module {
            public Module(string name, List<string> destinationModules)
            {
                Name = name;
                DestinationModules = destinationModules;
            }
            public string Name { get; set; }
            public List<string> DestinationModules { get; set; } = new();
            public abstract Pulse GeneratePulse();
            public abstract void ReceivePulse(Module module, Pulse pulse);
            public abstract void ExecuteReceivedPulse();
        }


        /// <summary>
        /// Symbol is %
        /// Receive High Pulse: ignore
        /// Receive Low Pulse: flips switch. Turn on sends high pulse. Turn off sends low pulse
        /// </summary>
        private class FlipFlop : Module
        {
            public bool On { get; set; } = false;
            private Pulse _currentPulse = Pulse.None;
            private Pulse _receivedPulse = Pulse.None;

            public FlipFlop(string name, List<string> destinationModules) : base(name, destinationModules)
            {
            }

            public override Pulse GeneratePulse()
            {
                return _currentPulse;
            }

            public override void ReceivePulse(Module module, Pulse pulse)
            {
                if (pulse == Pulse.High)
                {
                    _receivedPulse = Pulse.None;
                }
                else
                {
                    On = !On;
                    if (On)
                    {
                        _receivedPulse = Pulse.High;
                    }
                    else
                    {
                        _receivedPulse = Pulse.Low;
                    }
                }
            }

            public override void ExecuteReceivedPulse()
            {
                _currentPulse = _receivedPulse;
            }
        }

        /// <summary>
        /// Symbol is &
        /// Remember type of most recent pulse for each of connected input modules
        /// Sends high pulse unless all inputs are high
        /// </summary>
        private class Conjunction : Module
        {
            public Conjunction(string name, List<string> destinationModules) : base(name, destinationModules)
            {
            }

            public void SetInput(List<string> inputs)
            {
                foreach(var input in inputs)
                {
                    ReceivedInput.Add(input, Pulse.Low);
                }
            }

            public Dictionary<string, Pulse> CurrentInput { get; set; } = new();
            public Dictionary<string, Pulse> ReceivedInput { get; set; } = new();

            public override Pulse GeneratePulse()
            {
                if (CurrentInput.All(x => x.Value == Pulse.High))
                {
                    return Pulse.Low;
                }
                else
                {
                    return Pulse.High;
                }
            }

            public override void ReceivePulse(Module module, Pulse pulse)
            {
                if (ReceivedInput.ContainsKey(module.Name))
                {
                    ReceivedInput[module.Name] = pulse;
                }
                else
                {
                    ReceivedInput.Add(module.Name, pulse);
                }
            }

            public override void ExecuteReceivedPulse()
            {
                CurrentInput = ReceivedInput;
            }
        }

        /// <summary>
        /// Sends same pulse to all destination
        /// </summary>
        private class Broadcast : Module
        {
            private Pulse _currentPulse;
            private Pulse _receivedPulse;

            public Broadcast(string name, List<string> destinationModules) : base(name, destinationModules)
            {
            }

            public override void ExecuteReceivedPulse()
            {
                _currentPulse = _receivedPulse;
            }

            public override Pulse GeneratePulse()
            {
                return _currentPulse;
            }

            public override void ReceivePulse(Module module, Pulse pulse)
            {
                _receivedPulse = pulse;
            }
        }

        public enum Pulse
        {
            None,
            Low,
            High
        }
    }
}
