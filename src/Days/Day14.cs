using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 14)]
    public class Day14 : BaseDay
    {
        private Dictionary<string, Reaction> _reactions;
        private Dictionary<string, long> _chemicals = new Dictionary<string, long>();
        private readonly long _startOre = 1000000000000;

        public override string PartOne(string input)
        {
            InitializeData(input);
            MakeFuel(1);

            return (_startOre - _chemicals["ORE"]).ToString();
        }

        private void InitializeData(string input)
        {
            _reactions = new Dictionary<string, Reaction>();
            input.Lines().Select(x => GetReaction(x)).ToList().ForEach(x => _reactions.Add(x.Output, x));

            foreach (var r in _reactions)
            {
                _chemicals.Add(r.Key, 0);
            }

            _chemicals.Add("ORE", _startOre);

            foreach (var r in _reactions)
            {
                foreach (var i in r.Value.Inputs.Where(x => x.input != "ORE"))
                {
                    r.Value.InputReactions.Add((_reactions[i.input], i.quantity));
                }
            }
        }

        private void PerformReaction(Reaction reaction, long count)
        {
            _chemicals[reaction.Output] += reaction.Quantity * count;

            foreach (var (quantity, input) in reaction.Inputs)
            {
                _chemicals[input] -= quantity * count;
            }
        }

        private Reaction GetReaction(string input)
        {
            var left = input.Split("=>")[0];
            var right = input.Split("=>")[1];

            var result = new Reaction
            {
                Output = right.Words().ElementAt(1),
                Quantity = long.Parse(right.Words().ElementAt(0))
            };

            var inputs = left.Words().ToList();

            for (var i = 0; i < inputs.Count; i += 2)
            {
                result.Inputs.Add((long.Parse(inputs[i]), inputs[i + 1]));
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            InitializeData(input);

            Log($"Making FUEL in batches of 10000000...");
            while (MakeFuel(10000000)) { };

            Log("Converting back to ORE...");
            ReverseReactions();

            Log($"Making FUEL in batches of 10000...");
            while (MakeFuel(10000)) { };

            Log("Converting back to ORE...");
            ReverseReactions();

            Log($"Making FUEL in batches of 100...");
            while (MakeFuel(100)) { };

            Log("Converting back to ORE...");
            ReverseReactions();

            Log($"Making FUEL in batches of 1...");
            while (MakeFuel(1)) { };

            return _chemicals["FUEL"].ToString();
        }

        private bool MakeFuel(long batchSize)
        {
            var reset = true;

            while (reset)
            {
                reset = false;
                var useful = _reactions.Where(r => r.Key == "FUEL").Select(r => r.Value).ToList();

                while (useful.Any() && !reset)
                {
                    foreach (var r in useful)
                    {
                        if (!reset && r.Inputs.All(i => _chemicals[i.input] >= i.quantity))
                        {
                            var count = GetMaxReactions(r, batchSize);
                            PerformReaction(r, count);

                            if (r.Output == "FUEL")
                            {
                                return true;
                            }

                            reset = true;
                        }
                    }

                    useful = useful.SelectMany(x => x.InputReactions).Where(x => _chemicals[x.reaction.Output] < x.quantity).Select(x => x.reaction).ToList();
                }
            }

            return false;
        }

        private void ReverseReactions()
        {
            var repeat = true;
            var chems = _chemicals.Where(x => x.Key != "FUEL" && x.Key != "ORE").Select(c => c.Key).ToList();

            while (repeat)
            {
                repeat = false;

                foreach (var c in chems)
                {
                    if (_chemicals[c] >= _reactions[c].Quantity)
                    {
                        var count = _chemicals[c] / _reactions[c].Quantity;
                        PerformReaction(_reactions[c], -count);
                        repeat = true;
                    }
                }
            }
        }

        private long GetMaxReactions(Reaction reaction, long max)
        {
            var result = max;

            foreach (var (quantity, input) in reaction.Inputs)
            {
                var count = _chemicals[input] / quantity;

                if (count < result)
                {
                    result = count;
                }
            }

            return result;
        }

        private class Reaction
        {
            public List<(long quantity, string input)> Inputs { get; set; } = new List<(long quantity, string input)>();
            public string Output { get; set; }
            public long Quantity { get; set; }
            public List<(Reaction reaction, long quantity)> InputReactions { get; set; } = new List<(Reaction, long)>();
        }
    }
}
