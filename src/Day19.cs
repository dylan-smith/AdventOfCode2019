using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day19
    {
        static int _ipRegister = 2;

        public static string PartOne(string input)
        {
            long[] registers = new long[6];
            RunProgram(registers, ParseProgram(input).ToList());

            return registers[0].ToString();
        }

        private static void RunProgram(long[] registers, List<(string opCode, int a, int b, int c)> program)
        {
            var halted = false;

            while (!halted)
            {
                if (registers[_ipRegister] >= program.Count)
                {
                    halted = true;
                }
                else
                {
                    if (registers[_ipRegister] == 3)
                    {
                        if (registers[1] % registers[5] == 0)
                        {
                            registers[3] = registers[1];
                            registers[4] = 1;
                            registers[_ipRegister] = 7;
                        }
                        else
                        {
                            registers[3] = registers[1] + 1;
                            registers[4] = 1;
                            registers[_ipRegister] = 12;
                        }
                    }

                    var instruction = program[(int)registers[_ipRegister]];
                    ExecuteInstruction(registers, instruction.opCode, instruction.a, instruction.b, instruction.c);
                    registers[_ipRegister]++;
                }
            }
        }

        private static IEnumerable<(string opCode, int a, int b, int c)> ParseProgram(string input)
        {
            var lines = input.Lines().Skip(1).ToList();

            foreach (var line in lines)
            {
                var instruction = line.Words().ToList();

                yield return (instruction[0], int.Parse(instruction[1]), int.Parse(instruction[2]), int.Parse(instruction[3]));
            }
        }

        private static void ExecuteInstruction(long[] registers, string opCode, int a, int b, int c)
        {
            switch (opCode)
            {
                case "addi":
                    registers[c] = registers[a] + b;
                    break;
                case "seti":
                    registers[c] = a;
                    break;
                case "mulr":
                    registers[c] = registers[a] * registers[b];
                    break;
                case "eqrr":
                    registers[c] = registers[a] == registers[b] ? 1 : 0;
                    break;
                case "addr":
                    registers[c] = registers[a] + registers[b];
                    break;
                case "gtrr":
                    registers[c] = registers[a] > registers[b] ? 1 : 0;
                    break;
                case "muli":
                    registers[c] = registers[a] * b;
                    break;
                case "setr":
                    registers[c] = registers[a];
                    break;
                default:
                    throw new Exception("should never happen");
            }
        }

        public static string PartTwo(string input)
        {
            long[] registers = new long[6];
            registers[0] = 1;

            RunProgram(registers, ParseProgram(input).ToList());

            return registers[0].ToString();
        }
    }
}