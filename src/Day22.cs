using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day22
    {
        private static int _depth = 8787;
        private static Point _target = new Point(10, 725);
        private static char[,] _cave;
        private static long[,] _erosion;

        public static string PartOne(string input)
        {
            CreateCave();
            Debug.WriteLine(_cave.GetString());

            return (_cave.Count('=') + (2 * _cave.Count('|'))).ToString();
        }

        private static void CreateCave(int extra = 0)
        {
            _cave = new char[_target.X + 1 + extra, _target.Y + 1 + extra];
            _erosion = new long[_target.X + 1 + extra, _target.Y + 1 + extra];

            for (var y = 0; y < _cave.GetLength(1); y++)
            {
                for (var x = 0; x < _cave.GetLength(0); x++)
                {
                    _erosion[x, y] = GetErosionLevel(GetGeologicIndex(x, y));
                    _cave[x, y] = GetType(_erosion[x, y]);
                }
            }
        }

        private static long GetErosionLevel(long geoIndex)
        {
            return (geoIndex + _depth) % 20183L;
        }

        private static char GetType(long erosionLevel)
        {
            if (erosionLevel % 3 == 0)
            {
                return '.';
            }

            if (erosionLevel % 3 == 1)
            {
                return '=';
            }

            if (erosionLevel % 3 == 2)
            {
                return '|';
            }

            throw new Exception("should never happen");
        }

        private static long GetGeologicIndex(int x, int y)
        {
            if (x == _target.X && y == _target.Y)
            {
                return 0;
            }

            if (y == 0)
            {
                return (long)x * 16807L;
            }

            if (x == 0)
            {
                return (long)y * 48271L;
            }

            return _erosion[x - 1, y] * _erosion[x, y - 1];
        }

        public static string PartTwo(string input)
        {
            CreateCave(200);
            Debug.WriteLine(_cave.GetString());

            return GetRescueTime(new Point(0, 0), 'T').ToString();
        }

        private static long GetRescueTime(Point pos, char gear)
        {
            var seen = new Dictionary<(Point pos, char gear), long>();
            var bestTime = long.MaxValue;

            var reachable = new List<(Point pos, char gear, long time)>();
            reachable.Add((pos, gear, 0));

            while (reachable.Any())
            {
                var next = reachable.WithMin(r => r.time);
                seen.Add((next.pos, next.gear), next.time);
                reachable.Remove(next);
                reachable.RemoveAll(r => r.pos == next.pos && r.gear == next.gear && r.time >= next.time);

                var newStates = new List<(Point pos, char gear, long time)>();

                if (next.pos == _target && next.gear == 'T')
                {
                    if (next.time < bestTime)
                    {
                        bestTime = next.time;
                        reachable.RemoveAll(r => r.time >= bestTime);
                    }
                }
                else
                {
                    var neighbors = _cave.GetNeighborPoints(next.pos.X, next.pos.Y).Where(n => IsValidGear(next.gear, n.c)).ToList();
                    var gearChange = GetGearChange(_cave[next.pos.X, next.pos.Y], next.gear);

                    foreach (var n in neighbors)
                    {
                        newStates.Add((n.point, next.gear, next.time + 1));
                    }

                    newStates.Add((next.pos, gearChange, next.time + 7));
                    newStates = newStates.Where(r => r.time < bestTime).Where(r =>
                    {
                        if (seen.ContainsKey((r.pos, r.gear)))
                        {
                            return seen[(r.pos, r.gear)] > r.time;
                        }

                        return true;
                    }).ToList();
                }

                reachable.AddRange(newStates);

                //reachable = reachable.Where(r =>
                //{
                //    if (seen.ContainsKey((r.pos, r.gear)))
                //    {
                //        return seen[(r.pos, r.gear)] > r.time;
                //    }

                //    return true;
                //}).ToList();
            }

            return bestTime;
        }

        private static bool IsValidGear(char gear, char region)
        {
            if (region == '.' && (gear == 'T' || gear == 'C'))
            {
                return true;
            }

            if (region == '=' && (gear == 'C' || gear == 'N'))
            {
                return true;
            }

            if (region == '|' && (gear == 'T' || gear == 'N'))
            {
                return true;
            }

            return false;
        }

        private static char GetGearChange(char region, char gear)
        {
            if (region == '.')
            {
                if (gear == 'T')
                {
                    return 'C';
                }

                return 'T';
            }

            if (region == '=')
            {
                if (gear == 'C')
                {
                    return 'N';
                }

                return 'C';
            }

            if (region == '|')
            {
                if (gear == 'T')
                {
                    return 'N';
                }

                return 'T';
            }

            throw new Exception("Should never happen");
        }
    }
}