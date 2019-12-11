using System;

namespace AdventOfCode2019
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var day1 = new Day1();
            Console.WriteLine($"Day 1 Part 1: {day1.ExecutePart1()}");
            Console.WriteLine($"Day 1 Part 1: {day1.ExecutePart2()}");

            Console.ReadKey();
        }
    }
}
