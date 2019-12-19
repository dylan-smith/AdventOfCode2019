using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 16)]
    public class Day16 : BaseDay
    {
        public override string PartOne(string input)
        {
            var signal = input.Trim().Select(x => int.Parse(x.ToString())).ToList();

            for (var p = 0; p < 100; p++)
            {
                signal = ProcessPhase(signal).ToList();
            }

            return $"{signal[0]}{signal[1]}{signal[2]}{signal[3]}{signal[4]}{signal[5]}{signal[6]}{signal[7]}";
        }

        private IEnumerable<int> ProcessPhase(List<int> signal)
        {
            for (var i = 0; i < signal.Count; i++)
            {
                yield return TransformElement(signal, i + 1);
            }
        }

        private int TransformElement(List<int> signal, int position)
        {
            var pattern = GetPattern(position, signal.Count);

            var value = signal.Zip(pattern, (a, b) => a * b).Sum();

            return Math.Abs(value) % 10;
        }

        private List<int> GetPattern(int position, int length)
        {
            var basePattern = new List<int>();

            basePattern.AddMany(0, position);
            basePattern.AddMany(1, position);
            basePattern.AddMany(0, position);
            basePattern.AddMany(-1, position);

            while (basePattern.Count < (length + 1))
            {
                basePattern.AddRange(basePattern);
            }

            return basePattern.Skip(1).Take(length).ToList();
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
