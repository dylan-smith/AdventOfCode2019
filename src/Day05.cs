using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day05
    {
        public static string PartOne(string input)
        {
            var vm = new IntCodeVM(input);

            return vm.Run(1).Last().ToString();
        }

        public static string PartTwo(string input)
        {
            return "";
        }

        public class IntCodeVM
        {
            private readonly List<int> _instructions;
            private List<int> _memory;
            private int _ip = 0;
            private List<int> _inputs;
            private List<int> _outputs = new List<int>();

            public IntCodeVM(string program)
            {
                _instructions = program.Integers().ToList();
                _memory = _instructions.Select(x => x).ToList();
            }

            public void Reset()
            {
                _memory = _instructions.Select(x => x).ToList();
                _ip = 0;
            }

            public void SetMemory(int address, int value) => _memory[address] = value;

            public IEnumerable<int> Run(params int[] inputs)
            {
                _inputs = inputs.ToList();

                while (_memory[_ip] != 99)
                {
                    var op = _memory[_ip];

                    var (opcode, p1, p2, p3) = ParseOpCode(_memory[_ip]);

                    int a;
                    int b;
                    int c;

                    switch (opcode)
                    {
                        case 1:
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            _memory[c] = a + b;
                            _ip += 4;
                            break;
                        case 2:
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            _memory[c] = a * b;
                            _ip += 4;
                            break;
                        case 3:
                            a = _memory[_ip + 1];

                            _memory[a] = _inputs.First();
                            _inputs.RemoveAt(0);
                            _ip += 2;
                            break;
                        case 4:
                            a = GetParameter(_memory[_ip + 1], p1);

                            _outputs.Add(a);
                            _ip += 2;
                            break;
                        default:
                            throw new Exception($"Invalid op code [{opcode}]");
                    }
                }

                return _outputs;
            }

            private int GetParameter(int value, int mode)
            {
                if (mode == 0)
                {
                    return _memory[value];
                }

                return value;
            }

            private (int op, int p1, int p2, int p3) ParseOpCode(int input)
            {
                var text = input.ToString().PadLeft(5, '0');

                var p3 = int.Parse(text[0].ToString());
                var p2 = int.Parse(text[1].ToString());
                var p1 = int.Parse(text[2].ToString());
                var op = int.Parse(text.ShaveLeft(3));

                return (op, p1, p2, p3);
            }
        }
    }
}