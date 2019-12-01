using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day18
    {
        public static string PartOne(string input)
        {
            var map = input.CreateCharGrid();
            Enumerable.Range(0, 10).ForEach(x => map = EvolveMap(map));
            return (map.Count('#') * map.Count('|')).ToString();
        }

        private static char[,] EvolveMap(char[,] map)
        {
            return map.Clone((x, y, c) => GetNewMapSquare(map, x, y));
        }

        private static char GetNewMapSquare(char[,] map, int x, int y)
        {
            if (map[x, y] == '.' && map.GetNeighbors(x, y).Count(m => m == '|') >= 3)
            {
                return '|';
            }

            if (map[x, y] == '|' && map.GetNeighbors(x, y).Count(m => m == '#') >= 3)
            {
                return '#';
            }

            if (map[x, y] == '#' && map.GetNeighbors(x, y).Any(m => m == '#') && map.GetNeighbors(x, y).Any(m => m == '|'))
            {
                return '#';
            }

            if (map[x, y] == '#')
            {
                return '.';
            }

            return map[x, y];
        }

        public static string PartTwo(string input)
        {
            var map = input.CreateCharGrid();
            var seen = new Dictionary<string, int>();
            var count = 0;
            var mapString = map.GetString();

            while (!seen.Any(x => x.Key == mapString))
            {
                seen.Add(mapString, count++);
                map = EvolveMap(map);
                mapString = map.GetString();
            }

            var cycleStart = seen.First(x => x.Key == mapString).Value;
            var cycleLength = count - cycleStart;
            var answerCount = (1000000000 - cycleStart) % cycleLength + cycleStart;

            return (seen.First(x => x.Value == answerCount).Key.Count(m => m == '#') * seen.First(x => x.Value == answerCount).Key.Count(m => m == '|')).ToString();
        }
    }
}