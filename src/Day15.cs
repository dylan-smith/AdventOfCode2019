using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day15
    {
        public static string PartOne(string input)
        {
            var game = new Game(input);
            var score = game.Play();

            return score.ToString();
        }

        public static string PartTwo(string input)
        {
            Game game;
            var elfAP = 3;
            var score = 0L;

            do
            {
                game = new Game(input) { StopWhenElfDies = true };
                game.SetElfAttackPower(++elfAP);
                score = game.Play();
            } while (game.ElfDied);

            return score.ToString();
        }
    }

    public class Game
    {
        private GameUnit[,] _state;
        public bool ElfDied { get; set; }
        public bool StopWhenElfDies { get; set; }

        public Game(string input)
        {
            var lines = input.Lines().ToList();

            _state = new GameUnit[lines[0].Length, lines.Count];

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    _state[x, y] = new GameUnit(lines[y][x], x, y);
                }
            }

            StopWhenElfDies = false;
            ElfDied = false;
        }

        public void SetElfAttackPower(int attackPower)
        {
            GetPlayerUnits().Where(p => p.UnitType == UnitType.Elf).ForEach(e => e.AttackPower = attackPower);
        }

        public long Play()
        {
            var turns = 0;

            while (!StopWhenElfDies || !ElfDied)
            {
                turns++;
                SetUnitTurnOrder();

                foreach (var player in GetPlayerUnits().OrderBy(p => p.TurnOrder).ToList())
                {
                    if (player.HitPoints > 0)
                    {
                        if (!player.TakeTurn(_state))
                        {
                            return GetScore(turns);
                        }

                        RemoveDeadPlayers();
                    }
                }
            }

            return 0;
        }

        private void RemoveDeadPlayers()
        {
            foreach (var player in GetPlayerUnits().Where(p => p.HitPoints <= 0).ToList())
            {
                if (_state[player.Location.X, player.Location.Y].UnitType == UnitType.Elf)
                {
                    ElfDied = true;
                }

                _state[player.Location.X, player.Location.Y] = new GameUnit('.', player.Location.X, player.Location.Y);
            }
        }

        private long GetScore(int turns)
        {
            return GetPlayerUnits().Sum(p => p.HitPoints) * (turns - 1);
        }

        private List<GameUnit> GetPlayerUnits()
        {
            return _state.ToList().Where(x => x.UnitType == UnitType.Elf || x.UnitType == UnitType.Goblin).ToList();
        }

        private void SetUnitTurnOrder()
        {
            var turn = 1;

            for (var y = 0; y <= _state.GetUpperBound(1); y++)
            {
                for (var x = 0; x <= _state.GetUpperBound(0); x++)
                {
                    _state[x, y].TurnOrder = turn++;
                }
            }
        }
    }

    public class GameUnit
    {
        public UnitType UnitType { get; set; }
        public int HitPoints { get; set; }
        public int AttackPower { get; set; }
        public int TurnOrder { get; set; }
        public Point Location { get; set; }

        public GameUnit(char input, int x, int y)
        {
            switch (input)
            {
                case '#':
                    UnitType = UnitType.Wall;
                    break;
                case '.':
                    UnitType = UnitType.Empty;
                    break;
                case 'E':
                    UnitType = UnitType.Elf;
                    break;
                case 'G':
                    UnitType = UnitType.Goblin;
                    break;
            }

            Location = new Point(x, y);
            HitPoints = 200;
            AttackPower = 3;
        }

        public bool TakeTurn(GameUnit[,] state)
        {
            if (FindEnemies(state).Count() == 0)
            {
                return false;
            }

            var enemiesInRange = GetEnemiesInRange(state);

            if (!enemiesInRange.Any())
            {
                Move(state);
            }

            enemiesInRange = GetEnemiesInRange(state);

            if (enemiesInRange.Any())
            {
                Attack(enemiesInRange);
            }

            return true;
        }

        private void Attack(List<GameUnit> enemiesInRange)
        {
            var minHP = enemiesInRange.Min(e => e.HitPoints);

            var target = enemiesInRange.Where(e => e.HitPoints == minHP).WithMin(e => GetReadingOrder(e.Location));

            target.HitPoints -= AttackPower;
        }

        private List<GameUnit> GetEnemiesInRange(GameUnit[,] state)
        {
            return FindEnemies(state).Where(x => Location.ManhattanDistance(x.Location) == 1).ToList();
        }

        private void Move(GameUnit[,] state)
        {
            var inRangeSquares = GetOpenInRangeSquares(state);
            var target = GetMoveTarget(state, inRangeSquares);

            if (target.HasValue)
            {
                var nextMove = GetMoveDirection(state, target.Value);

                state[Location.X, Location.Y] = new GameUnit('.', Location.X, Location.Y);
                state[nextMove.X, nextMove.Y] = this;
                Location = nextMove;
            }
        }

        private Point GetMoveDirection(GameUnit[,] state, Point target)
        {
            var validSteps = Location.GetNeighbors(false).Where(p => state[p.X, p.Y].UnitType == UnitType.Empty).ToList();

            var distances = validSteps.Select(p => (Location: p, Distance: FindShortestDistance(state, p, target))).ToList();
            var minDistance = distances.Min(x => x.Distance);

            return distances.Where(x => x.Distance == minDistance).WithMin(x => GetReadingOrder(x.Location)).Location;
        }

        private int FindShortestDistance(GameUnit[,] state, Point start, Point target)
        {
            var seen = new HashSet<Point>();
            var steps = 0;

            if (start == target)
            {
                return 0;
            }

            var reachable = start.GetNeighbors(false).Where(p => (state[p.X, p.Y].UnitType == UnitType.Empty || state[p.X, p.Y] == this) &&
                                                                 !seen.Contains(p)).ToList();

            while (reachable.Any())
            {
                steps++;

                if (reachable.Any(p => p == target))
                {
                    return steps;
                }

                reachable.ForEach(r => seen.Add(r));

                var newReachable = new List<Point>();
                reachable.ForEach(p => newReachable.AddRange(p.GetNeighbors(false).ToList()));
                reachable = newReachable.Where(p => (state[p.X, p.Y].UnitType == UnitType.Empty || state[p.X, p.Y] == this) &&
                                                    !seen.Contains(p)).Distinct().ToList();
            }

            throw new Exception("Should never happen");
        }

        private Point? GetMoveTarget(GameUnit[,] state, List<Point> inRangeSquares)
        {
            var seen = new List<Point>();
            var steps = 0;

            var reachable = Location.GetNeighbors(false).Where(p => state[p.X, p.Y].UnitType == UnitType.Empty && !seen.Contains(p)).ToList();

            while (reachable.Any())
            {
                steps++;

                if (reachable.Any(p => inRangeSquares.Contains(p)))
                {
                    return reachable.Where(p => inRangeSquares.Contains(p)).ToList().WithMin(p => GetReadingOrder(p));
                }

                seen.AddRange(reachable);

                var newReachable = new List<Point>();
                reachable.ForEach(p => newReachable.AddRange(p.GetNeighbors(false).ToList()));
                reachable = newReachable.Distinct().Where(p => state[p.X, p.Y].UnitType == UnitType.Empty && !seen.Contains(p)).ToList();
            }

            return null;
        }

        private int GetReadingOrder(Point p)
        {
            return p.Y * 10000 + p.X;
        }

        private List<Point> GetOpenInRangeSquares(GameUnit[,] state)
        {
            var enemies = FindEnemies(state);
            var result = new List<Point>();

            enemies.ForEach(e => result.AddRange(e.Location.GetNeighbors(false).ToList()));

            return result.Where(p => state[p.X, p.Y].UnitType == UnitType.Empty).Distinct().ToList();
        }

        private List<GameUnit> FindEnemies(GameUnit[,] state)
        {
            if (UnitType == UnitType.Elf)
            {
                return state.ToList().Where(x => x.UnitType == UnitType.Goblin).ToList();
            }

            if (UnitType == UnitType.Goblin)
            {
                return state.ToList().Where(x => x.UnitType == UnitType.Elf).ToList();
            }

            throw new Exception("Should never happen");
        }
    }

    public enum UnitType
    {
        Elf,
        Goblin,
        Wall,
        Empty
    }
}