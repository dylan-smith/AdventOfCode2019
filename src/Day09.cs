using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day09
    {
        public static string PartOne(string input)
        {
            var players = int.Parse(input.Words().ToList()[0]);
            var lastMarble = int.Parse(input.Words().ToList()[6]);

            return MarbleMania(players, lastMarble).ToString();
        }

        public static string PartTwo(string input)
        {
            var players = int.Parse(input.Words().ToList()[0]);
            var lastMarble = int.Parse(input.Words().ToList()[6]) * 100;

            return MarbleMania(players, lastMarble).ToString();
        }

        private static long MarbleMania(int players, int marbles)
        {
            var playerScores = new long[players];

            var marbleList = new LinkedList<int>();
            var curMarble = marbleList.AddFirst(0);
            var nextMarbleValue = 1;

            foreach (var player in Enumerable.Range(0, players).Cycle())
            {
                if (nextMarbleValue % 23 == 0)
                {
                    playerScores[player] += nextMarbleValue++;
                    var removeMarble = curMarble.PreviousCircular(7);
                    curMarble = removeMarble.NextCircular();
                    playerScores[player] += removeMarble.Value;
                    marbleList.Remove(removeMarble);
                }
                else
                {
                    curMarble = marbleList.AddAfter(curMarble.NextCircular(), nextMarbleValue++);
                }

                if (nextMarbleValue > marbles)
                {
                    return playerScores.Max();
                }
            }

            throw new Exception("Should never reach this point");
        }
    }
}