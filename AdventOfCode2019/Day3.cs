using System;
using System.Collections.Generic;
using System.Dynamic;
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

            var firstCoordinate = new Coordinate
            {
                OrderCableOne = 0,
                OrderCableTwo = 0
            };

            var wireBoard = new Dictionary<string, Coordinate>(500000)
            {
                { firstCoordinate.Key, firstCoordinate }
            };

            AddWireCoordinates2(wireBoard, wireOne, true);
            AddWireCoordinates2(wireBoard, wireTwo, false);

            return new Day3(wireBoard);
        }

        public int ExecutePart1()
        {
            var firstIteration = true;
            var bestManhattanDistance = 0;
            var crossingPoints = _wireBoard.Values.Where(c => c.WireOne && c.WireTwo && !(c.X == 0 && c.Y == 0));

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

        public int ExecutePart2()
        {
            var crossingPoints = _wireBoard.Values
                .Where(c => c.WireOne && c.WireTwo && !(c.X == 0 && c.Y == 0))
                .ToList();

            var wireOnePoints = _wireBoard.Values
                .Where(c => c.WireOne)
                .OrderBy(c => c.OrderCableOne)
                .ToList();

            var wireTwoPoints = _wireBoard.Values
                .Where(c => c.WireTwo)
                .OrderBy(c => c.OrderCableTwo)
                .ToList();

            var steps = crossingPoints.ToDictionary(k => k.Key, v => 0);

            foreach (var point in crossingPoints)
            {
                foreach (var wireOnePoint in wireOnePoints)
                {
                    if (point.Key == wireOnePoint.Key)
                    {
                        break;
                    }

                    steps[point.Key] = steps[point.Key] + 1;
                }

                foreach (var wireTwoPoint in wireTwoPoints)
                {
                    if (point.Key == wireTwoPoint.Key)
                    {
                        break;
                    }

                    steps[point.Key] = steps[point.Key] + 1;
                }
            }

            return steps
                .Min(kvp => kvp.Value);
        }

        private static int ParseNumberOfMovements(string coordinate)
        {
            return int.Parse(coordinate.Remove(0, 1));
        }

        private static int CalculateManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private static void AddWireCoordinates2(Dictionary<string, Coordinate> wireBoard, string[] wireCoordinates, bool wireOne)
        {
            var currentCoordinate = wireBoard.First().Value;
            var counter = 0;
            foreach (var wireOneCoordinate in wireCoordinates)
            {
                var numberOfMovements = ParseNumberOfMovements(wireOneCoordinate);
                var newCoordinate = new Coordinate(currentCoordinate.X, currentCoordinate.Y, wireOne, !wireOne);

                if (wireOneCoordinate.StartsWith("R"))
                {
                    newCoordinate.X += numberOfMovements;
                    var offset = newCoordinate.X - currentCoordinate.X;
                    var currentCoordinateX = newCoordinate.X;
                    var toAdd = new List<Coordinate>(Math.Abs(offset));
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(--currentCoordinateX, newCoordinate.Y, wireOne, !wireOne);
                        toAdd.Add(newPathCoordinate);
                        offset = currentCoordinateX - currentCoordinate.X;
                    }

                    toAdd.Reverse();
                    toAdd.ForEach(c =>
                    {
                        if (wireOne)
                        {
                            c.OrderCableOne = ++counter;
                        }
                        else
                        {
                            c.OrderCableTwo = ++counter;
                        }
                        AddOrUpdate(wireBoard, c, wireOne, counter);
                    });
                }
                else if (wireOneCoordinate.StartsWith("L"))
                {
                    newCoordinate.X -= numberOfMovements;
                    var offset = newCoordinate.X - currentCoordinate.X;
                    var currentCoordinateX = newCoordinate.X;
                    var toAdd = new List<Coordinate>(Math.Abs(offset));
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(++currentCoordinateX, newCoordinate.Y, wireOne, !wireOne);
                        toAdd.Add(newPathCoordinate);
                        offset = currentCoordinateX - currentCoordinate.X;
                    }

                    toAdd.Reverse();
                    toAdd.ForEach(c =>
                    {
                        if (wireOne)
                        {
                            c.OrderCableOne = ++counter;
                        }
                        else
                        {
                            c.OrderCableTwo = ++counter;
                        }
                        AddOrUpdate(wireBoard, c, wireOne, counter);
                    });
                }
                else if (wireOneCoordinate.StartsWith("U"))
                {
                    newCoordinate.Y += numberOfMovements;
                    var offset = newCoordinate.Y - currentCoordinate.Y;
                    var newCoordinateY = newCoordinate.Y;
                    var toAdd = new List<Coordinate>(Math.Abs(offset));
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(newCoordinate.X, --newCoordinateY, wireOne, !wireOne);
                        toAdd.Add(newPathCoordinate);
                        offset = newCoordinateY - currentCoordinate.Y;
                    }

                    toAdd.Reverse();
                    toAdd.ForEach(c =>
                    {
                        if (wireOne)
                        {
                            c.OrderCableOne = ++counter;
                        }
                        else
                        {
                            c.OrderCableTwo = ++counter;
                        }
                        AddOrUpdate(wireBoard, c, wireOne, counter);
                    });
                }
                else if (wireOneCoordinate.StartsWith("D"))
                {
                    newCoordinate.Y -= numberOfMovements;
                    var offset = newCoordinate.Y - currentCoordinate.Y;
                    var newCoordinateY = newCoordinate.Y;
                    var toAdd = new List<Coordinate>(Math.Abs(offset));
                    while (offset != 0)
                    {
                        var newPathCoordinate = new Coordinate(newCoordinate.X, ++newCoordinateY, wireOne, !wireOne);
                        toAdd.Add(newPathCoordinate);
                        offset = newCoordinateY - currentCoordinate.Y;
                    }

                    toAdd.Reverse();
                    toAdd.ForEach(c =>
                    {
                        if (wireOne)
                        {
                            c.OrderCableOne = ++counter;
                        }
                        else
                        {
                            c.OrderCableTwo = ++counter;
                        }
                        AddOrUpdate(wireBoard, c, wireOne, counter);
                    });
                }
                else
                {
                    throw new ArgumentException($"Invalid coordinate {wireOneCoordinate}.");
                }

                if (wireOne)
                {
                    newCoordinate.OrderCableOne = ++counter;
                }
                else
                {
                    newCoordinate.OrderCableTwo = ++counter;
                }

                AddOrUpdate(wireBoard, newCoordinate, wireOne, counter);
                currentCoordinate = newCoordinate;
            }
        }

        private static void AddOrUpdate(Dictionary<string, Coordinate> wireBoard, Coordinate coordinate, bool wireOne, int counter)
        {
            if (wireBoard.ContainsKey(coordinate.Key))
            {
                if (wireOne)
                {
                    wireBoard[coordinate.Key].WireOne = true;
                    wireBoard[coordinate.Key].OrderCableOne = counter;
                }
                else
                {
                    wireBoard[coordinate.Key].WireTwo = true;
                    wireBoard[coordinate.Key].OrderCableTwo = counter;
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

            public int OrderCableOne { get; set; }

            public int OrderCableTwo { get; set; }

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
