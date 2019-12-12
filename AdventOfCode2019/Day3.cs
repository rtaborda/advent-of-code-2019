using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day3
    {
        private readonly Dictionary<string, Coordinate> _wireBoard;

        private Day3(Dictionary<string, Coordinate> wireBoard)
        {
            _wireBoard = wireBoard;
        }

        public static Day3 Create()
        {
            var file = Path.Combine(Environment.CurrentDirectory, "Inputs", "Day3.txt");
            var lines = File.ReadAllLines(file);
            var wireOne = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
            var wireTwo = lines[1].Split(",", StringSplitOptions.RemoveEmptyEntries);

            var wireBoard = new Dictionary<string, Coordinate>(500000)
            {
                { "X0Y0", new Coordinate() }
            };

            AddWireCoordinates(wireBoard, wireOne, true);
            AddWireCoordinates(wireBoard, wireTwo, false);

            return new Day3(wireBoard);
        }

        public int ExecutePart1()
        {
            var firstIteration = true;
            var bestManhattanDistance = 0;
            var crossingPoints = _wireBoard.Values.Where(c => c.WireOne && c.WireTwo && c.X != 0 && c.Y != 0);

            foreach (var c in crossingPoints)
            {
                var currentManhattanDistance = CalculateManhattanDistance(0, c.X, 0, c.Y);
                if (firstIteration)
                {
                    bestManhattanDistance = currentManhattanDistance;
                    firstIteration = false;
                    continue;
                }

                if (currentManhattanDistance < bestManhattanDistance)
                {
                    bestManhattanDistance = currentManhattanDistance;
                }
            }

            return bestManhattanDistance;
        }

        private static int ParseNumberOfMovements(string coordinate)
        {
            return int.Parse(coordinate.Remove(0, 1));
        }

        private static int CalculateManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private static void AddWireCoordinates(Dictionary<string, Coordinate> wireBoard, string[] wireCoordinates, bool wireOne)
        {
            var currentCoordinate = wireBoard.First().Value;
            foreach (var wireOneCoordinate in wireCoordinates)
            {
                var numberOfMovements = ParseNumberOfMovements(wireOneCoordinate);
                var newCoordinate = new Coordinate(currentCoordinate.X, currentCoordinate.Y, wireOne, !wireOne);

                if (wireOneCoordinate.StartsWith("R"))
                {
                    newCoordinate.X += numberOfMovements;
                    var offset = newCoordinate.X - currentCoordinate.X;
                    var currentCoordinateX = newCoordinate.X;
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(--currentCoordinateX, newCoordinate.Y, wireOne, !wireOne);
                        AddOrUpdate(wireBoard, newPathCoordinate, wireOne);
                        offset = currentCoordinateX - currentCoordinate.X;
                    }
                }
                else if (wireOneCoordinate.StartsWith("L"))
                {
                    newCoordinate.X -= numberOfMovements;
                    var offset = newCoordinate.X - currentCoordinate.X;
                    var currentCoordinateX = newCoordinate.X;
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(++currentCoordinateX, newCoordinate.Y, wireOne, !wireOne);
                        AddOrUpdate(wireBoard, newPathCoordinate, wireOne);
                        offset = currentCoordinateX - currentCoordinate.X;
                    }
                }
                else if (wireOneCoordinate.StartsWith("U"))
                {
                    newCoordinate.Y += numberOfMovements;
                    var offset = newCoordinate.Y - currentCoordinate.Y;
                    var newCoordinateY = newCoordinate.Y;
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(newCoordinate.X, --newCoordinateY, wireOne, !wireOne);
                        AddOrUpdate(wireBoard, newPathCoordinate, wireOne);
                        offset = newCoordinateY - currentCoordinate.Y;
                    }
                }
                else if (wireOneCoordinate.StartsWith("D"))
                {
                    newCoordinate.Y -= numberOfMovements;
                    var offset = newCoordinate.Y - currentCoordinate.Y;
                    var newCoordinateY = newCoordinate.Y;
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(newCoordinate.X, ++newCoordinateY, wireOne, !wireOne);
                        AddOrUpdate(wireBoard, newPathCoordinate, wireOne);
                        offset = newCoordinateY - currentCoordinate.Y;
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid coordinate {wireOneCoordinate}.");
                }

                AddOrUpdate(wireBoard, newCoordinate, wireOne);
                currentCoordinate = newCoordinate;
            }
        }

        private static void AddOrUpdate(Dictionary<string, Coordinate> wireBoard, Coordinate coordinate, bool wireOne)
        {
            if (wireBoard.ContainsKey(coordinate.Key))
            {
                if (wireOne)
                {
                    wireBoard[coordinate.Key].WireOne = true;
                }
                else
                {
                    wireBoard[coordinate.Key].WireTwo = true;
                }
            }
            else
            {
                wireBoard.Add(coordinate.Key, coordinate);
            }
        }

        private class Coordinate
        {
            public int X { get; set; }

            public int Y { get; set; }

            public bool WireOne { get; set; }

            public bool WireTwo { get; set; }

            public string Key => $"X{X}Y{Y}";

            public Coordinate()
            {
                X = 0;
                Y = 0;
                WireOne = false;
                WireTwo = false;
            }

            public Coordinate(int x, int y, bool wireOne, bool wireTwo)
            {
                X = x;
                Y = y;
                WireOne = wireOne;
                WireTwo = wireTwo;
            }

            public override string ToString()
            {
                return $"X: {X}\tY: {Y}\tWireOne: {WireOne}\tWireTwo: {WireTwo}";
            }
        }
    }
}
