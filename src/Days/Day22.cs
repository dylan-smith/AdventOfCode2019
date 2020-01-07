using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 22)]
    public class Day22 : BaseDay
    {
        private LinkedList<int> _deck = new LinkedList<int>();
        private Dictionary<long, List<long>> _rounds = new Dictionary<long, List<long>>();

        public override string PartOne(string input)
        {
            var shuffles = input.Lines().Select(i => GetShuffleFunction(i)).ToList();

            InitializeDeck();
            
            foreach (var shuffle in shuffles)
            {
                shuffle.shuffle(shuffle.n);
            }

            return _deck.SelectWithIndex().Single(x => x.item == 2019).index.ToString();
        }

        private void InitializeDeck()
        {
            for (var i = 0; i < 10007; i++)
            {
                _deck.AddLast(i);
            }
        }

        private (Action<int> shuffle, int n) GetShuffleFunction(string i)
        {
            if (i.StartsWith("deal with"))
            {
                return (DealIncrement, int.Parse(i.Words().Last()));
            }

            if (i.StartsWith("deal into"))
            {
                return (DealNewStack, 0);
            }

            return (CutDeck, int.Parse(i.Words().Last()));
        }

        private void CutDeck(int n)
        {
            _deck.RotateLeft(n);
        }

        private void DealNewStack(int _)
        {
            _deck = new LinkedList<int>(_deck.Reverse());
        }

        private void DealIncrement(int n)
        {
            var newDeck = new int[10007];

            var node = _deck.First;
            var pos = 0;

            while (node != null)
            {
                newDeck[pos] = node.Value;
                pos = (pos + n) % 10007;
                node = node.Next;
            }

            _deck = new LinkedList<int>(newDeck);
        }

        private long ReverseIncrement(long n, long pos, long len)
        {
            if (pos == 0)
            {
                return 0;
            }

            var mod = pos % n;
            var round = _rounds[n].IndexOf(mod);

            var prevCount = (round * len) + pos;
            var result = ((prevCount - 1) / n) + 1;

            return result;
        }

        private long ReverseCut(long n, long pos, long len)
        {
            return (pos + n + len) % len;
        }

        private long ReverseNewStack(long _, long pos, long len)
        {
            return len - pos - 1;
        }

        public override string PartTwo(string input)
        {
            var deckSize = 119315717514047;
            //var deckSize = 10007;
            var targetPos = 2020L;
            //var targetPos = 3074L;
            var shuffleCount = 101741582076661;

            var shuffles = input.Lines().Select(l => GetReverseShuffleFunction(l, deckSize)).ToList();
            var curPos = targetPos;
            var count = 0L;

            shuffles.Reverse();

            var seen = new HashSet<long>();

            while (true)
            {
                //Log($"{curPos}");
                seen.Add(curPos);
                curPos = ReverseShuffle(shuffles, curPos, deckSize);
                count++;

                if (count % 1000000 == 0)
                {
                    Log($"{count}");
                }

                //if (seen.Contains(curPos))
                //{
                //    Log("SEEN");
                //}

                if (curPos == targetPos)
                {
                    for (var i = 0; i < shuffleCount % count; i++)
                    {
                        curPos = ReverseShuffle(shuffles, curPos, deckSize);
                    }

                    return curPos.ToString();
                }
            }

            throw new Exception("Should never happen");
        }

        private long ReverseShuffle(List<(Func<long, long, long, long> func, long n)> shuffles, long startPos, long deckSize)
        {
            var curPos = startPos;

            foreach (var (func, n) in shuffles)
            {
                curPos = func(n, curPos, deckSize);
            }

            return curPos;
        }

        //private long ReverseShuffle(List<string> lines, long startPos, long deckSize)
        //{
        //    var curPos = startPos;

        //    foreach (var line in lines)
        //    {
        //        curPos = ReverseShuffle(line, curPos, deckSize);
        //    }

        //    return curPos;
        //}

        //private long ReverseShuffle(string line, long curPos, long deckSize)
        //{
        //    if (line.StartsWith("deal with increment"))
        //    {
        //        var n = int.Parse(line.Words().Last());

        //        return ReverseIncrement(n, curPos, deckSize);
        //    }

        //    if (line.StartsWith("cut"))
        //    {
        //        var n = int.Parse(line.Words().Last());

        //        return ReverseCut(n, curPos, deckSize);
        //    }

        //    if (line.StartsWith("deal into new stack"))
        //    {
        //        return ReverseNewStack(0, curPos, deckSize);
        //    }

        //    throw new ArgumentException($"Unrecognized input: {line}");
        //}

        private (Func<long, long, long, long> func, long n) GetReverseShuffleFunction(string line, long deckSize)
        {
            if (line.StartsWith("deal with increment"))
            {
                var n = long.Parse(line.Words().Last());
                CreateRoundsLookup(n, deckSize);

                return (ReverseIncrement, n);
            }

            if (line.StartsWith("deal into new stack"))
            {
                return (ReverseNewStack, 0);
            }

            if (line.StartsWith("cut"))
            {
                return (ReverseCut, long.Parse(line.Words().Last()));
            }

            throw new ArgumentException($"Unrecognized input: {line}");
        }

        private void CreateRoundsLookup(long n, long deckSize)
        {
            if (!_rounds.ContainsKey(n))
            {
                var rounds = new List<long> { 0 };

                for (var r = 1; r < n; r++)
                {
                    rounds.Add(n - ((deckSize - rounds[r - 1]) % n));
                }

                _rounds.Add(n, rounds);
            }
        }
    }
}
