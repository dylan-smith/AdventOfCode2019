using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 14)]
    public class Day14 : BaseDay
    {
        private List<Reaction> _reactions;

        public override string PartOne(string input)
        {
            _reactions = input.Lines().Select(x => GetReaction(x)).ToList();

            var needed = new Dictionary<string, int>();
            var leftover = new Dictionary<string, int>();
            var reaction = _reactions.Single(r => r.Output == "FUEL");

            foreach (var i in reaction.Inputs)
            {
                needed.SafeIncrement(i.input, i.quantity);
            }

            while (needed.Count > 1 || needed.Keys.First() != "ORE")
            {
                var neededCopy = needed;
                needed = new Dictionary<string, int>();

                foreach (var n in neededCopy)
                {
                    if (n.Key == "ORE")
                    {
                        needed.SafeIncrement(n.Key, n.Value);
                    }
                    else
                    {
                        reaction = _reactions.Single(r => r.Output == n.Key);
                        var amountNeeded = n.Value;

                        if (leftover.ContainsKey(n.Key))
                        {
                            amountNeeded -= leftover[n.Key];

                            if (amountNeeded < 0)
                            {
                                leftover[n.Key] = 0 - amountNeeded;
                                amountNeeded = 0;
                            }
                        }

                        var amount = (int)Math.Ceiling((double)amountNeeded / (double)reaction.Quantity);
                        leftover.SafeIncrement(n.Key, (amount * reaction.Quantity) - amountNeeded);

                        foreach (var i in reaction.Inputs)
                        {
                            needed.SafeIncrement(i.input, i.quantity * amount);
                        }
                    }
                }
            }

            return needed["ORE"].ToString();

            //var needed = reactions.Single(r => r.Output == "FUEL").Inputs;

            //while (needed.Count > 1 || needed[0].input != "ORE")
            //{

            //}

            //var elements = new List<(string element, double oreCount)>();
            //elements.Add(("ORE", 1));

            //var valid = reactions.Where(r => r.Inputs.All(i => elements.Any(e => e.element == i.input)))
            //                     .Where(r => !elements.Any(e => e.element == r.Output))
            //                     .ToList();

            //while (valid.Count > 0)
            //{
            //    foreach (var v in valid)
            //    {
            //        var oreCount = 0.0;

            //        foreach (var i in v.Inputs)
            //        {
            //            oreCount += elements.Single(e => e.element == i.input).oreCount * i.quantity;
            //        }

            //        elements.Add((v.Output, oreCount / v.Quantity));
            //    }

            //    valid = reactions.Where(r => r.Inputs.All(i => elements.Any(e => e.element == i.input)))
            //                     .Where(r => !elements.Any(e => e.element == r.Output))
            //                     .ToList();
            //}

            //return elements.Single(e => e.element == "FUEL").oreCount.ToString();
        }

        private Reaction GetReaction(string input)
        {
            var left = input.Split("=>")[0];
            var right = input.Split("=>")[1];

            var result = new Reaction();

            result.Output = right.Words().ElementAt(1);
            result.Quantity = int.Parse(right.Words().ElementAt(0));

            var inputs = left.Words().ToList();

            for (var i = 0; i < inputs.Count; i += 2)
            {
                result.Inputs.Add((int.Parse(inputs[i]), inputs[i + 1]));
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }

        private class Reaction
        {
            public List<(int quantity, string input)> Inputs { get; set; } = new List<(int quantity, string input)>();
            public string Output { get; set; }
            public int Quantity { get; set; }
        }
    }
}
