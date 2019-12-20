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

        public override string PartTwo(string input)
        {
            var baseSignal = input.Trim().Select(x => int.Parse(x.ToString())).ToList();
            var signal = baseSignal.Select(x => x).ToList();

            for (var i = 0; i < 9999; i++)
            {
                signal.AddRange(baseSignal);
            }

            for (var p = 0; p < 100; p++)
            {
                Log($"Phase {p}");
                signal = ProcessPhase2(signal).ToList();
            }

            return $"{signal[0]}{signal[1]}{signal[2]}{signal[3]}{signal[4]}{signal[5]}{signal[6]}{signal[7]}";
        }

        private IEnumerable<int> ProcessPhase(List<int> signal)
        {
            for (var i = 0; i < signal.Count; i++)
            {
                Log($"   {i}");
                yield return TransformElement(signal, i + 1);
            }
        }

        private List<int> ProcessPhase2(List<int> signal)
        {
            var result = new List<int>(signal.Count);
            result.Initialize(0, signal.Count);

            var sum = 0;

            for (var i = signal.Count - 1; i >= (signal.Count / 2); i--)
            { 
                //Log($"   {i}");
                sum += signal[i];
                result[i] = sum % 10;
            }

            sum -= signal[signal.Count - 1];
            sum += signal[(signal.Count / 2) - 1];
            result[(signal.Count / 2) - 1] = sum % 10;

            var pos = (signal.Count / 2) - 2;
            var val1 = signal.Count - 2;
            var val2 = signal.Count - 3;

            while (signal.Count - val2 <= pos + 1)
            {
                sum -= signal[val1];
                sum -= signal[val2];
                sum += signal[pos];

                result[pos] = sum % 10;

                pos--;
                val1 -= 2;
                val2 -= 2;
            }

            for (var i = 0; i <= pos; i++)
            {
                Log($"   {i}");
                result[i] = TransformElement(signal, i + 1);
            }

            return result;
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
    }
}
