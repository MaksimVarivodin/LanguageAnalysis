using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parsing
{
    public static class DoubleParsers
    {
        /// <summary>
        /// Parses a string to a double using the invariant culture.
        /// Returns 0 if parsing fails or the string is empty.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <returns>The parsed double value, or 0 if parsing fails.</returns>
        public static double doubleParseFromStringInvariant(string data)
        {
            if (data.Length <= 0)
                return 0d;
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out var x1))
                return x1;
            return 0d;
        }

        /// <summary>
        /// Parses a string to a double using the current culture.
        /// Returns 0 if parsing fails or the string is empty.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <returns>The parsed double value, or 0 if parsing fails.</returns>
        public static double doubleParseFromStringCurrent(string data)
        {
            if (data.Length <= 0)
                return 0d;
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.CurrentCulture, out var x1))
                return x1;
            return 0d;
        }

        /// <summary>
        /// Parses a string to a double, first using the current culture, then the invariant culture if needed.
        /// Returns 0 if parsing fails or the string is empty.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <returns>The parsed double value, or 0 if parsing fails.</returns>
        public static double doubleParseFromString(string data)
        {
            var result = doubleParseFromStringCurrent(data);
            if (result != 0d)
                return result;
            result = doubleParseFromStringInvariant(data);
            if (result != 0d)
                return result;
            return 0d;
        }

        /// <summary>
        /// Parses a tab-separated string into a tuple of two doubles.
        /// Expects the string to have three tab-separated parts; parses the second and third as doubles.
        /// Returns (0, 0) if parsing fails or the string is empty.
        /// </summary>
        /// <param name="data">The tab-separated string to parse.</param>
        /// <returns>A tuple of two doubles parsed from the string, or (0, 0) if parsing fails.</returns>
        public static (double, double) doubleParseFromStrings(string data)
        {
            if (data.Length <= 0)
                return (0d, 0d);
            var parts = data.Split('\t');
            if (parts.Length != 3)
                return (0d, 0d);
            if (double.TryParse(parts[1], NumberStyles.Float, CultureInfo.CurrentCulture, out var x2) &&
                double.TryParse(parts[2], NumberStyles.Float, CultureInfo.CurrentCulture, out var y2))
                return (x2, y2);
            else if (double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var x1) &&
                double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var y1))
                return (x1, y1);

            return (0d, 0d);
        }

        /// <summary>
        /// Compares two double values with a specified epsilon tolerance.
        /// Returns 0 if the values are approximately equal, 1 if a &gt; b, -1 if a &lt; b.
        /// Throws ArgumentException if either value is NaN.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <param name="epsilon">The tolerance for comparison (default: 1e-9).</param>
        /// <returns>0 if approximately equal, 1 if a &gt; b, -1 if a &lt; b.</returns>
        public static int CompareDouble(double a, double b, double epsilon = 1e-9)
        {
            // Проверка особых случаев
            if (double.IsNaN(a) || double.IsNaN(b))
                throw new ArgumentException("Cannot compare NaN values");

            // Бесконечности
            if (double.IsPositiveInfinity(a) && double.IsPositiveInfinity(b)) return 0;
            if (double.IsNegativeInfinity(a) && double.IsNegativeInfinity(b)) return 0;

            // Основное сравнение с погрешностью
            double diff = a - b;

            if (Math.Abs(diff) < epsilon)
                return 0;  // a ≈ b

            return diff > 0 ? 1 : -1; // a > b : a < b
        }
        public static bool IsDouble(string data)
        {
            double result;
            bool isDouble = double.TryParse(data, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out result)
                || double.TryParse(data, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result);

            bool hasSeparator = Regex.IsMatch(data, @"[.,]");
            return isDouble && hasSeparator;
        }
    }
}
