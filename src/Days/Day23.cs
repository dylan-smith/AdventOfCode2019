using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 23)]
    public class Day23 : BaseDay
    {
        private List<IntCodeVM> _vms = new List<IntCodeVM>();
        private long _result = 0;

        public override string PartOne(string input)
        {
            foreach (var i in Enumerable.Range(0, 50))
            {
                _vms.Add(new IntCodeVM(input)
                {
                    OutputFunction = OutputAddress,
                    InputFunction = InputAddress
                });
            }

            while (_result == 0)
            {
                _vms.ForEach(vm => vm.Run());
            }

            return _result.ToString();
        }

        private long InputAddress(IntCodeVM receiver)
        {
            receiver.InputFunction = InputX;

            Log($"Setting VM Address [{_vms.IndexOf(receiver)}]");

            return _vms.IndexOf(receiver);
        }

        private long _inputY;

        private long InputX(IntCodeVM receiver)
        {
            var address = _vms.IndexOf(receiver);

            var packet = _traffic.FirstOrDefault(t => t.address == address);

            if (packet != default)
            {
                Log($"VM [{address}] receiving packet. X = {packet.x}");
                _traffic.Remove(packet);

                _inputY = packet.y;
                receiver.InputFunction = InputY;
                return packet.x;
            }
            else
            {
                Log($"VM [{address}] receiving NULL packet");
                receiver.Halt();
                return -1;
            }
        }

        private long InputY(IntCodeVM receiver)
        {
            receiver.InputFunction = InputX;
            Log($"Receiving packet. Y = {_inputY}");

            return _inputY;
        }

        private long _outputAddress;
        private long _outputX;

        private List<(long address, long x, long y)> _traffic = new List<(long address, long x, long y)>();

        private void OutputAddress(IntCodeVM sender, long address)
        {
            _outputAddress = address;
            Log($"Sending packet to [{address}]");

            if (address == 255)
            {
                sender.OutputFunction = OutputResult;
            }
            else
            {
                sender.OutputFunction = OutputX;
            }
        }

        private void OutputResult(IntCodeVM sender, long result)
        {
            if (_result == 0)
            {
                _result = result;
            }
            else
            {
                _result = result;
                sender.Halt();
            }
        }

        private void OutputX(IntCodeVM sender, long x)
        {
            _outputX = x;
            Log($"Sending X = {x}");

            sender.OutputFunction = OutputY;
        }

        private void OutputY(IntCodeVM sender, long y)
        {
            _traffic.Add((_outputAddress, _outputX, y));
            Log($"Sending Y = {y}");

            sender.OutputFunction = OutputAddress;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }

        public class IntCodeVM
        {
            private readonly List<long> _instructions;
            private List<long> _memory;
            private Dictionary<long, long> _sparseMemory;
            private int _ip = 0;
            private List<long> _inputs = new List<long>();
            private List<long> _outputs = new List<long>();
            private long _relativeBase = 0;
            public Func<IntCodeVM, long> InputFunction { get; set; }
            public Action<IntCodeVM, long> OutputFunction { get; set; }
            private bool _halt = false;
            private bool _isSparseMemory;
            private int _memorySize;

            public IntCodeVM(string program, bool isSparseMemory = false, int memorySize = 1000000)
            {
                _instructions = program.Longs().ToList();
                _isSparseMemory = isSparseMemory;
                _memorySize = memorySize;
                Reset();
            }

            public void Reset()
            {
                if (_isSparseMemory)
                {
                    _sparseMemory = new Dictionary<long, long>();

                    for (var i = 0; i < _instructions.Count; i++)
                    {
                        _sparseMemory.Add((long)i, _instructions[i]);
                    }
                }
                else
                {
                    _memory = _instructions.Select(x => x).ToList();
                    _memory.AddMany(0, _memorySize);
                }

                _inputs = new List<long>();
                _outputs = new List<long>();

                _ip = 0;
            }

            public void AddInput(long input) => _inputs.Add(input);

            public void AddInputs(IEnumerable<long> inputs) => _inputs.AddRange(inputs);

            public void SetMemory(int address, long value)
            {
                if (_isSparseMemory)
                {
                    _sparseMemory.SafeSet(address, value);
                }
                else
                {
                    _memory[address] = value;
                }
            }

            public long GetMemory(int address)
            {
                if (_isSparseMemory)
                {
                    if (_sparseMemory.ContainsKey(address))
                    {
                        return _sparseMemory[address];
                    }

                    _sparseMemory.Add(address, 0);
                    return 0;
                }

                if (_memory.Count < (address - 1))
                {
                    var toAdd = (address - 1) - _memory.Count;
                    _memory.AddMany(0, toAdd);
                }

                return _memory[address];
            }

            public List<long> Run(params long[] inputs)
            {
                AddInputs(inputs);
                _halt = false;

                while (GetMemory(_ip) != 99 && !_halt)
                {
                    var (op, p1, p2, p3) = ParseOpCode(GetMemory(_ip));

                    _ = op switch
                    {
                        1 => Add(p1, p2, p3),
                        2 => Multiply(p1, p2, p3),
                        3 => Input(p1),
                        4 => Output(p1),
                        5 => JumpIfNotZero(p1, p2),
                        6 => JumpIfZero(p1, p2),
                        7 => LessThan(p1, p2, p3),
                        8 => EqualCheck(p1, p2, p3),
                        9 => AdjustRelativeBase(p1),
                        _ => throw new Exception($"Invalid op code [{op}]")
                    };
                }

                return _outputs;
            }

            public void Halt()
            {
                _halt = true;
            }

            private int Add(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                SetMemory(c, a + b);
                return _ip += 4;
            }

            private int Multiply(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                SetMemory(c, a * b);
                return _ip += 4;
            }

            private int Input(int p1)
            {
                var a = GetParameterAddress(1, p1);

                if (InputFunction != null)
                {
                    SetMemory(a, InputFunction(this));
                }
                else
                {
                    SetMemory(a, _inputs[0]);
                    _inputs.RemoveAt(0);
                }
                return _ip += 2;
            }

            private int Output(int p1)
            {
                var a = GetParameter(1, p1);

                if (OutputFunction != null)
                {
                    OutputFunction(this, a);
                }
                else
                {
                    _outputs.Add(a);
                }

                return _ip += 2;
            }

            private int JumpIfNotZero(int p1, int p2)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);

                _ip = a != 0 ? (int)b : _ip + 3;
                return _ip;
            }

            private int JumpIfZero(int p1, int p2)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);

                _ip = a == 0 ? (int)b : _ip + 3;
                return _ip;
            }

            private int LessThan(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                SetMemory(c, a < b ? 1 : 0);

                return _ip += 4;
            }

            private int EqualCheck(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                SetMemory(c, a == b ? 1 : 0);

                return _ip += 4;
            }

            private int AdjustRelativeBase(int p1)
            {
                var a = GetParameter(1, p1);

                _relativeBase += a;

                return _ip += 2;
            }

            private long GetParameter(int offset, int mode)
            {
                return mode switch
                {
                    0 => GetMemory((int)GetMemory(_ip + offset)),
                    1 => GetMemory(_ip + offset),
                    2 => GetMemory((int)GetMemory(_ip + offset) + (int)_relativeBase),
                    _ => throw new Exception("Invalid parameter mode")
                };
            }

            private int GetParameterAddress(int offset, int mode)
            {
                return mode switch
                {
                    0 => (int)GetMemory(_ip + offset),
                    2 => (int)(GetMemory(_ip + offset) + _relativeBase),
                    _ => throw new Exception("Invalid parameter mode")
                };
            }

            private (int op, int p1, int p2, int p3) ParseOpCode(long input)
            {
                var op = input % 100;
                var p1 = input % 1000 / 100;
                var p2 = input % 10000 / 1000;
                var p3 = input % 100000 / 10000;

                return ((int)op, (int)p1, (int)p2, (int)p3);
            }
        }
    }
}
