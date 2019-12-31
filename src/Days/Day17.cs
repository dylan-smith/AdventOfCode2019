using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 17)]
    public class Day17 : BaseDay
    {
        private string _mapString = string.Empty;
        private char[,] _map;
        private Point _startPos;
        private Direction _startDir;
        private List<Point> _intersections;
        private long _completeCheck = 0;

        public override string PartOne(string input)
        {
            var vm = new IntCodeVM(input);

            vm.OutputFunction = Output;

            vm.Run();

            _map = _mapString.CreateCharGrid();

            var intersections = _map.GetPoints().Where(p => _map[p.X, p.Y] == '#')
                                                .Where(p => _map.GetNeighbors(p.X, p.Y, false)
                                                                .All(c => c == '#'))
                                               .ToList();

            var result = intersections.Sum(i => i.X * i.Y);

            return result.ToString();
        }

        private Direction ToDirection(char c)
        {
            return c switch
            {
                '^' => Direction.Up,
                '>' => Direction.Right,
                '<' => Direction.Left,
                'v' => Direction.Down,
                _ => throw new ArgumentException($"Invalid direction: {c}")
            };
        }

        private void Output(long value)
        {
            _mapString += ((char)value).ToString();
        }

        public override string PartTwo(string input)
        {
            var vm = new IntCodeVM(input);

            vm.OutputFunction = Output;
            vm.Run();

            _map = _mapString.CreateCharGrid();
            _startPos = _map.GetPoints().First(p => _map[p.X, p.Y] != '.' && _map[p.X, p.Y] != '#');
            _startDir = ToDirection(_map[_startPos.X, _startPos.Y]);
            _map[_startPos.X, _startPos.Y] = '#';

            _intersections = _map.GetPoints().Where(p => _map[p.X, p.Y] == '#')
                                                .Where(p => _map.GetNeighbors(p.X, p.Y, false)
                                                                .Count(c => c == '#') >= 3)
                                               .ToList();

            var possible = _map.GetPoints().Where(p => _map[p.X, p.Y] == '#').Where(p => _map.GetNeighbors(p.X, p.Y, false).Count(c => c == '#') == 2);

            foreach (var i in possible)
            {
                var neighbors = _map.GetNeighborPoints(i.X, i.Y).Where(p => p.c == '#').ToList();

                if (neighbors.First().point.X != neighbors.Last().point.X && neighbors.First().point.Y != neighbors.Last().point.Y)
                {
                    _intersections.Add(i);
                }
            }

            FindMovementLogic("A,", "R,", string.Empty, string.Empty, _startPos, _startDir.TurnRight());

            return "foo";
        }

        private void FindMovementLogic(string routine, string a, string b, string c, Point pos, Direction dir)
        {
            if (routine.ShaveRight(",").Length > 20 || a.ShaveRight(",").Length > 20 || b.ShaveRight(",").Length > 20 || c.ShaveRight(",").Length > 20)
            {
                return;
            }

            // TODO: Check if this is complete and if so do something
            if (IsComplete(routine, a, b, c))
            {
                Log("Complete!");
            }

            var aDone = routine.Length > 2;
            var bDone = routine.IndexOf('B') != routine.Length - 2;
            var cDone = routine.IndexOf('C') != routine.Length - 2;
            var allDone = aDone && bDone && cDone;

            var curFunc = (!aDone) ? a : (!bDone) ? b : (!cDone) ? c : null;

            if (!allDone && curFunc.ShaveRight(",").Length < 20)
            {
                var forwardMoves = GetForwardMoves(pos, dir).ToList();

                foreach (var m in forwardMoves)
                {
                    var newFunc = curFunc + m.ToString() + ",";
                    var (newA, newB, newC) = GetNewFunc(aDone, bDone, cDone, a, b, c, newFunc);
                    FindMovementLogic(routine, newA, newB, newC, pos.Move(dir, m), dir);
                }

                if (CanTurnRight(pos, dir, curFunc))
                {
                    var newFunc = curFunc + "R,";
                    var (newA, newB, newC) = GetNewFunc(aDone, bDone, cDone, a, b, c, newFunc);
                    FindMovementLogic(routine, newA, newB, newC, pos, dir.TurnRight());
                }

                if (CanTurnLeft(pos, dir, curFunc))
                {
                    var newFunc = curFunc + "L,";
                    var (newA, newB, newC) = GetNewFunc(aDone, bDone, cDone, a, b, c, newFunc);
                    FindMovementLogic(routine, newA, newB, newC, pos, dir.TurnLeft());
                }
            }

            if (routine.ShaveRight(",").Length < 20)
            {
                var (newPos, newDir, valid) = IsValid(pos, dir, a);

                if (valid)
                {
                    var newRoutine = routine + "A,";
                    FindMovementLogic(newRoutine, a, b, c, newPos, newDir);
                }

                (newPos, newDir, valid) = IsValid(pos, dir, b);

                if (valid)
                {
                    var newRoutine = routine + "B,";
                    FindMovementLogic(newRoutine, a, b, c, newPos, newDir);
                }

                (newPos, newDir, valid) = IsValid(pos, dir, c);

                if (valid)
                {
                    var newRoutine = routine + "C,";
                    FindMovementLogic(newRoutine, a, b, c, newPos, newDir);
                }
            }
        }

        private (string a, string b, string c) GetNewFunc(bool aDone, bool bDone, bool cDone, string a, string b, string c, string newFunc)
        {
            if (!aDone)
            {
                return (newFunc, b, c);
            }

            if (!bDone)
            {
                return (a, newFunc, c);
            }

            if (!cDone)
            {
                return (a, b, newFunc);
            }

            throw new Exception("Should never happen");
        }

        private bool IsComplete(string routine, string a, string b, string c)
        {
            var pos = _startPos;
            var dir = _startDir;
            var points = new List<Point>();
            var r = routine.Split(",", StringSplitOptions.RemoveEmptyEntries);

            _completeCheck++;

            if (_completeCheck % 100000 == 0)
            {
                Log($"{_completeCheck} - {routine}");
            }

            for (var i = 0; i < r.Length; i++)
            {
                var moves = (r[i] == "A" ? a : r[i] == "B" ? b : c).Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var m in moves)
                {
                    switch (m)
                    {
                        case "R":
                            dir = dir.TurnRight();
                            break;
                        case "L":
                            dir = dir.TurnLeft();
                            break;
                        default:
                            var mLen = int.Parse(m);
                            for (var x = 0; x < mLen; x++)
                            {
                                pos = pos.Move(dir);
                                points.Add(pos);

                                if (_map[pos.X, pos.Y] != '#')
                                {
                                    throw new Exception("Should never happen");
                                }
                            }
                            break;
                    }
                }
            }

            return !_map.GetPoints('#').Any(p => !points.Contains(p));
        }

        private (Point pos, Direction dir, bool valid) IsValid(Point pos, Direction dir, string moves)
        {
            foreach (var m in moves.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                switch (m)
                {
                    case "R":
                        dir = dir.TurnRight();
                        break;
                    case "L":
                        dir = dir.TurnLeft();
                        break;
                    default:
                        for (var i = 0; i < int.Parse(m); i++)
                        {
                            pos = pos.Move(dir);
                            if (!_map.IsValidPoint(pos) || _map[pos.X, pos.Y] != '#')
                            {
                                return (pos, dir, false);
                            }
                        }
                        break;
                }
            }

            return (pos, dir, true);
        }

        private bool CanTurnLeft(Point pos, Direction dir, string moves)
        {
            var newDir = dir.TurnLeft();
            var newPos = pos.Move(newDir);

            if (!_map.IsValidPoint(newPos)) return false;

            if (_map[newPos.X, newPos.Y] != '#') return false;

            if (!moves.Any()) return true;

            if (moves[moves.Length - 2] == 'R') return false;

            if (moves.Length >= 4 && moves[moves.Length - 2] == 'L' && moves[moves.Length - 4] == 'L') return false;

            return true;
        }

        private bool CanTurnRight(Point pos, Direction dir, string moves)
        {
            var newDir = dir.TurnRight();
            var newPos = pos.Move(newDir);

            if (!_map.IsValidPoint(newPos)) return false;

            if (_map[newPos.X, newPos.Y] != '#') return false;

            if (!moves.Any()) return true;

            if (moves[moves.Length - 2] == 'L') return false;

            if (moves.Length >= 4 && moves[moves.Length - 2] == 'R' && moves[moves.Length - 4] == 'R') return false;

            return true;
        }

        private IEnumerable<int> GetForwardMoves(Point pos, Direction dir)
        {
            var result = 0;
            var newPos = pos.Move(dir);

            while (_map.IsValidPoint(newPos) && _map[newPos.X, newPos.Y] == '#')
            {
                result++;

                if (_intersections.Contains(newPos))
                {
                    yield return result;
                }

                newPos = newPos.Move(dir);
            }

            // TODO: Optimize by moving logic to intersection list
            if (result > 0)
            {

            }

            //if (!_map.IsValidPoint(newPos) || _map[newPos.X, newPos.Y] != '#')
            //{
            //    yield return result;
            //}
        }

        private (Point pos, Direction dir) ApplyMoves(string routine, string a, string b, string c)
        {
            var pos = _startPos;
            var dir = _startDir;

            foreach (var r in routine)
            {
                var func = r == 'A' ? a : r == 'B' ? b : c;

                foreach (var m in func)
                {
                    switch (m)
                    {
                        case 'R':
                            dir = dir.TurnRight();
                            break;
                        case 'L':
                            dir = dir.TurnLeft();
                            break;
                        default:
                            pos = pos.Move(dir, (int)m);
                            break;
                    }
                }
            }

            return (pos, dir);
        }

        //public override string PartTwo(string input)
        //{
        //    var vm = new IntCodeVM(input);

        //    vm.OutputFunction = Output;

        //    vm.Run();

        //    _map = _mapString.CreateCharGrid();
        //    _startPos = _map.GetPoints().First(p => _map[p.X, p.Y] != '.' && _map[p.X, p.Y] != '#');
        //    _startDir = ToDirection(_map[_startPos.X, _startPos.Y]);
        //    _map[_startPos.X, _startPos.Y] = '#';

        //    var logic = FindMovementLogic();

        //    throw new NotImplementedException();
        //}

        //private MovementLogic FindMovementLogic()
        //{
        //    var routines = FindAllRoutines(10);

        //    foreach (var r in routines)
        //    {
        //        var logic = FindMovementLogic(r);

        //        if (logic.HasValue)
        //        {
        //            return logic.Value;
        //        }
        //    }

        //    throw new Exception("No valid Movement Logic found");
        //}

        //private MovementLogic? FindMovementLogic(List<char> r)
        //{
        //    Log($"ROUTINE: {FuncToString(r)}");
        //    var validA = FindAllValidA(r, new List<char>(), _startPos, _startDir);

        //    foreach (var a in validA)
        //    {
        //        //Log($"A: {FuncToString(a)}");

        //        if (r.Contains('B'))
        //        {
        //            var pos = _startPos;
        //            var dir = _startDir;

        //            var x = 0;

        //            while (x < r.Count && r[x] == 'A')
        //            {
        //                (pos, dir) = ApplyMoves(pos, dir, a);
        //                x++;
        //            }

        //            var validB = FindAllValidB(r, a, new List<char>(), pos, dir);

        //            foreach (var b in validB)
        //            {
        //                //Log($"B: {FuncToString(b)}");
        //                if (r.Contains('C'))
        //                {
        //                    pos = _startPos;
        //                    dir = _startDir;

        //                    x = 0;

        //                    while (x < r.Count && r[x] != 'C')
        //                    {
        //                        (pos, dir) = ApplyMoves(pos, dir, r[x] == 'A' ? a : b);
        //                        x++;
        //                    }

        //                    var validC = FindAllValidC(r, a, b, new List<char>(), pos, dir);

        //                    if (validC.Any(c => IsComplete(r, a, b, c)))
        //                    {
        //                        var result = new MovementLogic();
        //                        result.Routine = r;
        //                        result.A = a;
        //                        result.B = b;
        //                        result.C = validC.First(c => IsComplete(r, a, b, c));

        //                        return result;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}

        //private string FuncToString(List<char> r)
        //{
        //    var result = string.Empty;

        //    foreach (var c in r)
        //    {
        //        result += c switch
        //        {
        //            'A' => "A ",
        //            'B' => "B ",
        //            'C' => "C ",
        //            'R' => "R ",
        //            'L' => "L ",
        //            _ => ((int)c).ToString() + " "
        //        };
        //    }

        //    return result;
        //}

        //private bool IsComplete(List<char> r, List<char> a, List<char> b, List<char> c)
        //{
        //    var pos = _startPos;
        //    var dir = _startDir;
        //    var points = new List<Point>();

        //    for (var i = 0; i < r.Count; i++)
        //    {
        //        var moves = r[i] == 'A' ? a : r[i] == 'B' ? b : c;

        //        foreach (var m in moves)
        //        {
        //            switch (m)
        //            {
        //                case 'R':
        //                    dir = dir.TurnRight();
        //                    break;
        //                case 'L':
        //                    dir = dir.TurnLeft();
        //                    break;
        //                default:
        //                    for (var x = 0; x < (int)m; x++)
        //                    {
        //                        pos = pos.Move(dir);
        //                        points.Add(pos);

        //                        if (_map[pos.X, pos.Y] != '#')
        //                        {
        //                            throw new Exception("Should never happen");
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //    }

        //    return !_map.GetPoints('#').Any(p => !points.Contains(p));
        //}

        //private List<List<char>> FindAllValidA(List<char> r, List<char> a, Point pos, Direction dir)
        //{
        //    var result = new List<List<char>>() { a };

        //    if (a.Count == 10)
        //    {
        //        return result;
        //    }

        //    //var (pos, dir) = ApplyMoves(_startPos, _startDir, a);

        //    if (CanTurnRight(pos,dir, a))
        //    {
        //        var newA = a.Select(x => x).ToList();
        //        newA.Add('R');
        //        result.AddRange(FindAllValidA(r, newA, pos, dir.TurnRight()));
        //    }

        //    if (CanTurnLeft(pos, dir, a))
        //    {
        //        var newA = a.Select(x => x).ToList();
        //        newA.Add('L');
        //        result.AddRange(FindAllValidA(r, newA, pos, dir.TurnLeft()));
        //    }

        //    if (a.Any() && (a.Last() == 'R' || a.Last() == 'L'))
        //    {
        //        var forwardMoves = GetForwardMoves(pos, dir);
        //        for (var i = 1; i <= forwardMoves; i++)
        //        {
        //            var newA = a.Select(x => x).ToList();
        //            newA.Add((char)(i));
        //            result.AddRange(FindAllValidA(r, newA, pos.Move(dir, i), dir));
        //        }
        //    }

        //    return result.Where(x => IsValidA(r, x)).ToList();
        //}

        //private List<List<char>> FindAllValidB(List<char> r, List<char> a, List<char> b, Point pos, Direction dir)
        //{
        //    var result = new List<List<char>>() { b };

        //    if (b.Count == 10)
        //    {
        //        return result;
        //    }

        //    //var pos = _startPos;
        //    //var dir = _startDir;

        //    //var x = 0;

        //    //while (x < r.Count && r[x] == 'A')
        //    //{
        //    //    (pos, dir) = ApplyMoves(pos, dir, a);
        //    //    x++;
        //    //}

        //    //(pos, dir) = ApplyMoves(pos, dir, b);

        //    if (CanTurnRight(pos, dir, b))
        //    {
        //        var newB = b.Select(x => x).ToList();
        //        newB.Add('R');
        //        result.AddRange(FindAllValidB(r, a, newB, pos, dir.TurnRight()));
        //    }

        //    if (CanTurnLeft(pos, dir, b))
        //    {
        //        var newB = b.Select(x => x).ToList();
        //        newB.Add('L');
        //        result.AddRange(FindAllValidB(r, a, newB, pos, dir.TurnLeft()));
        //    }

        //    if (b.Any() && (b.Last() == 'R' || b.Last() == 'L'))
        //    {
        //        var forwardMoves = GetForwardMoves(pos, dir);
        //        for (var i = 1; i <= forwardMoves; i++)
        //        {
        //            var newB = b.Select(x => x).ToList();
        //            newB.Add((char)(i));
        //            result.AddRange(FindAllValidB(r, a, newB, pos.Move(dir, i), dir));
        //        }
        //    }

        //    return result.Where(x => IsValidB(r, a, x)).ToList();
        //}

        //private List<List<char>> FindAllValidC(List<char> r, List<char> a, List<char> b, List<char> c, Point pos, Direction dir)
        //{
        //    var result = new List<List<char>>() { c };

        //    if (c.Count == 10)
        //    {
        //        return result;
        //    }

        //    //var pos = _startPos;
        //    //var dir = _startDir;

        //    //var x = 0;

        //    //while (x < r.Count && r[x] != 'C')
        //    //{
        //    //    (pos, dir) = ApplyMoves(pos, dir, r[x] == 'A' ? a : b);
        //    //    x++;
        //    //}

        //    //(pos, dir) = ApplyMoves(pos, dir, c);

        //    if (CanTurnRight(pos, dir, c))
        //    {
        //        var newC = c.Select(x => x).ToList();
        //        newC.Add('R');
        //        result.AddRange(FindAllValidC(r, a, b, newC, pos, dir.TurnRight()));
        //    }

        //    if (CanTurnLeft(pos, dir, c))
        //    {
        //        var newC = c.Select(x => x).ToList();
        //        newC.Add('L');
        //        result.AddRange(FindAllValidC(r, a, b, newC, pos, dir.TurnLeft()));
        //    }

        //    if (c.Any() && (c.Last() == 'R' || c.Last() == 'L'))
        //    {
        //        var forwardMoves = GetForwardMoves(pos, dir);
        //        for (var i = 1; i <= forwardMoves; i++)
        //        {
        //            var newC = c.Select(x => x).ToList();
        //            newC.Add((char)(i));
        //            result.AddRange(FindAllValidC(r, a, b, newC, pos.Move(dir, i), dir));
        //        }
        //    }

        //    return result.Where(x => IsValidC(r, a, b, x)).ToList();
        //}

        //private int GetForwardMoves(Point pos, Direction dir)
        //{
        //    var result = 0;
        //    var newPos = pos.Move(dir);

        //    while (_map.IsValidPoint(newPos) && _map[newPos.X, newPos.Y] == '#')
        //    {
        //        newPos = newPos.Move(dir);
        //        result++;
        //    }

        //    return result;
        //}

        //private bool IsValidA(List<char> routine, List<char> a)
        //{
        //    var pos = _startPos;
        //    var dir = _startDir;
        //    bool valid;
        //    var i = 0;

        //    while (i < routine.Count && routine[i] == 'A')
        //    {
        //        (pos, dir, valid) = ValidateMoves(pos, dir, a);
        //        if (!valid) return false;
        //        i++;
        //    }

        //    return true;
        //}

        //private bool IsValidB(List<char> routine, List<char> a, List<char> b)
        //{
        //    var pos = _startPos;
        //    var dir = _startDir;
        //    bool valid;
        //    var i = 0;

        //    while (i < routine.Count && routine[i] != 'C')
        //    {
        //        (pos, dir, valid) = ValidateMoves(pos, dir, routine[i] == 'A' ? a : b);
        //        if (!valid) return false;
        //        i++;
        //    }

        //    return true;
        //}

        //private bool IsValidC(List<char> routine, List<char> a, List<char> b, List<char> c)
        //{
        //    var pos = _startPos;
        //    var dir = _startDir;
        //    bool valid;

        //    for (var i = 0; i < routine.Count; i++)
        //    {
        //        (pos, dir, valid) = ValidateMoves(pos, dir, routine[i] == 'A' ? a : routine[i] == 'B' ? b : c);
        //        if (!valid) return false;
        //    }

        //    return true;
        //}

        //private bool CanTurnRight(Point pos, Direction dir, List<char> moves)
        //{
        //    var newDir = dir.TurnRight();
        //    var newPos = pos.Move(newDir);

        //    if (!_map.IsValidPoint(newPos)) return false;

        //    if (_map[newPos.X, newPos.Y] != '#') return false;

        //    if (!moves.Any()) return true;

        //    if (moves.Last() == 'L') return false;

        //    if (moves.Count >= 2 && moves.Last() == 'R' && moves[moves.Count - 2] == 'R') return false;

        //    return true;
        //}

        //private bool CanTurnLeft(Point pos, Direction dir, List<char> moves)
        //{
        //    var newDir = dir.TurnLeft();
        //    var newPos = pos.Move(newDir);

        //    if (!_map.IsValidPoint(newPos)) return false;

        //    if (_map[newPos.X, newPos.Y] != '#') return false;

        //    if (!moves.Any()) return true;

        //    if (moves.Last() == 'R') return false;

        //    if (moves.Count >= 2 && moves.Last() == 'L' && moves[moves.Count - 2] == 'L') return false;

        //    return true;
        //}

        //private (Point pos, Direction dir) ApplyMoves(Point startPos, Direction startDir, List<char> moves)
        //{
        //    var pos = startPos;
        //    var dir = startDir;

        //    foreach (var m in moves)
        //    {
        //        switch (m)
        //        {
        //            case 'R':
        //                dir = dir.TurnRight();
        //                break;
        //            case 'L':
        //                dir = dir.TurnLeft();
        //                break;
        //            default:
        //                pos = pos.Move(dir, (int)m);
        //                break;
        //        }
        //    }

        //    return (pos, dir);
        //}

        //private (Point pos, Direction dir, bool valid) ValidateMoves(Point startPos, Direction startDir, List<char> moves)
        //{
        //    var pos = startPos;
        //    var dir = startDir;
        //    var valid = true;

        //    foreach (var m in moves)
        //    {
        //        switch (m)
        //        {
        //            case 'R':
        //                dir = dir.TurnRight();
        //                break;
        //            case 'L':
        //                dir = dir.TurnLeft();
        //                break;
        //            default:
        //                for (var i = 0; i < (int)m; i++)
        //                {
        //                    pos = pos.Move(dir);
        //                    if (!_map.IsValidPoint(pos) || _map[pos.X, pos.Y] != '#')
        //                    {
        //                        valid = false;
        //                    }
        //                }
        //                break;
        //        }
        //    }

        //    return (pos, dir, valid);
        //}

        //private List<List<char>> FindAllRoutines(int maxLength)
        //{
        //    var result = new List<List<char>>();

        //    for (var l = 1; l <= maxLength; l++)
        //    {
        //        result.AddRange(FindAllRoutines(new List<char>(), l));
        //    }

        //    return result;
        //}

        //private List<List<char>> FindAllRoutines(List<char> start, int length)
        //{
        //    var result = new List<List<char>>();

        //    if (start.Count == length)
        //    {
        //        result.Add(start);
        //    }
        //    else
        //    {
        //        var temp = start.Select(x => x).ToList();
        //        temp.Add('A');
        //        result.AddRange(FindAllRoutines(temp, length));

        //        if (start.Any(c => c == 'A'))
        //        {
        //            temp = start.Select(x => x).ToList();
        //            temp.Add('B');
        //            result.AddRange(FindAllRoutines(temp, length));
        //        }

        //        if (start.Any(c => c == 'A') && start.Any(c => c == 'B'))
        //        {
        //            temp = start.Select(x => x).ToList();
        //            temp.Add('C');
        //            result.AddRange(FindAllRoutines(temp, length));
        //        }
        //    }

        //    return result;
        //}

        private struct MovementLogic
        {
            public List<char> Routine;
            public List<char> A;
            public List<char> B;
            public List<char> C;
        }

        public class IntCodeVM
        {
            private readonly List<long> _instructions;
            private List<long> _memory;
            private int _ip = 0;
            private List<long> _inputs = new List<long>();
            private long _relativeBase = 0;
            public Func<long> InputFunction { get; set; }
            public Action<long> OutputFunction { get; set; }
            private bool _halt = false;

            public IntCodeVM(string program)
            {
                _instructions = program.Longs().ToList();
                Reset();
            }

            public void Reset()
            {
                _memory = _instructions.Select(x => x).ToList();
                _memory.AddMany(0, 1000000);
                _ip = 0;
            }

            public void AddInput(long input) => _inputs.Add(input);

            public void AddInputs(IEnumerable<long> inputs) => _inputs.AddRange(inputs);

            public void SetMemory(int address, long value) => _memory[address] = value;

            public long GetMemory(int address)
            {
                if (_memory.Count < (address - 1))
                {
                    var toAdd = (address - 1) - _memory.Count;
                    _memory.AddMany(0, toAdd);
                }

                return _memory[address];
            }

            public void Run(params long[] inputs)
            {
                AddInputs(inputs);

                while (_memory[_ip] != 99 && !_halt)
                {
                    var (op, p1, p2, p3) = ParseOpCode(_memory[_ip]);

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

                _memory[c] = a + b;
                return _ip += 4;
            }

            private int Multiply(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                _memory[c] = a * b;
                return _ip += 4;
            }

            private int Input(int p1)
            {
                var a = GetParameterAddress(1, p1);

                _memory[a] = InputFunction();
                return _ip += 2;
            }

            private int Output(int p1)
            {
                var a = GetParameter(1, p1);

                OutputFunction(a);

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

                _memory[c] = a < b ? 1 : 0;

                return _ip += 4;
            }

            private int EqualCheck(int p1, int p2, int p3)
            {
                var a = GetParameter(1, p1);
                var b = GetParameter(2, p2);
                var c = GetParameterAddress(3, p3);

                _memory[c] = a == b ? 1 : 0;

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
                    0 => _memory[(int)_memory[_ip + offset]],
                    1 => _memory[_ip + offset],
                    2 => _memory[(int)(_memory[_ip + offset] + _relativeBase)],
                    _ => throw new Exception("Invalid parameter mode")
                };
            }

            private int GetParameterAddress(int offset, int mode)
            {
                return mode switch
                {
                    0 => (int)_memory[_ip + offset],
                    2 => (int)(_memory[_ip + offset] + _relativeBase),
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
