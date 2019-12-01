using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class AdventVM
    {
        private readonly List<IInstruction> _instructions = new List<IInstruction>();
        private readonly List<string> _instructionsText = new List<string>();
        private readonly List<Type> _instructionTypes = new List<Type>();

        public int IPC = 0;
        public Dictionary<string, int> Registers = new Dictionary<string, int>();

        public void Execute()
        {
            //var file = new StreamWriter(@"C:\AoC\log.txt");
            while (IPC < _instructions.Count)
            {
                //var log = $"{IPC.ToString().PadLeft(2, '0')}: {_instructionsText[IPC]}";
                _instructions[IPC++].Execute(this);
                //log += $"[{PrintRegisters()}]";

                //Debug.WriteLine(log);
                //file.WriteLine(log);
            }
        }

        private string PrintRegisters()
        {
            var result = new StringBuilder();

            foreach (var r in Registers)
            {
                result.Append($"{r.Key}:{r.Value}, ");
            }

            return result.ToString(0, result.Length - 2);
        }

        public void RegisterInstructionType(Type instructionType)
        {
            _instructionTypes.Add(instructionType);
        }

        public void ParseProgram(string program)
        {
            foreach (var line in program.Lines())
            {
                foreach (var i in _instructionTypes)
                {
                    var instance = (IInstruction)Activator.CreateInstance(i);

                    if (instance.ParseInstruction(line))
                    {
                        _instructions.Add(instance);
                        _instructionsText.Add(line);
                    }
                }
            }
        }
    }

    public interface IInstruction
    {
        bool ParseInstruction(string instruction);
        void Execute(AdventVM vm);
    }
}



//public class SetInstruction : IInstruction
//{
//    private string _setRegister = null;
//    private string _readRegister = null;
//    private int _setValue = 0;

//    public void Execute(AdventVM vm)
//    {
//        if (_readRegister != null)
//        {
//            vm.Registers[_setRegister] = vm.Registers[_readRegister];
//        }
//        else
//        {
//            vm.Registers[_setRegister] = _setValue;
//        }
//    }

//    public bool ParseInstruction(string instruction)
//    {
//        if (!instruction.StartsWith("set"))
//        {
//            return false;
//        }

//        _setRegister = instruction.Words().ElementAt(1);
//        _readRegister = instruction.Words().ElementAt(2);

//        if (int.TryParse(_readRegister, out _))
//        {
//            _setValue = int.Parse(_readRegister);
//            _readRegister = null;
//        }

//        return true;
//    }
//}

//public class SubtractInstruction : IInstruction
//{
//    private string _setRegister = null;
//    private string _subRegister = null;
//    private int _subValue = 0;

//    public void Execute(AdventVM vm)
//    {
//        if (_subRegister != null)
//        {
//            vm.Registers[_setRegister] -= vm.Registers[_subRegister];
//        }
//        else
//        {
//            vm.Registers[_setRegister] -= _subValue;
//        }
//    }

//    public bool ParseInstruction(string instruction)
//    {
//        if (!instruction.StartsWith("sub"))
//        {
//            return false;
//        }

//        _setRegister = instruction.Words().ElementAt(1);
//        _subRegister = instruction.Words().ElementAt(2);

//        if (int.TryParse(_subRegister, out _))
//        {
//            _subValue = int.Parse(_subRegister);
//            _subRegister = null;
//        }

//        return true;
//    }
//}

//public class MultiplyInstruction : IInstruction
//{
//    private string _setRegister = null;
//    private string _mulRegister = null;
//    private int _mulValue = 0;

//    public static int ExecCount = 0;

//    public void Execute(AdventVM vm)
//    {
//        if (_mulRegister != null)
//        {
//            vm.Registers[_setRegister] *= vm.Registers[_mulRegister];
//        }
//        else
//        {
//            vm.Registers[_setRegister] *= _mulValue;
//        }

//        ExecCount++;
//    }

//    public bool ParseInstruction(string instruction)
//    {
//        if (!instruction.StartsWith("mul"))
//        {
//            return false;
//        }

//        _setRegister = instruction.Words().ElementAt(1);
//        _mulRegister = instruction.Words().ElementAt(2);

//        if (int.TryParse(_mulRegister, out _))
//        {
//            _mulValue = int.Parse(_mulRegister);
//            _mulRegister = null;
//        }

//        return true;
//    }
//}

//public class JnzInstruction : IInstruction
//{
//    private string _checkRegister = null;
//    private bool _checkValue = true;
//    private string _jumpRegister = null;
//    private int _jumpValue = 0;

//    public void Execute(AdventVM vm)
//    {
//        if (!_checkValue)
//        {
//            return;
//        }

//        if (_checkRegister != null && vm.Registers[_checkRegister] == 0)
//        {
//            return;
//        }

//        var jump = _jumpValue;

//        if (_jumpRegister != null)
//        {
//            jump = vm.Registers[_jumpRegister];
//        }

//        // need to subtract 1 because IPC has already incremented
//        vm.IPC += jump - 1;
//    }

//    public bool ParseInstruction(string instruction)
//    {
//        if (!instruction.StartsWith("jnz"))
//        {
//            return false;
//        }

//        _checkRegister = instruction.Words().ElementAt(1);
//        _jumpRegister = instruction.Words().ElementAt(2);

//        if (int.TryParse(_checkRegister, out _))
//        {
//            _checkValue = !(int.Parse(_checkRegister) == 0);
//            _checkRegister = null;
//        }

//        if (int.TryParse(_jumpRegister, out _))
//        {
//            _jumpValue = int.Parse(_jumpRegister);
//            _jumpRegister = null;
//        }

//        return true;
//    }
//}