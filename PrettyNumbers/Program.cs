using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Interview
{
    // TODO refactor to specify class
    class Program
    {
        private static StringBuilder _numberHalf;

        private static ImmutableDictionary<char, int> _notation;

        private static char _notationMinValue = '0';
        private static char _notationMaxValue;

        private static void Main(string[] args)
        {
            Init();

            var sw = new Stopwatch();
            sw.Start();

            var sumDict = new Dictionary<int, int>();

            // Collecting counts for every sum
            do
            {
                var sum = CalcPartSum(_numberHalf.ToString());

                if (!sumDict.ContainsKey(sum))
                {
                    sumDict.Add(sum, 1);
                }
                else
                {
                    sumDict[sum]++;
                }

            } while (!IncrementNumber(_numberHalf));

            // Dont forget max value
            sumDict.Add(CalcPartSum(_numberHalf.ToString()), 1);

            // Counting all unique combinations for every sum value (by pow^2) and then multiplying them to notation base because of middle number
            var prettyNumbersCount = sumDict.Values.Select(sumCount => Math.Pow(sumCount, 2) * _notation.Count).Sum();

            sw.Stop();

            Console.WriteLine($"Pretty Numbers Count: {prettyNumbersCount} \nDone in:{sw.ElapsedMilliseconds}ms");
        }

        private static void Init()
        {
            // use only half of the number (current lenght is 6 for overall 13 numbers)
            // TODO use args to config and generate number length
            _numberHalf = new StringBuilder("000000");

            // 13 is task base
            // TODO use args
            _notation = CreateNotation(13);

            _notationMaxValue = _notation.OrderByDescending(kv => kv.Value).First().Key;
        }

        private static ImmutableDictionary<char, int> CreateNotation(int notationBase)
        {
            var notationDict = new Dictionary<char, int>(notationBase);

            for (int value = 0; value < notationBase; value++)
            {
                if (value < 10)
                {
                    // 48 is '0' char
                    notationDict.Add((char)(48 + value), value);
                }
                else
                {
                    // 65 is 'A' char
                    notationDict.Add((char)(65 + value - 10), value);
                }
            }

            return notationDict.ToImmutableDictionary();
        }

        private static int CalcPartSum(string numPart) => numPart.Sum(c => _notation[c]);

        // Increment value in original notation
        private static bool IncrementNumber(StringBuilder num)
        {
            var numLenIterator = num.Length - 1;
            for (int i = numLenIterator; i >= 0; i--)
            {
                var currentDigit = num[i];
                if (currentDigit == _notationMaxValue)
                {
                    continue;
                }
                else
                {
                    var currentDigitValue = _notation[currentDigit] + 1;
                    var newCurrentDigit = _notation.First(c => c.Value == currentDigitValue).Key;
                    num[i] = newCurrentDigit;

                    for (int j = i + 1; j <= numLenIterator; j++)
                    {
                        num[j] = _notationMinValue;
                    }

                    break;
                }
            }

            return num.ToString().All(c => c == _notationMaxValue);
        }
    }
}
