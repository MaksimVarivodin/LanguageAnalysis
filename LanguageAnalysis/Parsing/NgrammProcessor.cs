using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Globalization;
using JiebaNet.Segmenter;
using NMeCab;
using System.Text;

using NGramm.Models;
using Ude;
using FolderWork;

namespace NGramm
{
    public class NgrammProcessor
    {
        private const int ProgressDivider = 1_000;
        bool IgnoreSpaces = false;
        bool IgnoreNLines = false;
        bool IgnoreEnds = false;
        private bool isCode = false;

        public static string Signs = "\\|\"{}()[]=+_~!@'#$-…%^&*:№:";
        public string SS = "\\|\"{}()[]=+_~'@#$…%^&*№:";
        public static HashSet<char> EndSigns = new HashSet<char>(".?!;。？！¿¡؟؛¿¡።༼⸮〽⋯…⸰;".ToCharArray());
        public static string EndSignss = ",.?!;";
        public string RawTextOrg = "";
        public int CountDesiredVariables = 0;
        public string UnsignedTextorg = "";
        public string EndsignedTextorg = "";
        public string Raw = "";
        public static bool ProcessSpaces = true;
        public static bool ConsequtiveSpaces = true;        
        public static bool ignore_case = true;
        public static bool ignore_punctuation = true;
        public static bool ignoreComments = false;

