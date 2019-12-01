using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventOfCode
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowResult(Func<string, string> exec, int day)
        {
            var start = Stopwatch.StartNew();
            var result = exec(GetInput(day));
            var end = start.ElapsedMilliseconds;

            Clipboard.SetText(result);

            result += Environment.NewLine;
            result += $"Elapsed: {end.ToString()}";

            MessageBox.Show(result);
        }

        private string GetInput(int day)
        {
            if (InputTextBox.Text.Trim().Length > 0)
            {
                return InputTextBox.Text;
            }

            var inputFolder = @"C:\Git\AdventOfCode2019\input\";
            var inputFileName = $"{day.ToString()}.txt";
            var inputFile = Path.Combine(inputFolder, inputFileName);

            if (!File.Exists(inputFile))
            {
                DownloadInput(day, inputFile);
            }

            return File.ReadAllText(inputFile);
        }

        private void DownloadInput(int day, string inputFile)
        {
            var url = $"https://adventofcode.com/2019/day/{day.ToString()}/input";
            var sessionCookie = File.ReadAllText(@"C:\AoC\AoCSessionCookie.txt");

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, sessionCookie);
                client.DownloadFile(url, inputFile);
            }
        }

        private void Day1AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day01.PartOne(x), 1);
        }

        private void Day1BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day01.PartTwo(x), 1);
        }

        private void Day2AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day02.PartOne(x), 2);
        }

        private void Day2BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day02.PartTwo(x), 2);
        }

        private void Day3AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day03.PartOne(x), 3);
        }

        private void Day3BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day03.PartTwo(x), 3);
        }

        private void Day4AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day04.PartOne(x), 4);
        }

        private void Day4BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day04.PartTwo(x), 4);
        }

        private void Day5AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day05.PartOne(x), 5);
        }

        private void Day5BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day05.PartTwo(x), 5);
        }

        private void Day6AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day06.PartOne(x), 6);
        }

        private void Day6BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day06.PartTwo(x), 6);
        }

        private void Day7AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day07.PartOne(x), 7);
        }

        private void Day7BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day07.PartTwo(x), 7);
        }

        private void Day8AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day08.PartOne(x), 8);
        }

        private void Day8BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day08.PartTwo(x), 8);
        }

        private void Day9AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day09.PartOne(x), 9);
        }

        private void Day9BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day09.PartTwo(x), 9);
        }

        private void Day10AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day10.PartOne(x), 10);
        }

        private void Day10BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day10.PartTwo(x), 10);
        }

        private void Day11AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day11.PartOne(x), 11);
        }

        private void Day11BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day11.PartTwo(x), 11);
        }

        private void Day12AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day12.PartOne(x), 12);
        }

        private void Day12BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day12.PartTwo(x), 12);
        }

        private void Day13AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day13.PartOne(x), 13);
        }

        private void Day13BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day13.PartTwo(x), 13);
        }

        private void Day14AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day14.PartOne(x), 14);
        }

        private void Day14BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day14.PartTwo(x), 14);
        }

        private void Day15AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day15.PartOne(x), 15);
        }

        private void Day15BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day15.PartTwo(x), 15);
        }

        private void Day16AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day16.PartOne(x),16);
        }

        private void Day16BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day16.PartTwo(x), 16);
        }

        private void Day17AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day17.PartOne(x), 17);
        }

        private void Day17BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day17.PartTwo(x), 17);
        }

        private void Day18AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day18.PartOne(x), 18);
        }

        private void Day18BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day18.PartTwo(x), 18);
        }

        private void Day19AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day19.PartOne(x), 19);
        }

        private void Day19BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day19.PartTwo(x), 19);
        }

        private void Day20AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day20.PartOne(x), 20);
        }

        private void Day20BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day20.PartTwo(x), 20);
        }

        private void Day21AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day21.PartOne(x), 21);
        }

        private void Day21BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day21.PartTwo(x), 21);
        }

        private void Day22AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day22.PartOne(x), 22);
        }

        private void Day22BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day22.PartTwo(x), 22);
        }

        private void Day23AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day23.PartOne(x), 23);
        }

        private void Day23BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day23.PartTwo(x), 23);
        }

        private void Day24AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day24.PartOne(x), 24);
        }

        private void Day24BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day24.PartTwo(x), 24);
        }

        private void Day25AButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day25.PartOne(x), 25);
        }

        private void Day25BButton_Click(object sender, EventArgs e)
        {
            ShowResult(x => Day25.PartTwo(x), 25);
        }
    }
}
