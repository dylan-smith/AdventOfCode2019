using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day16
    {
        public static string PartOne(string input)
        {
            var samples = GetSamples(input).ToList();
            return samples.Count(s => HowManyInstructionsMatch(s.before, s.command, s.after) >= 3).ToString();
        }

        private static int HowManyInstructionsMatch(int[] beforeRegisters, int[] instruction, int[] afterRegisters)
        {
            return CreateInstructions().Count(i => DoesInstructionMatchSample(i, beforeRegisters, instruction, afterRegisters));
        }

        private static bool DoesInstructionMatchSample(IOpCode i, int[] beforeRegisters, int[] instruction, int[] afterRegisters)
        {
            var beforeCopy = new int[] { beforeRegisters[0], beforeRegisters[1], beforeRegisters[2], beforeRegisters[3] };

            var result = i.Execute(beforeCopy, instruction[1], instruction[2], instruction[3]);
            return afterRegisters.SequenceEqual(result);
        }

        private static IEnumerable<IOpCode> CreateInstructions()
        {
            yield return new Addr();
            yield return new Addi();
            yield return new Mulr();
            yield return new Muli();
            yield return new Banr();
            yield return new Bani();
            yield return new Borr();
            yield return new Bori();
            yield return new Setr();
            yield return new Seti();
            yield return new Gtir();
            yield return new Gtri();
            yield return new Gtrr();
            yield return new Eqir();
            yield return new Eqri();
            yield return new Eqrr();
        }

        public static string PartTwo(string input)
        {
            var samples = GetSamples(input);
            var allInstructions = CreateInstructions().ToList();
            var possibleNumbers = FindPossibleNumbers(allInstructions, samples);

            while (possibleNumbers.Count > 0)
            {
                var found = possibleNumbers.First(x => x.Value.Count == 1);
                found.Key.OpNumber = found.Value[0];
                possibleNumbers.Remove(found.Key);

                possibleNumbers.ForEach(p => p.Value.Remove(found.Value[0]));
            }

            var program = GetProgram(input);

            var registers = new int[4];

            foreach (var command in program)
            {
                var (i, a, b, c) = command.Words().Select(int.Parse).ToArray();

                allInstructions.Single(x => x.OpNumber == i).Execute(registers, a, b, c);
            }

            return registers[0].ToString();
        }

        private static Dictionary<IOpCode, List<int>> FindPossibleNumbers(List<IOpCode> allInstructions, IEnumerable<(int[] before, int[] command, int[] after)> samples)
        {
            var result = new Dictionary<IOpCode, List<int>>();

            foreach (var i in allInstructions)
            {
                result.Add(i, new List<int>());
                Enumerable.Range(0, 16).ForEach(x => result[i].Add(x));

                foreach (var (before, command, after) in samples.Where(s => result[i].Contains(s.command[0])).ToList())
                {
                    if (!DoesInstructionMatchSample(i, before, command, after))
                    {
                        result[i].Remove(command[0]);
                    }
                }
            }

            return result;
        }

        private static IEnumerable<(int[] before, int[] command, int[] after)> GetSamples(string input)
        {
            var endOfSamples = input.Lines().SelectWithIndex().Last(x => x.item.StartsWith("After")).index;
            var lines = input.Lines().Take(endOfSamples + 1).ToList();

            for (var s = 0; s < lines.Count / 3; s++)
            {
                var beforeRegisters = lines[s * 3].Substring(9).ShaveRight(1).Words().Select(x => int.Parse(x)).ToArray();
                var instruction = lines[s * 3 + 1].Integers().ToArray();
                var afterRegisters = lines[s * 3 + 2].Substring(9).ShaveRight(1).Words().Select(x => int.Parse(x)).ToArray();

                yield return (beforeRegisters, instruction, afterRegisters);
            }
        }

        private static IEnumerable<string> GetProgram(string input)
        {
            var endOfSamples = input.Lines().SelectWithIndex().Last(x => x.item.StartsWith("After")).index;
            return input.Lines().Skip(endOfSamples + 1);
        }
    }

    public interface IOpCode
    {
        int[] Execute(int[] registers, int a, int b, int c);
        int OpNumber { get; set; }
    }

    public class Addr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + registers[b];
            return registers;
        }
    }

    public class Addi : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + b;
            return registers;
        }
    }

    public class Mulr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * registers[b];
            return registers;
        }
    }

    public class Muli : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * b;
            return registers;
        }
    }

    public class Banr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & registers[b];
            return registers;
        }
    }

    public class Bani : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & b;
            return registers;
        }
    }

    public class Borr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | registers[b];
            return registers;
        }
    }

    public class Bori : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | b;
            return registers;
        }
    }

    public class Setr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a];
            return registers;
        }
    }

    public class Seti : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = a;
            return registers;
        }
    }

    public class Gtir : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = a > registers[b] ? 1 : 0;
            return registers;
        }
    }

    public class Gtri : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > b ? 1 : 0;
            return registers;
        }
    }

    public class Gtrr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > registers[b] ? 1 : 0;
            return registers;
        }
    }

    public class Eqir : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = a == registers[b] ? 1 : 0;
            return registers;
        }
    }

    public class Eqri : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == b ? 1 : 0;
            return registers;
        }
    }

    public class Eqrr : IOpCode
    {
        public int OpNumber { get; set; }

        public int[] Execute(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == registers[b] ? 1 : 0;
            return registers;
        }
    }
}