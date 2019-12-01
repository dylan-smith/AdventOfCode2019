using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day04
    {
        public static string PartOne(string input)
        {
            var logEntries = input.Lines().Select(x => GetLogEntry(x)).OrderBy(x => x.date);
            var sleeps = GetSleeps(logEntries);

            var guardWithMostSleep = sleeps.GroupBy(x => x.Key.guard).WithMax(g => g.Sum(x => x.Value)).Key;
            var maxMinute = sleeps.Where(s => s.Key.guard == guardWithMostSleep).WithMax(x => x.Value).Key.minute;

            return (guardWithMostSleep * maxMinute).ToString();
        }

        private static Dictionary<(int guard, int minute), int> GetSleeps(IEnumerable<(DateTime date, string text)> logEntries)
        {
            var sleeps = new Dictionary<(int guard, int minute), int>();
            int guard = 0, startMin = 0;

            foreach (var (date, text) in logEntries)
            {
                if (text.Contains("Guard"))
                {
                    guard = int.Parse(Regex.Match(text, @".*Guard #(\d*)").Groups[1].Value);
                }

                if (text.Contains("falls"))
                {
                    startMin = date.Minute;
                }
                
                if (text.Contains("wakes"))
                {
                    Enumerable.Range(startMin, date.Minute - startMin).ForEach(m => sleeps.SafeIncrement((guard, m)));
                }
            }

            return sleeps;
        }

        private static (DateTime date, string text) GetLogEntry(string line)
        {
            return (DateTime.Parse(Regex.Match(line, @"\[(.*)\]").Groups[1].Value), line);
        }

        public static string PartTwo(string input)
        {
            var logEntries = input.Lines().Select(x => GetLogEntry(x)).OrderBy(x => x.date);
            var sleeps = GetSleeps(logEntries);

            return (sleeps.WithMax(x => x.Value).Key.guard * sleeps.WithMax(x => x.Value).Key.minute).ToString();
        }
    }
}