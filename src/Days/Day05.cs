using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 5)]
    public class Day05 : BaseDay
    {
        public override string PartOne(string input)
        {
            var vm = new IntCodeVM(input);

            return vm.Run(1).Last().ToString();
        }

        public override string PartTwo(string input)
        {
            var vm = new IntCodeVM(input);

            return vm.Run(5).Last().ToString();
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
                    var (op, p1, p2, p3) = ParseOpCode(_memory[_ip]);

                    int a;
                    int b;
                    int c;

                    switch (op)
                    {
                        case 1: // add
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            _memory[c] = a + b;
                            _ip += 4;
                            break;
                        case 2: // mul
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            _memory[c] = a * b;
                            _ip += 4;
                            break;
                        case 3: // input
                            a = _memory[_ip + 1];

                            _memory[a] = _inputs.First();
                            _inputs.RemoveAt(0);
                            _ip += 2;
                            break;
                        case 4: // output
                            a = GetParameter(_memory[_ip + 1], p1);

                            _outputs.Add(a);
                            _ip += 2;
                            break;
                        case 5: // jump if not 0
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);

                            if (a != 0)
                            {
                                _ip = b;
                            }
                            else
                            {
                                _ip += 3;
                            }

                            break;
                        case 6: // jump if 0
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);

                            if (a == 0)
                            {
                                _ip = b;
                            }
                            else
                            {
                                _ip += 3;
                            }

                            break;
                        case 7: // less than
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            if (a < b)
                            {
                                _memory[c] = 1;
                            }
                            else
                            {
                                _memory[c] = 0;
                            }

                            _ip += 4;
                            break;
                        case 8: // equal
                            a = GetParameter(_memory[_ip + 1], p1);
                            b = GetParameter(_memory[_ip + 2], p2);
                            c = _memory[_ip + 3];

                            if (a == b)
                            {
                                _memory[c] = 1;
                            }
                            else
                            {
                                _memory[c] = 0;
                            }

                            _ip += 4;
                            break;
                        default:
                            throw new Exception($"Invalid op code [{op}]");
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
                var op = input % 100;
                var p1 = input % 1000 / 100;
                var p2 = input % 10000 / 1000;
                var p3 = input % 100000 / 10000;

                return (op, p1, p2, p3);
            }
        }
    }
}
