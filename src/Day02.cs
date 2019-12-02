using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day02
    {
        public static string PartOne(string input)
        {
            var program = input.Integers().ToList();

            var curIdx = 0;

            program[1] = 12;
            program[2] = 2;
            

            while (program[curIdx] != 99)
            {
                var op = program[curIdx];

                if (op == 1)
                {
                    var a = program[program[curIdx + 1]];
                    var b = program[program[curIdx + 2]];
                    var c = program[curIdx + 3];

                    program[c] = a + b;
                }

                if (op == 2)
                {
                    var a = program[program[curIdx + 1]];
                    var b = program[program[curIdx + 2]];
                    var c = program[curIdx + 3];

                    program[c] = a * b;
                }

                curIdx += 4;
            }

            return program[0].ToString();
        }

        public static string PartTwo(string input)
        {
            var program = input.Integers().ToList();
            var backup = program.Select(x => x).ToList();

            for (var x = 0; x <= 99; x++)
            {
                for (var y = 0; y <= 99; y++)
                {
                    program = backup.Select(b => b).ToList();

                    program[1] = x;
                    program[2] = y;

                    var curIdx = 0;

                    while (program[curIdx] != 99)
                    {
                        var op = program[curIdx];

                        if (op == 1)
                        {
                            var a = program[program[curIdx + 1]];
                            var b = program[program[curIdx + 2]];
                            var c = program[curIdx + 3];

                            program[c] = a + b;
                        }

                        if (op == 2)
                        {
                            var a = program[program[curIdx + 1]];
                            var b = program[program[curIdx + 2]];
                            var c = program[curIdx + 3];

                            program[c] = a * b;
                        }

                        curIdx += 4;
                    }

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