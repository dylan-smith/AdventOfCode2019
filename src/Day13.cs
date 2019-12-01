using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day13
    {
        public static string PartOne(string input)
        {
            var (tracks, carts) = ParseInput(input);

            while (true)
            {
                for (var c = 0; c < carts.Count; c++)
                {
                    carts[c] = MoveCart(carts[c], tracks);

                    if (IsCrash(carts))
                    {
                        return $"{GetCrash(carts).X},{GetCrash(carts).Y}";
                    }
                }
            }
        }

        private static (char[,] tracks, List<(Point location, char direction, int turns)> carts) ParseInput(string input)
        {
            var lines = input.Lines().ToList();
            var tracks = new char[lines.Max(x => x.Length), lines.Count];
            var carts = new List<(Point location, char direction, int turns)>();

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '<' || lines[y][x] == '>')
                    {
                        carts.Add((new Point(x, y), lines[y][x], 0));
                        tracks[x, y] = '-';
                    }
                    else if (lines[y][x] == '^' || lines[y][x] == 'v')
                    {
                        carts.Add((new Point(x, y), lines[y][x], 0));
                        tracks[x, y] = '|';
                    }
                    else
                    {
                        tracks[x, y] = lines[y][x];
                    }
                }
            }

            return (tracks, carts);
        }

        private static bool IsCrash(List<(Point location, char direction, int turns)> carts)
        {
            return carts.GroupBy(c => c.location).Any(g => g.Count() > 1);
        }

        private static Point GetCrash(List<(Point location, char direction, int turns)> carts)
        {
            return carts.GroupBy(c => c.location).First(g => g.Count() > 1).First().location;
        }

        private static (Point location, char direction, int turns) MoveCart((Point location, char direction, int turns) cart, char[,] tracks)
        {
            if (cart.direction == '<')
            {
                if (tracks[cart.location.X, cart.location.Y] == '-')
                {
                    return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '\\')
                {
                    return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '/')
                {
                    return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '+')
                {
                    if (cart.turns % 3 == 0)
                    {
                        return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 1)
                    {
                        return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 2)
                    {
                        return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns + 1);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            if (cart.direction == '>')
            {
                if (tracks[cart.location.X, cart.location.Y] == '-')
                {
                    return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '\\')
                {
                    return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '/')
                {
                    return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '+')
                {
                    if (cart.turns % 3 == 0)
                    {
                        return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 1)
                    {
                        return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 2)
                    {
                        return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns + 1);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            if (cart.direction == '^')
            {
                if (tracks[cart.location.X, cart.location.Y] == '|')
                {
                    return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '\\')
                {
                    return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '/')
                {
                    return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '+')
                {
                    if (cart.turns % 3 == 0)
                    {
                        return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 1)
                    {
                        return (new Point(cart.location.X, cart.location.Y - 1), '^', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 2)
                    {
                        return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns + 1);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            if (cart.direction == 'v')
            {
                if (tracks[cart.location.X, cart.location.Y] == '|')
                {
                    return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '\\')
                {
                    return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '/')
                {
                    return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns);
                }
                else if (tracks[cart.location.X, cart.location.Y] == '+')
                {
                    if (cart.turns % 3 == 0)
                    {
                        return (new Point(cart.location.X + 1, cart.location.Y), '>', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 1)
                    {
                        return (new Point(cart.location.X, cart.location.Y + 1), 'v', cart.turns + 1);
                    }
                    else if (cart.turns % 3 == 2)
                    {
                        return (new Point(cart.location.X - 1, cart.location.Y), '<', cart.turns + 1);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            throw new Exception();
        }

        public static string PartTwo(string input)
        {
            var (tracks, carts) = ParseInput(input);

            var cartsMoved = new List<int>();
            carts = carts.OrderBy(c => c.location.Y * 10000 + c.location.X).ToList();
            var cartsWithIndex = carts.SelectWithIndex().ToList();

            while (true)
            {
                if (cartsWithIndex.Any(c => !cartsMoved.Any(m => m == c.index)))
                {
                    var cart = cartsWithIndex.First(c => !cartsMoved.Any(m => m == c.index));

                    cart.item = MoveCart(cart.item, tracks);
                    cartsMoved.Add(cart.index);
                    cartsWithIndex.RemoveAll(c => c.index == cart.index);
                    cartsWithIndex.Add(cart);

                    if (cartsWithIndex.Count == 1)
                    {
                        return $"{cart.item.location.X},{cart.item.location.Y}";
                    }

                    if (IsCrash(cartsWithIndex.Select(c => c.item).ToList()))
                    {
                        var crashLocation = GetCrash(cartsWithIndex.Select(c => c.item).ToList());
                        cartsWithIndex.RemoveAll(c => c.item.location == crashLocation);
                    }
                }
                else
                {
                    cartsWithIndex = cartsWithIndex.OrderBy(c => c.item.location.Y * 10000 + c.item.location.X).ToList();
                    cartsMoved.Clear();
                }
            }
        }
    }
}