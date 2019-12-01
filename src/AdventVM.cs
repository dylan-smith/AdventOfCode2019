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
