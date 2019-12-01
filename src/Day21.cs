using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    public class Day21
    {
        static int _ipRegister = 2;

        public static string PartOne(string input)
        {
            var registers = new long[6];

            return RunProgram(registers, ParseProgram(input).ToList()).ToString();
        }

        public static string PartTwo(string input)
        {
            var registers = new long[6] { -1, 0, 0, 0, 0, 0 };

            return RunProgram(registers, ParseProgram(input).ToList(), true).ToString();
        }

        private static long RunProgram(long[] registers, List<(string opCode, long a, long b, long c)> program, bool partTwo = false)
        {
            var halted = false;
            var seenValues = new HashSet<long>();

            while (!halted)
            {
                if (registers[_ipRegister] >= program.Count)
                {
                    halted = true;
                }
                else
                {
                    if (registers[_ipRegister] == 18)
                    {
                        registers[3] = registers[1] / 256;
                        registers[5] = 1;
                        registers[_ipRegister] = 26;
                    }

                    if (registers[_ipRegister] == 28)
                    {
                        if (seenValues.Add(registers[4]))
                        {
                            Debug.WriteLine($"{seenValues.Count} - {registers[4]}");
                        }
                        else
                        {
                            return seenValues.Last();
                        }

                        if (!partTwo)
                        {
                            return registers[4];
                        }
                    }

                    var instruction = program[(int)registers[_ipRegister]];
                    ExecuteInstruction(registers, instruction.opCode, instruction.a, instruction.b, instruction.c);
                    registers[_ipRegister]++;
                }
            }

            throw new Exception("Should never happen");
        }

        private static IEnumerable<(string opCode, long a, long b, long c)> ParseProgram(string input)
        {
            var lines = input.Lines().Skip(1).ToList();

            foreach (var line in lines)
            {
                var instruction = line.Words().ToList();

                yield return (instruction[0], long.Parse(instruction[1]), long.Parse(instruction[2]), long.Parse(instruction[3]));
            }
        }

        private static void ExecuteInstruction(long[] registers, string opCode, long a, long b, long c)
        {
            switch (opCode)
            {
                case "addi":
                    registers[c] = registers[a] + b;
                    break;
                case "addr":
                    registers[c] = registers[a] + registers[b];
                    break;
                case "seti":
                    registers[c] = a;
                    break;
                case "setr":
                    registers[c] = registers[a];
                    break;
                case "mulr":
                    registers[c] = registers[a] * registers[b];
                    break;
                case "muli":
                    registers[c] = registers[a] * b;
                    break;
                case "eqrr":
                    registers[c] = registers[a] == registers[b] ? 1 : 0;
                    break;
                case "eqri":
                    registers[c] = registers[a] == b ? 1 : 0;
                    break;
                case "eqir":
                    registers[c] = a == registers[b] ? 1 : 0;
                    break;
                case "gtrr":
                    registers[c] = registers[a] > registers[b] ? 1 : 0;
                    break;
                case "gtir":
                    registers[c] = a > registers[b] ? 1 : 0;
                    break;
                case "gtri":
                    registers[c] = registers[a] > b ? 1 : 0;
                    break;
                case "bani":
                    registers[c] = registers[a] & b;
                    break;
                case "banr":
                    registers[c] = registers[a] & registers[b];
                    break;
                case "borr":
                    registers[c] = registers[a] | registers[b];
                    break;
                case "bori":
                    registers[c] = registers[a] | b;
                    break;
                default:
                    throw new Exception("should never happen");
            }
        }
    }
}