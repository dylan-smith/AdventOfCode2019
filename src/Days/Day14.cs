using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 14)]
    public class Day14 : BaseDay
    {
        private List<Reaction> _reactions;
        private Dictionary<string, long> _chemicals = new Dictionary<string, long>();
        private readonly long _startOre = 1000000000000;

        public override string PartOne(string input)
        {
            InitializeData(input);

            var reset = false;
            
            while (_chemicals["FUEL"] == 0)
            {
                reset = false;
                var useful = _reactions.Where(r => r.Output == "FUEL").ToList();

                while (useful.Any() && !reset)
                {
                    foreach (var r in useful)
                    {
                        if (!reset && r.Inputs.All(i => _chemicals[i.input] >= i.quantity))
                        {
                            PerformReaction(r);
                            reset = true;
                        }
                    }

                    //useful = useful.SelectMany(x => x.Inputs.Where(i => i.input != "ORE").Select(i => _reactions.Single(r => r.Output == i.input))).ToList();
                    //useful = useful.SelectMany(x => x.Inputs.Where(i => _chemicals[i.input] < i.quantity).Select(i => _reactions.Single(r => r.Output == i.input))).ToList();
                    useful = useful.SelectMany(x => x.InputReactions).Where(x => _chemicals[x.reaction.Output] < x.quantity).Select(x => x.reaction).ToList();
                }
            }

            return (_startOre - _chemicals["ORE"]).ToString();
        }

        private void InitializeData(string input)
        {
            _reactions = input.Lines().Select(x => GetReaction(x)).ToList();

            foreach (var r in _reactions)
            {
                _chemicals.Add(r.Output, 0);
            }

            _chemicals.Add("ORE", _startOre);

            

            foreach (var r in _reactions)
            {
                foreach (var i in r.Inputs.Where(x => x.input != "ORE"))
                {
                    r.InputReactions.Add((_reactions.Single(r => r.Output == i.input), i.quantity));
                }
            }
        }

        private void PerformReaction(Reaction reaction)
        {
            _chemicals[reaction.Output] += reaction.Quantity;

            foreach (var (quantity, input) in reaction.Inputs)
            {
                _chemicals[input] -= quantity;
            }
        }

        private Reaction GetReaction(string input)
        {
            var left = input.Split("=>")[0];
            var right = input.Split("=>")[1];

            var result = new Reaction
            {
                Output = right.Words().ElementAt(1),
                Quantity = int.Parse(right.Words().ElementAt(0))
            };

            var inputs = left.Words().ToList();

            for (var i = 0; i < inputs.Count; i += 2)
            {
                result.Inputs.Add((int.Parse(inputs[i]), inputs[i + 1]));
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            InitializeData(input);

            var reset = true;

            while (reset)
            {
                reset = false;
                var useful = _reactions.Where(r => r.Output == "FUEL").ToList();

                while (useful.Any() && !reset)
                {
                    foreach (var r in useful)
                    {
                        if (!reset && r.Inputs.All(i => _chemicals[i.input] >= i.quantity))
                        {
                            PerformReaction(r);
                            reset = true;
                        }
                    }

                    //useful = useful.SelectMany(x => x.Inputs.Where(i => i.input != "ORE" && _chemicals[i.input] < i.quantity).Select(i => _reactions.Single(r => r.Output == i.input))).ToList();
                    useful = useful.SelectMany(x => x.InputReactions).Where(x => _chemicals[x.reaction.Output] < x.quantity).Select(x => x.reaction).ToList();
                }
            }

            return _chemicals["FUEL"].ToString();
        }

        private class Reaction
        {
            public List<(int quantity, string input)> Inputs { get; set; } = new List<(int quantity, string input)>();
            public string Output { get; set; }
            public int Quantity { get; set; }
            public List<(Reaction reaction, int quantity)> InputReactions { get; set; } = new List<(Reaction, int)>();
        }
    }
}
