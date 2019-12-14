using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 14)]
    public class Day14 : BaseDay
    {
        public override string PartOne(string input)
        {
            var reactions = input.Lines().Select(x => GetReaction(x)).ToList();

            var elements = new List<(string element, double oreCount)>();
            elements.Add(("ORE", 1));

            var valid = reactions.Where(r => r.Inputs.All(i => elements.Any(e => e.element == i.input)))
                                 .Where(r => !elements.Any(e => e.element == r.Output))
                                 .ToList();

            while (valid.Count > 0)
            {
                foreach (var v in valid)
                {
                    var oreCount = 0.0;

                    foreach (var i in v.Inputs)
                    {
                        oreCount += elements.Single(e => e.element == i.input).oreCount * i.quantity;
                    }

                    elements.Add((v.Output, oreCount / v.Quantity));
                }

                valid = reactions.Where(r => r.Inputs.All(i => elements.Any(e => e.element == i.input)))
                                 .Where(r => !elements.Any(e => e.element == r.Output))
                                 .ToList();
            }

            return elements.Single(e => e.element == "FUEL").oreCount.ToString();
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
