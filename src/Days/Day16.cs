using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 16)]
    public class Day16 : BaseDay
    {
        private Dictionary<(int, int), int> _phaseValues = new Dictionary<(int, int), int>();
        private int[] _signal;


        public override string PartOne(string input)
        {
            var signal = input.Trim().Select(x => int.Parse(x.ToString())).ToArray();

            for (var p = 0; p < 100; p++)
            {
                signal = ProcessPhase2(signal);
            }

            //return string.Concat(signal.Select(x => x.ToString()));
            return $"{signal[0]}{signal[1]}{signal[2]}{signal[3]}{signal[4]}{signal[5]}{signal[6]}{signal[7]}";
        }

        public override string PartTwo(string input)
        {
            var signalRepeat = 10000;
            var baseSignal = input.Trim().Select(x => int.Parse(x.ToString())).ToArray();
            _signal = new int[baseSignal.Length * signalRepeat];

            for (var i = 0; i < signalRepeat; i++)
            {
                baseSignal.CopyTo(_signal, i * baseSignal.Length);
            }

            var messageLocation = int.Parse(string.Concat(input.Take(7)));
            //var result = string.Empty;

            //for (var i = 0; i < 8; i++)
            //{
            //    _phaseValues.Add((99, messageLocation + i), GetElement(99, messageLocation + i));
            //    result += _phaseValues[(99, messageLocation + i)].ToString();
            //}

            for (var p = 0; p < 100; p++)
            {
                Log($"{p}");
                _signal = ProcessPhase2(_signal);
            }

            ////return string.Concat(signal.Select(x => x.ToString()));
            ////return $"{signal[0]}{signal[1]}{signal[2]}{signal[3]}{signal[4]}{signal[5]}{signal[6]}{signal[7]}";

            //var messageLocation = int.Parse(string.Concat(input.Take(7)));
            ////var messageLocation = 6023;
            var result = string.Concat(_signal.Skip(messageLocation).Take(8).Select(x => x.ToString()));

            return result;
        }

        private int GetElement(int phase, int position)
        {
            if (_phaseValues.ContainsKey((phase, position)))
            {
                return _phaseValues[(phase, position)];
            }

            if (phase == 0)
            {
                return _signal[position];
            }

            var sum = 0;
            var multiplier = -1;
            var stop = 0;

            while (stop < _signal.Length)
            {
                multiplier += 2;
                stop = Math.Min((position * (multiplier + 1)) - 1, _signal.Length);

                for (var i = (position * multiplier) - 1; i < stop; i++)
                {
                    sum += GetElement(phase - 1, i);
                    //sum += signal[i];
                }

                multiplier += 2;
                stop = Math.Min((position * (multiplier + 1)) - 1, _signal.Length);

                for (var i = (position * multiplier) - 1; i < stop; i++)
                {
                    //sum -= signal[i];
                    sum -= GetElement(phase - 1, i);
                }
            }

            var result = Math.Abs(sum) % 10;
            _phaseValues.Add((phase, position), result);
            return result;
        }

        private int[] ProcessPhase2(int[] signal)
        {
            var result = new int[signal.Length];
            var sum = 0;

            for (var i = signal.Length - 1; i >= (signal.Length / 2); i--)
            {
                sum += signal[i];
                result[i] = sum % 10;
            }

            sum += signal[(signal.Length / 2) - 1];
            sum -= signal[signal.Length - 1];
            result[(signal.Length / 2) - 1] = sum % 10;

            var pos = (signal.Length / 2) - 2;
            var val1 = signal.Length - 2;
            var val2 = signal.Length - 3;

            while (signal.Length - val2 <= pos + 1)
            {
                sum += signal[pos];
                sum -= signal[val1];
                sum -= signal[val2];

                result[pos] = sum % 10;

                pos--;
                val1 -= 2;
                val2 -= 2;
            }

            sum += signal[pos];
            sum -= signal[val1];
            sum -= signal[val2];

            var negCount = signal.Length - (((pos + 1) * 3) - 1);

            for (var i = 1; i <= negCount; i++)
            {
                sum -= signal[signal.Length - i];
            }

            result[pos] = Math.Abs(sum) % 10;

            pos--;
            val1 -= 2;
            val2 -= 2;
            var val3 = signal.Length - negCount - 1;
            var val4 = signal.Length - negCount - 2;
            var val5 = signal.Length - negCount - 3;

            while (signal.Length - val5 <= pos + 1)
            {
                sum += signal[pos];
                sum -= signal[val1];
                sum -= signal[val2];
                sum -= signal[val3];
                sum -= signal[val4];
                sum -= signal[val5];

                result[pos] = Math.Abs(sum) % 10;

                pos--;
                val1 -= 2;
                val2 -= 2;
                val3 -= 3;
                val4 -= 3;
                val5 -= 3;
            }

            //for (var i = 0; i <= pos; i++)
            //{
            //    result[i] = TransformElement2(signal, i + 1);
            //}

            return result;
        }

        private int TransformElement2(int[] signal, int position)
        {
            var sum = 0;
            var multiplier = -1;
            var stop = 0;

            while (stop < signal.Length)
            {
                multiplier += 2;
                stop = Math.Min((position * (multiplier + 1)) - 1, signal.Length);

                for (var i = (position * multiplier) - 1; i < stop; i++)
                {
                    sum += signal[i];
                }

                multiplier += 2;
                stop = Math.Min((position * (multiplier + 1)) - 1, signal.Length);

                for (var i = (position * multiplier) - 1; i < stop; i++)
                {
                    sum -= signal[i];
                }
            }

            return Math.Abs(sum) % 10;
        }
    }
}
