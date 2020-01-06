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

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
