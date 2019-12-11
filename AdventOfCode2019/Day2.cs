using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day2
    {
        private List<int> _intCodes;

        public Day2()
        {
            LoadIntCodes();
        }

        public int ExecutePart1()
        {
            _intCodes[1] = 12;
            _intCodes[2] = 2;

            ExecuteProgram();

            return _intCodes[0];
        }

        public string ExecutePart2()
        {
            for (var i = 0; i < 100; i++)
            {
                var noun = i;

                for (var j = 0; j < 100; j++)
                {
                    var verb = j;

                    LoadIntCodes(); // Reset
                    _intCodes[1] = noun;
                    _intCodes[2] = verb;
                    // Console.WriteLine($"Trying with noun {noun} and verb {verb}");

                    try
                    {
                        ExecuteProgram();
                        // Console.WriteLine($"Value {_intCodes[0]}");
                    }
                    catch
                    {
                        // Ignore
                    }

                    if (_intCodes[0] == 19690720)
                    {
                        return $"{noun}{verb}";
                    }
                }
            }

            return "Error: Couldn't find the answer";
        }

        private void LoadIntCodes()
        {
            var file = Path.Combine(Environment.CurrentDirectory, "Inputs", "Day2.txt");
            var values = File.ReadAllLines(file)
                ?.FirstOrDefault()
                ?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (values == null)
            {
                throw new ArgumentNullException($"Couldn't read the values from the input file {file}");
            }

            _intCodes = new List<int>(values.Length);
            _intCodes.AddRange(values.Select(int.Parse));
        }

        private void ExecuteProgram()
        {
            for (var position = 0; position < _intCodes.Count;)
            {
                var intCode = _intCodes[position];

                if (intCode == 99)
                {
                    break;
                }
                else if (intCode == 1)
                {
                    Sum(_intCodes[position + 1], _intCodes[position + 2], _intCodes[position + 3]);
                    position += 4;
                }
                else if (intCode == 2)
                {
                    Multiply(_intCodes[position + 1], _intCodes[position + 2], _intCodes[position + 3]);
                    position += 4;
                }
                else
                {
                    throw new Exception($"Something went wrong. Incorrect int code {intCode}");
                }
            }
        }

        private void Sum(int positionOne, int positionTwo, int positionResult)
        {
            _intCodes[positionResult] = _intCodes[positionOne] + _intCodes[positionTwo];
        }

        private void Multiply(int positionOne, int positionTwo, int positionResult)
        {
            _intCodes[positionResult] = _intCodes[positionOne] * _intCodes[positionTwo];
        }
    }
}
