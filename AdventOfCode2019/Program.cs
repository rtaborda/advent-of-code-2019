using System;

namespace AdventOfCode2019
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var day1 = Day1.Create();
            Console.WriteLine($"Day 1 Part 1: {day1.ExecutePart1()}");
            Console.WriteLine($"Day 1 Part 2: {day1.ExecutePart2()}");

            var day2 = Day2.Create();
            Console.WriteLine($"Day 2 Part 1: {day2.ExecutePart1()}");
            Console.WriteLine($"Day 2 Part 2: {day2.ExecutePart2()}");

            var day3 = Day3.Create();
            Console.WriteLine($"Day 3 Part 1: {day3.ExecutePart1()}");

            Console.ReadKey();
        }
    }
}
