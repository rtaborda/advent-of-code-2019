using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day2
    {
        private readonly List<int> _intCodes;

        public Day2()
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

        public int ExecutePart1()
        {
            _intCodes[1] = 12;
            _intCodes[2] = 2;

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

            return _intCodes[0];
        }

        public int ExecutePart2()
        {
            // output 

            return 0;
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
