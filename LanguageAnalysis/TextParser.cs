using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ude;

namespace VolosIndiv
{
    /// <summary>
    /// Винесен алгоритм парсингу тексту (символи, слова) з NgrammProcessor.
    /// Додано підтримку файлів з різними кодуваннями через Ude.
    /// </summary>
    public static class TextParser
    {
        private static readonly Regex HyphenCleanRegex = new Regex("(?<=(\\w))--(?=(\\w))", RegexOptions.Compiled);
        private const string StolenRegexp = "\\\\|\\\"{}()[]=+_~!@#$…%^&*№:";
        private const string StolenRegexpOr = "\\\\|\\||\"|{|}|\\(|\\)|[|]|=|+|_|~|!|@|#|\\$|…|%|\\^|&|*|№|:|,|\\.|?|;";
        private const string Endsigns = ",.?!;";

        /// <summary>
        /// Попередня обробка тексту: видалення -- між літерами.
        /// </summary>
        public static string PreprocessWithRegex(string text)
        {
            text = new Regex("(" + Regex.Escape(StolenRegexpOr) + ")--").Replace(text, "--");

            text = new Regex("--(" + Regex.Escape(StolenRegexpOr) + ")").Replace(text, "--");
            text = HyphenCleanRegex.Replace(text, " ");
            return text;
        }

        /// <summary>
        /// Розбиває файл на "raw" та "unsigned" тексти, визначаючи кодування за допомогою Ude.
        /// </summary>
        public static (string rawText, string unsignedText) ExtractTexts(string filename)
        {
            byte[] rawBytes = File.ReadAllBytes(filename);
            var detector = new CharsetDetector();
            detector.Feed(rawBytes, 0, rawBytes.Length);
            detector.DataEnd();
            Encoding encoding = detector.Charset != null
                ? Encoding.GetEncoding(detector.Charset)
                : Encoding.UTF8;
            string content = encoding.GetString(rawBytes);
            content = PreprocessWithRegex(content);
            return ProcessContent(content);
        }

        /// <summary>
        /// Перевантаження: приймає вхідний текст замість файлу.
        /// </summary>
        public static (string rawText, string unsignedText) ExtractTexts(string content, bool preprocess = true)
        {
            if (preprocess)
                content = PreprocessWithRegex(content);
            return ProcessContent(content);
        }

        private static (string rawText, string unsignedText) ProcessContent(string content)
        {
            var rawBuilder = new StringBuilder();
            var unsignedBuilder = new StringBuilder();
            bool ignoreSpaces = true;
            bool ignoreNewLines = false;

            foreach (char symbol in content)
            {
                if (char.IsControl(symbol) && symbol != '\r' && symbol != '\n' && symbol != '\t')
                    continue;

                if (char.IsLetterOrDigit(symbol))
                {
                    rawBuilder.Append(symbol);
                    unsignedBuilder.Append(symbol);
                    ignoreSpaces = false;
                    ignoreNewLines = false;
                }
                else if ((symbol == ' ' || symbol == '\t' || symbol == '\u00a0') && !ignoreSpaces)
                {
                    rawBuilder.Append(' ');
                    unsignedBuilder.Append(' ');
                    ignoreSpaces = true;
                }
                else if (symbol == '\n' && !ignoreNewLines)
                {
                    rawBuilder.Append(' ');
                    unsignedBuilder.Append(' ');
                    ignoreSpaces = true;
                    ignoreNewLines = true;
                }
                else if (StolenRegexp.IndexOf(symbol) >= 0)
                {
                    rawBuilder.Append(symbol);
                }
                else if ((symbol == '`' || symbol == '\'' || symbol == '’' || symbol == 'ʼ'))
                {
                    rawBuilder.Append('\'');
                    unsignedBuilder.Append('\'');
                }
                else if (Endsigns.IndexOf(symbol) >= 0)
                {
                    rawBuilder.Append(symbol);
                }
                else
                {
                    rawBuilder.Append(symbol);
                }
            }

            return (rawBuilder.ToString(), unsignedBuilder.ToString());
        }

        public static int GetAllSymbolsCount(string text, bool countSpaces)
            => countSpaces ? text.Length : text.Count(c => !char.IsWhiteSpace(c));

        public static int GetUniqueSymbolsCount(string text, bool ignoreSpaces, bool toLower)
        {
            var set = new HashSet<char>();
            foreach (var ch in text)
            {
                if (ignoreSpaces && char.IsWhiteSpace(ch)) continue;
                set.Add(toLower ? char.ToLowerInvariant(ch) : ch);
            }
            return set.Count;
        }

        public static int GetAllWordsCount(string text)
            => text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public static int GetDictionaryCount(string text, bool toLower)
        {
            var punctuation = new HashSet<char> { '.', ',', ';', '!', '?', ':', '-', '\'', '"', '’', '“', '”' };
            var words = text.Split(new[] { ' ', '\t', '\n', '\r', '\u00a0' }, StringSplitOptions.RemoveEmptyEntries);
            var set = new HashSet<string>();
            foreach (var raw in words)
            {
                var cleaned = raw.Trim(punctuation.ToArray());
                if (toLower) cleaned = cleaned.ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(cleaned)) set.Add(cleaned);
            }
            return set.Count;
        }
    }
}

