using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

            Log($"{_deck.SelectWithIndex().Single(x => x.item == 2019).index}");

            foreach (var shuffle in shuffles)
            {
                shuffle.shuffle(shuffle.n);
                Log($"{_deck.SelectWithIndex().Single(x => x.item == 2019).index}");
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

        private long InverseMod(long n, long mod)
        {
            return (long)BigInteger.ModPow(n, mod - 2, mod);
        }

        private (BigInteger A, BigInteger B) ReverseIncrement(long n, long len, BigInteger A, BigInteger B)
        {
            var inverse = InverseMod(n, len);

            return (A * inverse, B * inverse);
        }

        private (BigInteger A, BigInteger B) ReverseCut(long n, long len, BigInteger A, BigInteger B)
        {
            return (A, B + n);
        }

        private (BigInteger A, BigInteger B) ReverseNewStack(long _, long len, BigInteger A, BigInteger B)
        {
            return (A * -1, B * -1 + len - 1);
        }

        public override string PartTwo(string input)
        {
            var deckSize = 119315717514047;
            var targetPos = 2020L;
            var shuffleCount = 101741582076661;

            var shuffles = input.Lines().Select(l => GetReverseShuffleFunction(l, deckSize)).ToList();

            shuffles.Reverse();

            var (a, b) = ReverseShuffle(shuffles, deckSize);
            var (newa, newb) = RepeatShuffle(a, b, shuffleCount, deckSize);

            return ((newa * targetPos + newb) % deckSize).ToString();
        }

        private (BigInteger A, BigInteger B) RepeatShuffle(BigInteger A, BigInteger B, BigInteger count, BigInteger deckSize)
        {
            var a = BigInteger.ModPow(A, count, deckSize);
            var b = B * (a - 1) * BigInteger.ModPow(A - 1, deckSize - 2, deckSize);

            return (a, b);
        }

        private (BigInteger A, BigInteger B) ReverseShuffle(List<(Func<long, long, BigInteger, BigInteger, (BigInteger A, BigInteger B)> func, long n)> shuffles, long deckSize)
        {
            var a = (BigInteger)1;
            var b = (BigInteger)0;

            foreach (var (func, n) in shuffles)
            {
                (a, b) = func(n, deckSize, a, b);
            }

            return (a, b);
        }

        private (Func<long, long, BigInteger, BigInteger, (BigInteger A, BigInteger B)> func, long n) GetReverseShuffleFunction(string line, long deckSize)
        {
            if (line.StartsWith("deal with increment"))
            {
                return (ReverseIncrement, long.Parse(line.Words().Last()));
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
    }
}
