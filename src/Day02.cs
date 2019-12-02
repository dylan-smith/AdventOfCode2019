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

                if (op == 1)
                {
                    var a = program[program[ipc + 1]];
                    var b = program[program[ipc + 2]];
                    var c = program[ipc + 3];

                    program[c] = a + b;
                }

                if (op == 2)
                {
                    var a = program[program[ipc + 1]];
                    var b = program[program[ipc + 2]];
                    var c = program[ipc + 3];

                    program[c] = a * b;
                }

                ipc += 4;
            }

            return program[0];
        }

        public static string PartTwo(string input)
        {
            var backup = input.Integers().ToList();

            for (var x = 0; x <= 99; x++)
            {
                for (var y = 0; y <= 99; y++)
                {
                    var program = backup.Select(b => b).ToList();

                    program[1] = x;
                    program[2] = y;

                    RunProgram(program);

                    if (program[0] == 19690720)
                    {
                        return (100 * x + y).ToString();
                    }
                }
            }

            throw new Exception();
        }
    }
}