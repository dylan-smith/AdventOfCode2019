using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day02
    {
        public static string PartOne(string input)
        {
            var program = input.Integers().ToList();

            program[1] = 12;
            program[2] = 2;

            return RunProgram(program).ToString();
        }

        private static int RunProgram(List<int> program)
        {
            var ipc = 0;

            while (program[ipc] != 99)
            {
                var op = program[ipc];
                var a = program[program[ipc + 1]];
                var b = program[program[ipc + 2]];
                var c = program[ipc + 3];

                switch (op)
                {
                    case 1:
                        program[c] = a + b;
                        break;
                    case 2:
                        program[c] = a * b;
                        break;
                    default:
                        throw new Exception($"Invalid op code [{op}]");
                }

                ipc += 4;
            }

            return program[0];
        }

        public static string PartTwo(string input)
        {
            var backup = input.Integers().ToList();

            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var program = backup.Select(b => b).ToList();

                    program[1] = noun;
                    program[2] = verb;

                    RunProgram(program);

                    if (program[0] == 19690720)
                    {
                        return (100 * noun + verb).ToString();
                    }
                }
            }

            throw new Exception();
        }
    }
}