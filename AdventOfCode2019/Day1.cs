using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day1
    {
        private readonly List<double> _modulesMass;

        public Day1()
        {
            var lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Inputs", "Day1.txt"));
            _modulesMass = new List<double>(lines.Length);
            _modulesMass.AddRange(lines.Select(double.Parse));
        }

        public int ExecutePart1() => _modulesMass.Sum(CalculateMassFuel);

        public int ExecutePart2() => _modulesMass.Sum(CalculateFuelForFuelMass);

        private static int CalculateMassFuel(double mass) => (int)(Math.Floor(mass / 3) - 2);

        private static int CalculateFuelForFuelMass(double mass)
        {
            var moduleFuel = CalculateMassFuel(mass);
            var fuelMassFuel = moduleFuel;
            while (fuelMassFuel > 0)
            {
                fuelMassFuel = CalculateMassFuel(fuelMassFuel);
                if (fuelMassFuel > 0)
                {
                    moduleFuel += fuelMassFuel;
                }
            }

            return moduleFuel;
        }
    }
}
