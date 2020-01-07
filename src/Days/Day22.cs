using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 22)]
    public class Day22 : BaseDay
    {
        private LinkedList<int> _deck = new LinkedList<int>();

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

            var rounds = new List<long> { 0 };

            for (var r = 1; r < n; r++)
            {
                rounds.Add(n - ((len - rounds[r - 1]) % n));
            }

            var mod = pos % n;
            var round = rounds.IndexOf(mod);

            var prevCount = (round * len) + pos;
            var result = ((prevCount - 1) / n) + 1;

            return result;
        }

        private long ReverseCut(long n, long pos, long len)
        {
            return (pos + n + len) % len;
        }

        private long ReverseNewStack(long pos, long len)
        {
            return len - pos - 1;
        }

        public override string PartTwo(string input)
        {
            var lines = input.Lines().ToList();
            var deckSize = 10007L;
            var targetPos = 3074L;

            lines.Reverse();

            var curPos = ReverseShuffle(lines, targetPos, deckSize);

            return curPos.ToString();
        }

        private long ReverseShuffle(List<string> lines, long startPos, long deckSize)
        {
            var curPos = startPos;

            foreach (var line in lines)
            {
                curPos = ReverseShuffle(line, curPos, deckSize);
            }

            return curPos;
        }

        private long ReverseShuffle(string line, long curPos, long deckSize)
        {
            if (line.StartsWith("deal with increment"))
            {
                var n = int.Parse(line.Words().Last());

                return ReverseIncrement(n, curPos, deckSize);
            }

            if (line.StartsWith("cut"))
            {
                var n = int.Parse(line.Words().Last());

                return ReverseCut(n, curPos, deckSize);
            }

            if (line.StartsWith("deal into new stack"))
            {
                return ReverseNewStack(curPos, deckSize);
            }

            throw new ArgumentException($"Unrecognized input: {line}");
        }
    }
}
