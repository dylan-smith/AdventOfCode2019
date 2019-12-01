using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public class Day14
    {
        public static string PartOne(string input)
        {
            var recipes = new LinkedList<int>();
            recipes.AddFirst(3);
            recipes.AddLast(7);
            var recipeCount = 793061;
            var elf1 = recipes.First;
            var elf2 = recipes.Last;

            while (recipes.Count < recipeCount + 10)
            {
                AddNewRecipes(recipes, elf1, elf2);
                (elf1, elf2) = MoveElfs(recipes, elf1, elf2);
            }

            var curItem = GetNodeAt(recipes, recipeCount);
            var result = string.Empty;

            for (var x = 0; x < 10; x++)
            {
                result += curItem.Value.ToString();
                curItem = curItem.NextCircular();
            }

            return result;
        }

        private static (LinkedListNode<int> elf1, LinkedListNode<int> elf2) MoveElfs(LinkedList<int> recipes, LinkedListNode<int> elf1, LinkedListNode<int> elf2)
        {
            var elf1Move = elf1.Value + 1;
            for (var x = 0; x < elf1Move; x++)
            {
                elf1 = elf1.NextCircular();
            }

            var elf2Move = elf2.Value + 1;
            for (var x = 0; x < elf2Move; x++)
            {
                elf2 = elf2.NextCircular();
            }

            return (elf1, elf2);
        }

        private static void AddNewRecipes(LinkedList<int> recipes, LinkedListNode<int> elf1, LinkedListNode<int> elf2)
        {
            var sum = elf1.Value + elf2.Value;

            if (sum / 10 >= 1)
            {
                recipes.AddLast(1);
                recipes.AddLast(sum % 10);
            }
            else
            {
                recipes.AddLast(sum);
            }
        }

        private static LinkedListNode<int> GetNodeAt(LinkedList<int> recipes, int recipeCount)
        {
            var a = recipes.First;

            for (var x = 0; x < recipeCount; x++)
            {
                a = a.Next;
            }

            return a;
        }

        public static string PartTwo(string input)
        {
            var recipes = new LinkedList<int>();
            recipes.AddFirst(3);
            recipes.AddLast(7);
            var countString = "793061".ToString();
            var elf1 = recipes.First;
            var elf2 = recipes.Last;

            while (true)
            {
                var sum = elf1.Value + elf2.Value;

                if (sum / 10 >= 1)
                {
                    recipes.AddLast(1);
                    recipes.AddLast(sum % 10);

                    if (GetRecipeString(recipes, countString.Length) == countString)
                    {
                        return (recipes.Count - countString.Length).ToString();
                    }

                    if (GetRecipeString(recipes, countString.Length + 1).ShaveRight(1) == countString)
                    {
                        return (recipes.Count - countString.Length - 1).ToString();
                    }
                }
                else
                {
                    recipes.AddLast(sum);

                    if (GetRecipeString(recipes, countString.Length) == countString)
                    {
                        return (recipes.Count - countString.Length).ToString();
                    }
                }

                (elf1, elf2) = MoveElfs(recipes, elf1, elf2);
            }
        }

        private static string GetRecipeString(LinkedList<int> recipes, int length)
        {
            if (length > recipes.Count)
            {
                return "XXXXXXXXXXXXXXXXXXX";
            }

            var cur = recipes.Last;

            for (var x = 0; x < length - 1; x++)
            {
                cur = cur.Previous;
            }

            var result = string.Empty;

            while (cur != null)
            {
                result += cur.Value.ToString();
                cur = cur.Next;
            }

            return result;
        }
    }
}