        private static HashSet<char> spaces_list = new HashSet<char> { '\u0020', '\u00a0', '\u1680', '\u2000', '\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200a', '\u202f', '\u205f', '\u3000', '\u200b' };
        private static char[] skip_spaces = new char[] { '\u3000' };
        private static string[] consequtive_spaces_pattern = spaces_list.Select(x => $"({x})+").ToArray();
        private readonly Regex spaces_pattern = new Regex($"({string.Join("|", consequtive_spaces_pattern)})", RegexOptions.Compiled); private static
            List<UnicodeCategory> nonRenderingCategories = new List<UnicodeCategory> {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate,
            UnicodeCategory.Format
        };
        private static Regex startSpaces = new Regex(@"^\s+", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex endSpaces = new Regex(@"\s+$", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex spacesInRow = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Multiline);

        private ConcurrentBag<NGrammContainer> symbol_ngrams = new ConcurrentBag<NGrammContainer>();
        private ConcurrentBag<NGrammContainer> words_ngrams = new ConcurrentBag<NGrammContainer>();
        private ConcurrentBag<NGrammContainer> literal_ngrams = new ConcurrentBag<NGrammContainer>();

        private readonly string _filename;
    
        public readonly ProgressReporter progressReporter;
        private static MeCabParam mecabParameter;

        public NgrammProcessor(string filename, ProgressReporter progressReporter)
        {

            _filename = filename;
            CountDesiredVariables = 0;
            this.progressReporter = progressReporter;
        }

        private static bool IsEndSign(char ch) => EndSigns.Contains(ch);

        public string GetFileContent()
        {
            var bytes = File.ReadAllBytes(_filename);
            CharsetDetector cdet = new CharsetDetector();
            cdet.Feed(bytes, 0, bytes.Length);
            cdet.DataEnd();

            Encoding encoding = Encoding.GetEncoding("windows-1251");
            if (cdet.Charset != null)
            {
                encoding = Encoding.GetEncoding(cdet.Charset);
            }

            return encoding.GetString(bytes);
        }

        public async Task Preprocess(double oneDividedToFileCount)
        {
            await Task.Run(() =>
            {
                Regex reg_exp = new Regex(@"(?<=(\w))--(?=(\w))");

                progressReporter.StartNewOperation("Ініціалізація");
                progressReporter.MoveProgress();

                RawTextOrg = reg_exp.Replace(GetFileContent(), " ").Trim().Replace("\r", "");
                progressReporter.MoveProgress();

                var _raw = new StringBuilder();
                var _uns = new StringBuilder();
                var _ends = new StringBuilder();
                var progressMult = RawTextOrg.Length / 90;
                var currentPr = 1;

                progressReporter.MoveProgress();
                for (int i = 0; i < RawTextOrg.Length; i++)
                {

                    if (i >= progressMult * currentPr)
                    {
                        currentPr++;
                        progressReporter.MoveProgress();
                    }

                    char ch = RawTextOrg[i];
                    /*                    if (!char.IsControl(ch))
                                        {*/
                    bool added = false;
                    if (char.IsLetter(ch))
                    {
                        _raw.Append(ch);
                        added = true;
                        _uns.Append(ch);
                        _ends.Append(ch);
                        IgnoreSpaces = IgnoreEnds = IgnoreNLines = false;
                    }
                    else if (char.IsDigit(ch))
                    {
                        _raw.Append(ch);
                        added = true;
                        _uns.Append(ch);
                        _ends.Append(ch);
                        IgnoreSpaces = IgnoreEnds = IgnoreNLines = false;
                    }
                    else if (!IgnoreSpaces && (spaces_list.Contains(ch) || ch == '\t'))
                    {
                        _raw.Append(ch);
                        added = true;
                        _uns.Append(ch);
                        _ends.Append(ch);
                        IgnoreSpaces = true;
                        IgnoreEnds = false;
                    }
                    else if ((ch == '\n') && !IgnoreNLines)
                    {
                        _raw.Append(ch);
                        added = true;
                        _uns.Append(ch);
                        _ends.Append(ch);
                        IgnoreSpaces = true;
                        IgnoreNLines = true;
                        IgnoreEnds = false;
                    }
                    else if (SS.Contains(ch))
                    {
                        _raw.Append(ch);
                        added = true;
                    }
                    else if (ch == '-')
                    {
                        _raw.Append(ch);
                        added = true;
                    }
                    else if (!IgnoreEnds && IsEndSign(ch))
                    {
                        _raw.Append(ch);
                        added = true;
                        _ends.Append(ch);
                    }
                    else if (!added)
                    {
                        _raw.Append(ch);
                        added = true;
                    }
                }
                //}

                RawTextOrg = _raw.ToString();
                EndsignedTextorg = _ends.ToString();
                UnsignedTextorg = _uns.ToString();
                _raw.Clear();
                _ends.Clear();
                _uns.Clear();

                progressReporter.Finish();

            });
        }

        public static void InitializeJapaneseProcessing(string folderName)
        {
            if (FolderChecker.IsValidFolder(folderName))
                mecabParameter = new MeCabParam
                {
                    DicDir = folderName
                };
        }
        #region Symbol ngramms

        public Task ProcessSymbolNGramms(int n) =>
            Task.Run(() =>
            {
                progressReporter.StartNewOperation($"Обчислення символьних н-грамм від 1 до {n}");
                var text = RawTextOrg;

                if (!ConsequtiveSpaces)
                {
                    text = RemoveConsequtiveSpaces(text);

                    text = spacesInRow.Replace(text, " ");
                }


                CountDesiredVariables = text.Length;
                symbol_ngrams = new ConcurrentBag<NGrammContainer>();
                int progressMul = text.Length / 95;
                Parallel.For(1, n + 1, PerformanceSettings.ParallelOpt, nn =>
                {
                    var ct = ProcessSymbolNgrmmToContainer(text, nn, progressMul);
                    symbol_ngrams.Add(ct);
                });

                symbol_ngrams = new ConcurrentBag<NGrammContainer>(symbol_ngrams.OrderByDescending(w => w.n));
                progressReporter.Finish();
            });

        private NGrammContainer ProcessSymbolNgrmmToContainer(string text, int n, int reportMul = 0)
        {
            var container = new NGrammContainer(n);
            bool breaked;
            char ch;
            int ct = 1;

            for (int i = 0; i < text.Length; i++)
            {
                if (reportMul != 0 && i > reportMul * ct)
                {
                    progressReporter.MoveProgress();
                    ct++;
                }

                if (i + n - 1 < text.Length)
                {
                    breaked = false;
                    StringBuilder ngramBuilder = new StringBuilder();
                    for (int k = 0; k < n; k++)
                    {
                        ch = text[i + k];
                        var isSpace = spaces_pattern.Match(ch.ToString()).Success;
                        var notPrintableSymbol = NonRenderingCategories(ch);

                        if (ProcessSpaces && !notPrintableSymbol)
                        {
                            if (isSpace)
                            {
                                ngramBuilder.Append(' ');
                            }
                            else
                                ngramBuilder.Append(ch);
                        }
                        else
                        {
                            if (!isSpace && !notPrintableSymbol)
                            {
                                ngramBuilder.Append(ch);
                            }
                            else
                            {
                                breaked = true;
                                break;
                            }
                        }
                    }
                    if (!breaked)
                    {
                        var ngram = ngramBuilder.ToString();
                        ngramBuilder.Clear();
                        container.Add(ignore_case ? ngram.ToLower() : ngram);
                    }
                }
            }

            container.Process();
            return container;
        }

        public int GetSymbolsCount(bool countSpaces)
        {
            var text = RawTextOrg;

            if (!ConsequtiveSpaces)
            {
                text = RemoveConsequtiveSpaces(text);

                text = new string(text.Where(x => !NonRenderingCategories(x)).ToArray());
                
            }
            text = new string(text.Where(x => !NonRenderingCategories(x)).ToArray());
            
            return countSpaces ? text.Length : text.Count(c => !spaces_list.Contains(c));
        }

        #endregion

        #region Literal ngramms 

        public Task ProcessLiteralNGramms(int n) =>
            Task.Run(() =>
            {
                progressReporter.StartNewOperation($"Обчислення буквенних н-грамм від 1 до {n}");

                var text = RemoveConsequtiveSpaces(UnsignedTextorg);
                CountDesiredVariables = text.Length;
                var progressMult = text.Length * n / 95;
                literal_ngrams = new ConcurrentBag<NGrammContainer>();

                Parallel.For(1, n + 1, PerformanceSettings.ParallelOpt, nn =>
                {
                    var ct = ProcessLiteralNgrmmToContainer(text, nn, false, progressMult);
                    literal_ngrams.Add(ct);
                });

                literal_ngrams = new ConcurrentBag<NGrammContainer>(literal_ngrams.OrderByDescending(w => w.n));
                progressReporter.Finish();
            });

        private NGrammContainer ProcessLiteralNgrmmToContainer(string text, int n, bool countDigits, int progressMul = 0)
        {
            var container = new NGrammContainer(n);
            bool breaked;
            char ch;
            int ct = 1;

            text = spacesInRow.Replace(text, " ");

            for (int i = 0; i < text.Length; i++)
            {
                if (progressMul != 0 && i > progressMul * ct)
                {
                    progressReporter.MoveProgress();
                    ct++;
                }

                if (i + n - 1 < text.Length)
                {
                    breaked = false;
                    string ngram = string.Empty;
                    for (int k = 0; k < n; k++)
                    {
                        ch = text[i + k];

                        var isSpace = spaces_pattern.Match(ch.ToString()).Success;
                        var notPrintableSymbol = NonRenderingCategories(ch);

                        if (char.IsControl(ch))
                        {
                            breaked = true;
                            break;
                        }

                        if (ProcessSpaces)
                        {
                            if (!IsEndSign(ch) && !notPrintableSymbol)
                            {
                                if (isSpace)
                                    ngram += ' ';
                                else
                                    ngram += ch;
                            }
                            else
                            {
                                breaked = true;
                                break;
                            }

                        }
                        else
                        {
                            if (!IsEndSign(ch) && !spaces_list.Contains(ch) && !notPrintableSymbol)
                            {
                                ngram += ch;
                            }
                            else
                            {
                                breaked = true;
                                break;
                            }
                        }
                    }

                    if (!breaked)
                    {
                        container.Add(ignore_case ? ngram.ToLower() : ngram);
                    }
                }
            }

            container.Process();
            return container;
        }

        public int GetLiteralCount(bool countSpaces)
        {
            var text = spacesInRow.Replace(UnsignedTextorg, " ");

            return text.Count(c => char.IsLetter(c) || (countSpaces && spaces_list.Contains(c)));
        }

        #endregion


        #region Word ngramms

        public Task ProcessWordNGramms(int n) =>

            Task.Run(() =>
            {
                progressReporter.StartNewOperation($"Обчислення словесних н-грамм від 1 до {n}");
                progressReporter.MoveProgress();

                var words = Words(ignore_punctuation ? UnsignedTextorg : EndsignedTextorg);
                CountDesiredVariables = words.Count;
                words_ngrams = new ConcurrentBag<NGrammContainer>();
                int progressMult = words.Count / 95;

                Parallel.For(1, n + 1, PerformanceSettings.ParallelOpt, nn =>
                {
                    var ct = ProcessWordNgrmmToContainer(words, nn, ignore_punctuation, ignore_case, progressMult);
                    words_ngrams.Add(ct);
                });

                words_ngrams = new ConcurrentBag<NGrammContainer>(words_ngrams.OrderByDescending(w => w.n));
                progressReporter.Finish();
            });

        public int GetWordsCount() =>
            Words(ignore_punctuation ? UnsignedTextorg : EndsignedTextorg).Count;

        private NGrammContainer ProcessWordNgrmmToContainer(List<CategorizedTokens> words, int n, bool skipss, bool ignoreCase, int progressMul = 0)
        {
            var container = new NGrammContainer(n);
            bool breaked;
            int ct = 1;

            for (int i = 0; i < words.Count; i++)
            {
                if (progressMul != 0 && i > progressMul * ct)
                {
                    progressReporter.MoveProgress();
                    ct++;
                }

                if (i + n - 1 < words.Count)
                {
                    breaked = false;
                    StringBuilder ngramBuilder = new StringBuilder();
                    for (int k = 0; k < n; k++)
                    {
                        var word = words[i + k].Value;

                        if (string.IsNullOrWhiteSpace(word) || word.All(x => NonRenderingCategories(x)))
                        {
                            breaked = true;
                            break;
                        }

                        if (skipss)
                        {
                            if (word.Length > 1 && IsEndSign(word[word.Length - 1]))
                                word = word.Remove(word.Length - 1);

                            if (ngramBuilder.Length == 0)
                                ngramBuilder.Append(word);
                            else
                                ngramBuilder.Append($" {word}");
                        }
                        else
                        {
                            if (ngramBuilder.Length == 0)
                            {
                                ngramBuilder.Append(word);
                            }
                            else
                            if (!IsEndSign(ngramBuilder[ngramBuilder.Length - 1])) ngramBuilder.Append($" {word}");
                            else
                            {
                                breaked = true;
                                break;
                            }
                        }
                    }
                    if (!breaked)
                    {

                        CategorizedTokens type = null;
                        string ngram = RemoveEndSigns(ngramBuilder.ToString());

                        ngramBuilder.Clear();
                        container.Add(ignoreCase ? ngram.ToLower() : ngram, type != null ? type.Type : string.Empty);
                    }
                }
            }

            container.Process();
            return container;
        }

        #endregion

        private string RemoveEndSigns(string word)
        {
            return new string(word.Where(x => !IsEndSign(x) && (char.IsLetterOrDigit(x) || char.IsWhiteSpace(x))).ToArray());
        }

        public IReadOnlyCollection<NGrammContainer> GetLiteralNgrams() => literal_ngrams;

        public IReadOnlyCollection<NGrammContainer> GetSymbolNgrams() => symbol_ngrams;

        public IReadOnlyCollection<NGrammContainer> GetWordsNgrams() => words_ngrams;


        public List<CategorizedTokens> Words(string inputText)
        {

            var text = startSpaces.Replace(inputText, "");
            text = endSpaces.Replace(text, "");
            var textContainSpaces = text.Any(x => spaces_list.Except(skip_spaces).Contains(x));

            var splitList = new List<char>(spaces_list);
            splitList.Add('\n');
            if (textContainSpaces)
            {
                return text.Split(splitList.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => new CategorizedTokens { Value = value, Type = string.Empty })
                    .ToList(); ;
            }
            else
            {
                return TrySplitWords(text);
            }
        }


        public static List<CategorizedTokens> TrySplitWords(string inputText)
        {
            var analysis = AnalyzeText(inputText);

            if (analysis.Hiragana > 0 || analysis.Katakana > 0)
                return TokenizeJapanese(inputText)
                    .Select(value => new CategorizedTokens { Value = value, Type = string.Empty })
                    .ToList();
            else if (analysis.Kanji > 0)
                return TokenizeChinese(inputText)
                    .Select(value => new CategorizedTokens { Value = value, Type = string.Empty })
                    .ToList();


            return inputText.Split(spaces_list.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(value => new CategorizedTokens { Value = value, Type = string.Empty })
                    .ToList();
        }

        private static (int Hiragana, int Katakana, int Kanji) AnalyzeText(string input)
        {
            int hiragana = 0, katakana = 0, kanji = 0;
            foreach (char c in input)
            {
                if (c >= '\u3040' && c <= '\u309F') hiragana++;
                else if ((c >= '\u30A0' && c <= '\u30FF') ||
                         (c >= '\u31F0' && c <= '\u31FF') ||
                         (c >= '\uFF65' && c <= '\uFF9F')) katakana++;
                else if ((c >= '\u4E00' && c <= '\u9FFF') ||
                         (c >= '\u3400' && c <= '\u4DBF')) kanji++;
            }
            return (hiragana, katakana, kanji);
        }


        private static string[] TokenizeJapanese(string text)
        {

            var tagger = MeCabTagger.Create();

            var parsed = tagger.Parse(text);

            return parsed.Split('\n').Select(x => x.Split('\t')[0]).ToArray();
        }
        private static string[] TokenizeChinese(string text)
        {
            var segmenter = new JiebaSegmenter();
            return segmenter.Cut(text, true).ToArray();
        }





        private string RemoveConsequtiveSpaces(string input)
        {
            var result = input;
            foreach (string item in consequtive_spaces_pattern)
            {
                var regex = new Regex(item, RegexOptions.Multiline);

                result = regex.Replace(result, match =>
                {
                    return match.Value[0].ToString();
                });

            }

            return result;
        }
        private bool NonRenderingCategories(char c)
        {
            return nonRenderingCategories.Contains(char.GetUnicodeCategory(c));
        }
    }
}