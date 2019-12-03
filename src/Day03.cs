using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode
{
    public class Day03
    {
public static string PartOne(string input)
{
    var aPath = input.Lines().First().Words().Select(x => ParseWirePath(x)).ToList();
    var bPath = input.Lines().Last().Words().Select(x => ParseWirePath(x)).ToList();

    var aPoints = TraceWire(aPath);
    var bPoints = TraceWire(bPath);

    var intersections = aPoints.Keys.Intersect(bPoints.Keys);

    return intersections.Min(i => i.ManhattanDistance()).ToString();
}

public static Dictionary<Point, int> TraceWire(List<(Direction dir, int length)> path)
{
    var result = new Dictionary<Point, int>();

    var pos = new Point(0, 0);
    var steps = 0;

    foreach (var p in path)
    {
        for (var i = 0; i < p.length; i++)
        {
            pos = pos.Move(p.dir);
            steps++;

            if (!result.ContainsKey(pos))
            {
                result.Add(pos, steps);
            }
        }
    }

    return result;
}

public static (Direction dir, int length) ParseWirePath(string input)
{
    var dir = Direction.Up;

    switch (input[0])
    {
        case 'R':
            dir = Direction.Right;
            break;
        case 'D':
            dir = Direction.Down;
            break;
        case 'U':
            dir = Direction.Up;
            break;
        case 'L':
            dir = Direction.Left;
            break;
    }

    var len = int.Parse(input.ShaveLeft(1));

    return (dir, len);
}

public static string PartTwo(string input)
{
    var aPath = input.Lines().First().Words().Select(x => ParseWirePath(x)).ToList();
    var bPath = input.Lines().Last().Words().Select(x => ParseWirePath(x)).ToList();

    var aPoints = TraceWire(aPath);
    var bPoints = TraceWire(bPath);

    var intersections = aPoints.Keys.Intersect(bPoints.Keys);

    return intersections.Min(i => aPoints[i] + bPoints[i]).ToString();
}
    }
}