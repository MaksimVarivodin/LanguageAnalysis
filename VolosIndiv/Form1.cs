using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;



namespace VolosIndiv
{
    public partial class Form1 : Form
    {
        private enum AverageType
        {
            FirstAverage,
            SecondAverage,
            ThirdAverage,
        }
        public Form1()
        {
            InitializeComponent();
        }

        readonly DataGridView _dg = new DataGridView();
        readonly DataGridView _dg1 = new DataGridView();
        readonly DataGridView _dg2 = new DataGridView();

        // dont ask why ss, i have no fucking clue, i stole it while trying to repair the app written previously
        const string StolenRegexp_ss = "\\|\"{}()[]=+_~!@#$…%^&*№:";
        const string StolenRegexp_ss_or = "\\\\|\\||\"|{|}|\\(|\\)|\\[|\\]|=|\\+|_|~|!|@|#|\\$|…|%|\\^|&|\\*|№|:|,|\\.|\\?|;"; // same
        const string Endsigns = ",.?!;";
        private string selectedFolderPath;

        double[] x, y;
        double[] avgX;
        double[] avgY;
        double[] avgQuadY;
        int[] textCount;
        int[] textCount2;
        int[] textCount3;

        double maxPow = 0;
        int maxSteps = 0;

        int counter;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 3;                                                                                                                                                                                                                      //if (File.Exists("..//..//Properties//vini_vici_namaste.wav")){ try { new System.Media.SoundPlayer("..//..//Properties//vini_vici_namaste.wav").Play(); } catch (Exception) { } }//
        }

        #region UIHandlers Methods

        private async void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            chart1.Series[0].Points.Clear();
            textsAnalyzedLabel.Text = string.Empty;
            label5.Text = string.Empty;
            await OpenFolder(selectedFolderPath, true);
        }

        private async void countByWordsClick(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            chart1.Series[0].Points.Clear();
            textsAnalyzedLabel.Text = string.Empty;
            label5.Text = string.Empty;
            await OpenFolder(selectedFolderPath, false);
        }


        private void button7_Click(object sender, EventArgs e)
        {
            double a = 1.1, b = 3d;
            double eps = 0.00002; //точність від лукавого
            double avalue = 0d, bvalue = 0d;

            for (; a < 3d; a += 0.01)
            {
                avalue = GetLogLinearRegression(a);

                if (avalue > bvalue)
                {
                    b = a;
                    bvalue = avalue;
                }
            }
            MessageBox.Show($"Done! The best base = {b}");
            powAUpDown.Value = ((decimal)b);
        }

        private async void radioEqualLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg);
        }

        private async void radioDifferentLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg1);
        }

        private async void radioGrowLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg2);
        }

        private void saveDictionaryMenuItemClick(object sender, EventArgs e)
        {
            SaveSelectedFile(dataGridView1); // Save data from `dataGridView1`
        }

        private void saveBinningFileMenuItemClick(object sender, EventArgs e)
        {
            SaveSelectedFile(dataGridView2); // Save data from `dataGridView2`
        }


        private void ClearButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            chart1.Series[0].Points.Clear();
            textsAnalyzedLabel.Text = string.Empty;
            label5.Text = string.Empty;
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            switch (e.Column.Name)
            {
                case "Count":
                case "Unique":
                    {
                        if (!int.TryParse(e.CellValue1.ToString(), out var a))
                            a = 0;
                        if (!int.TryParse(e.CellValue2.ToString(), out var b))
                            b = 0;

                        e.SortResult = a.CompareTo(b);
                        break;
                    }
                case "NameDG":
                    e.SortResult = String.CompareOrdinal(e.CellValue1.ToString(), e.CellValue2.ToString());
                    break;
            }
            e.Handled = true;
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            _dg.ColumnCount = 6;
            _dg1.ColumnCount = 6;
            _dg2.ColumnCount = 6;
            _dg.Rows.Clear();
            _dg1.Rows.Clear();
            _dg2.Rows.Clear();
            dataGridView2.Rows.Clear();

            int M = ((int)binQuantityUpDown.Value);
            double basePow = 0d;
            try { basePow = ((double)powAUpDown.Value); }
            catch
            {
                MessageBox.Show("Введіть основу степеня!");
            }

            if (dataGridView1.RowCount == 0 && !fbd.CheckFileExists)
                counter = File.ReadLines(fbd.FileName).Count();
            else
                return;
            textsAnalyzedLabel.Text = Convert.ToString("Count = " + counter);

            if (M < counter)
            {

                for (int i = 0; i < 100; i++)
                {
                    maxPow = Math.Pow(basePow, i);
                    if (maxPow > x.Max())
                    {
                        maxSteps = i;
                        //MessageBox.Show($"EXCEEDED! Steps = {maxSteps}");
                        break;
                    }
                }
                //STEPS TO ADD on GRID?


                double step = (x.Max() - x.Min()) / (double)M;

                AverageMethod(x, y, M, 0, AverageType.FirstAverage, out avgX, out avgY, out avgQuadY, out textCount);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG ITER = {i}; {x.Min() + i * step} - {x.Min() + (i + 1) * step}; Step = {step}");
                    _dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }

                int step2 = (int)x.Length / M;

                AverageMethod(x, y, maxSteps, basePow, AverageType.ThirdAverage, out avgX, out avgY, out avgQuadY, out textCount3);
                int stepN = 0;

                for (int i = 0; i < maxSteps; i++)
                {
                    stepN = i;
                    //MessageBox.Show($"AVG3 ITER = {i}; {x.Min() + i * step2} - {x.Min() + (i + 1) * step2}; Step = {step2}");
                    _dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)) + " - " + Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
                }

                int step1 = (int)x.Length / M;

                AverageMethod(x, y, M, 0, AverageType.SecondAverage, out avgX, out avgY, out avgQuadY, out textCount2);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG2 ITER = {i}; {x[i * step1]} - {x[(step1 * (i + 1) - 1)]}; Step = {step1}");
                    _dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]) + " - " + Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i], textCount2[i]);
                }

                var selectedDataGrid = equalLengthRadio.Checked ? _dg :
                    differentLengthRadio.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }

            else
            {
                MessageBox.Show("Папка не містить файлів формату .txt");
            }

            /////////////////
        }

        private async void openDictionaryMenuItemClick(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() != DialogResult.OK || fbd.FileName.Length <= 0)
                return;

            chart1.Series[0].Points.Clear();
            var dgv = dataGridView1;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            try
            {
                dgv.ColumnCount = 3;
                dgv.Rows.Clear();

                string[] lines = File.ReadAllLines(fbd.FileName, Encoding.UTF8);
                progressBar1.Maximum = lines.Length;
                int[] xArray = new int[lines.Length];
                int[] yArray = new int[lines.Length];
                int index = 0;
                foreach (string line in lines)
                {
                    string[] res = Regex.Split(line, "\t");
                    dgv.Rows.Add(res);
                    xArray[index] = int.Parse(res[1]);
                    yArray[index] = int.Parse(res[2]);
                    progressBar1.Value++;
                    index++;
                }
                for (int i = 0; i < xArray.Length; i++)
                {
                    chart1.Series[0].Points.AddXY(xArray[i], yArray[i]);
                }


            }
            catch (Exception)
            {
                MessageBox.Show("The process done");
            }

            counter = File.ReadLines(fbd.FileName).Count();
            textsAnalyzedLabel.Text = Convert.ToString("Count = " + counter);

            //textBox1.Text = "2";

            int M = Convert.ToInt32(binQuantityUpDown.Text);

        }

        #endregion

        #region Private helper methods

        private void AverageMethod(double[] x, double[] y, int maxStep, double basePow, AverageType type, out double[] L, out double[] V, out double[] dV,
            out int[] textCount)
        {
            double step = 0d;
            switch (type)
            {
                case AverageType.FirstAverage:
                    step = (x.Max() - x.Min()) / (double)maxStep;
                    break;
                case AverageType.SecondAverage:
                    step = x.Length / maxStep;
                    break;
            }
            int count = 0;
            double avgQuadL = 0;
            double avgResL = 0;
            double avgQuadV = 0;
            double avgResV = 0;

            L = new double[maxStep];
            V = new double[maxStep];
            dV = new double[maxStep];
            textCount = new int[maxStep];
            if (type is AverageType.SecondAverage)
                Array.Sort(x, y);
            for (var i = 0; i < maxStep; i++)
            {
                avgQuadL = 0;
                avgQuadV = 0;
                avgResL = 0;
                avgResV = 0;
                count = 0;
                for (var j = 0; j < x.Length; j++)
                {
                    var shouldBeIncreased = false;
                    switch (type)
                    {
                        case AverageType.FirstAverage:
                            shouldBeIncreased = (x.Min() + i * step <= x[j]) && (x.Min() + (i + 1) * step >= x[j]);
                            break;
                        case AverageType.SecondAverage:
                            shouldBeIncreased = (i * step <= j) && ((i + 1) * step > j);
                            break;
                        case AverageType.ThirdAverage:
                            shouldBeIncreased = (Math.Pow(basePow, i) <= x[j]) && (Math.Pow(basePow, i + 1) >= x[j]);
                            break;
                        default:
                            shouldBeIncreased = false;
                            break;
                    }
                    if (!shouldBeIncreased)
                        continue;
                    avgResL += x[j];
                    avgQuadL += x[j] * x[j];
                    avgResV += y[j];
                    avgQuadV += y[j] * y[j];
                    count++;
                }
                if (count != 0)
                {
                    avgResL /= type is AverageType.SecondAverage ? step : count;
                    avgQuadL /= type is AverageType.SecondAverage ? step : count; ;
                    avgResV /= type is AverageType.SecondAverage ? step : count; ;
                    avgQuadV /= type is AverageType.SecondAverage ? step : count; ;
                }
                else
                {
                    avgResL = 0;
                    avgQuadL = 0;
                    avgResV = 0;
                    avgQuadV = 0;
                }
                textCount[i] = count;
                dV[i] = Math.Sqrt(avgQuadV - Math.Pow(avgResV, 2));
                L[i] = avgResL;
                V[i] = avgResV;
            }
        }

        private async Task<int> CountTextFilesAsync(string folderPath)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                {
                    throw new DirectoryNotFoundException("The specified folder does not exist.");
                }

                var textFiles = Directory.EnumerateFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                return textFiles.Count();
            });
        }

        private void BubbleSort(double[] array1, double[] array2)
        {
            int n = array1.Length;
            for (int i = 0; i < n - 1; i++)
            {
                // Проходим по массиву, уменьшая диапазон с каждым шагом
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Если текущий элемент больше следующего, меняем их местами
                    if (array1[j] > array1[j + 1])
                    {
                        double temp = array1[j];
                        array1[j] = array1[j + 1];
                        array1[j + 1] = temp;
                        temp = array2[j];
                        array2[j] = array2[j + 1];
                        array2[j + 1] = temp;
                    }
                }
            }
        }

        private async Task OpenFolder(string folderPath, bool byWords)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please select a folder first.");
                return;
            }

            label5.Text = "";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            textsAnalyzedLabel.Text = "";
            counter = 0;

            try
            {
                int textFileCount = await CountTextFilesAsync(folderPath);
                textsAnalyzedLabel.Text = $"Кількість текстів: {textFileCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while counting files: {ex.Message}");
                return;
            }

            var files = new List<string>();
            var processedFiles = 0;

            try
            {
                files = await ProcessDirectoryAsync(folderPath);
                progressBar1.Maximum = files.Count;
                progressBar1.Value = 0; // Reset progress bar value
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            var xlist = new double[files.Count];
            var ylist = new double[files.Count];

            await Task.WhenAll(files.Select((file, index) => Task.Run(() =>
            {
                var (rawText, unsignedText) = ProcessText(file);

                if (!byWords)
                {
                    xlist[index] = GetAllSymbolsCount(rawText);
                    ylist[index] = GetUniqueSymbolsCount(rawText);
                }
                else
                {
                    xlist[index] = GetAllWordsCount(unsignedText);
                    ylist[index] = GetDictionaryCount(unsignedText); 
                }

                // Update progress bar and processed files count
                Interlocked.Increment(ref processedFiles);
                Invoke((Action)(() =>
                {
                    progressBar1.Value = processedFiles;
                }));
            })));


            BubbleSort(xlist, ylist);


            var xList = new ArrayList(xlist.ToArray());
            var yList = new ArrayList(ylist.ToArray());


            
            for (var i = 0; i < xList.Count; i++)
            {
                chart1.Series[0].Points.AddXY(xList[i], yList[i]);
                AddToDataGrid(Path.GetFileNameWithoutExtension(files[i]), xlist[i], ylist[i]);
            }

            dataGridView1.Columns["Count"].ValueType = typeof(Int32);

            await SomeMagic(xList, yList);

            stopwatch.Stop();

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                stopwatch.Elapsed.TotalHours, stopwatch.Elapsed.TotalMinutes, stopwatch.Elapsed.TotalSeconds, stopwatch.Elapsed.TotalMilliseconds);
            label5.Text = $"Час виконання: {elapsedTime}";
            Invoke((Action)(() =>
            {
                progressBar1.Value = 0;
            }));
        }





        private (string, string) ProcessText(string filename)
        {
            string rawText;
            bool ignoreSpaces = true;
            bool ignoreNewLines = false;

            using (var sr = new StreamReader(filename))
            {
                rawText = PreprocessWithRegex(sr.ReadToEnd());
            }

            if (checkBox1.Checked)
            {
                rawText = rawText.ToLower();
            }

            var unsignedTextBuilder = new StringBuilder();
            var textAsRawBuilder = new StringBuilder();
            bool addedToRaw = false;


            for (int i = 0; i < rawText.Length; i++)
            {
                var symbol = rawText[i];

                if (!char.IsControl(symbol) || symbol == '\r' || symbol == '\n' || symbol == '\t')
                {
                    if (char.IsLetterOrDigit(symbol))
                    {
                        addedToRaw = true;
                        textAsRawBuilder.Append(symbol);
                        unsignedTextBuilder.Append(symbol);
                        ignoreSpaces = false;
                        ignoreNewLines = false;
                    }
                    else if (symbol == ' ' || symbol == '\t' || symbol == '\u00a0')
                    {
                        if (!ignoreSpaces)
                        {
                            addedToRaw = true;
                            textAsRawBuilder.Append(' ');
                            unsignedTextBuilder.Append(' ');
                            ignoreSpaces = true;
                        }
                    }
                    else if (symbol == '\n' && !ignoreNewLines)
                    {
                        addedToRaw = true;
                        textAsRawBuilder.Append(' ');
                        unsignedTextBuilder.Append(' ');
                        ignoreSpaces = true;
                        ignoreNewLines = true;
                    }
                    else if (StolenRegexp_ss.Contains(symbol))
                    {
                        addedToRaw = true;
                        textAsRawBuilder.Append(symbol);
                    }
                    else if (symbol == '-' && i > 0 && i < rawText.Length - 1)
                    {
                        if (char.IsLetter(rawText[i - 1]) && char.IsLetter(rawText[i + 1]))
                        {
                            addedToRaw = true;
                            textAsRawBuilder.Append(symbol);
                            unsignedTextBuilder.Append(symbol);
                        }
                    }
                    else if (symbol == '`' || symbol == '\'' || symbol == '’' || symbol == 'ʼ')
                    {
                        addedToRaw = true;
                        textAsRawBuilder.Append('\'');
                        unsignedTextBuilder.Append('\'');
                    }
                    else if (Endsigns.Contains(symbol))
                    {
                        addedToRaw = true;
                        textAsRawBuilder.Append(symbol);
                    }

                    if (!addedToRaw && symbol != '\r' && symbol != '\n' && symbol != ' ')
                    {
                        textAsRawBuilder.Append(symbol);
                    }

                    addedToRaw = false;
                }
            }

            var textAsRaw = textAsRawBuilder.ToString();
            var unsignedText = unsignedTextBuilder.ToString();

            return (textAsRaw, unsignedText);
        }







        private string PreprocessWithRegex(string text)
        {
            Regex reg_exp = new Regex("(" + StolenRegexp_ss_or + ")--");
            text = reg_exp.Replace(text, "--");

            reg_exp = new Regex("--(" + StolenRegexp_ss_or + ")");
            text = reg_exp.Replace(text, "--");

            reg_exp = new Regex(@"(?<=(\w))--(?=(\w))");
            text = reg_exp.Replace(text, " ");

            return text;
        }
        private int GetAllSymbolsCount(string text)
        {
            return text.Count(symbol => !checkBox2.Checked ? symbol != ' ' : true);
        }

        private int GetUniqueSymbolsCount(string text)
        {
            var uniqueSymbols = new HashSet<char>();

            foreach (var ch in text)
            {
                if (!checkBox2.Checked && ch == ' ')
                {
                    continue;
                }
                uniqueSymbols.Add(checkBox1.Checked ? char.ToLower(ch) : ch);
            }

            return uniqueSymbols.Count;
        }


        private int GetAllWordsCount(string text)
        {
            var parsedWords = text
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            return parsedWords.Count;
        }


        private int GetDictionaryCount(string text)
        {
            var punctuation = new HashSet<char> { '.', ',', ';', '!', '?', ':', '-', '\'', '\"', '’', '“', '”' };

            var words = text
                .Split(new char[] { ' ', '\t', '\n', '\r', '\u00a0' }, StringSplitOptions.RemoveEmptyEntries);

            var uniqueWords = new HashSet<string>();

            foreach (var rawWord in words)
            {
                var cleanedWord = rawWord.Trim(punctuation.ToArray());

                if (checkBox1.Checked)
                {
                    cleanedWord = cleanedWord.ToLower();
                }

                if (!string.IsNullOrWhiteSpace(cleanedWord))
                {
                    uniqueWords.Add(cleanedWord);
                }
            }

            return uniqueWords.Count;
        }






        private async Task SomeMagic(ArrayList xList, ArrayList yList)
        {
            var binCount = ((int)binQuantityUpDown.Value);
            x = xList.ToArray(typeof(double)) as double[];
            y = yList.ToArray(typeof(double)) as double[];

            _dg.ColumnCount = 6;
            _dg1.ColumnCount = 6;
            _dg2.ColumnCount = 6;
            _dg.Rows.Clear();
            _dg1.Rows.Clear();
            _dg2.Rows.Clear();

            double basePow = ((double)powAUpDown.Value);

            if (binCount < xList.Count)
            {

                for (var i = 0; i < 100; i++)
                {
                    maxPow = Math.Pow(basePow, i);
                    if (x == null || !(maxPow > x.Max()))
                        continue;
                    maxSteps = i;
                    break;
                }
                //STEPS TO ADD on GRID?

                double step = (x.Max() - x.Min()) / (double)binCount;

                AverageMethod(x, y, binCount, 0, AverageType.FirstAverage, out avgX, out avgY, out avgQuadY, out textCount);

                for (int i = 0; i < binCount; i++)
                {
                    //MessageBox.Show($"AVG ITER = {i}; {x.Min() + i * step} - {x.Min() + (i + 1) * step}; Step = {step}");
                    _dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }

                int step2 = (int)x.Length / binCount;

                AverageMethod(x, y, maxSteps, basePow, AverageType.ThirdAverage, out avgX, out avgY, out avgQuadY, out textCount3);
                int stepN = 0;

                for (int i = 0; i < maxSteps; i++)
                {
                    stepN = i;
                    //MessageBox.Show($"AVG3 ITER = {i}; {x.Min() + i * step2} - {x.Min() + (i + 1) * step2}; Step = {step2}");
                    _dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)) + " - " + Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
                }

                int step1 = (int)x.Length / binCount;

                AverageMethod(x, y, binCount, 0, AverageType.SecondAverage, out avgX, out avgY, out avgQuadY, out textCount2);

                for (int i = 0; i < binCount; i++)
                {
                    //MessageBox.Show($"AVG2 ITER = {i}; {x[i * step1]} - {x[(step1 * (i + 1) - 1)]}; Step = {step1}");
                    _dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]) + " - " + Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i], textCount2[i]);
                }


                var selectedDataGrid = equalLengthRadio.Checked ? _dg :
                    differentLengthRadio.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }
        }

        private void AddToDataGrid(string name, double count, double unique)
        {
            dataGridView1.Rows.Add(name, count.ToString(CultureInfo.InvariantCulture), unique.ToString(CultureInfo.InvariantCulture));
        }

        private async Task RadioButtonCheckedChanged(DataGridView dataGrid)
        {
            if (dataGridView2.Rows.Count <= 2)
                return;
            dataGridView2.Rows.Clear();
            _dg.ColumnCount = 6;
            _dg1.ColumnCount = 6;
            _dg2.ColumnCount = 6;

            await UpdateDataGrid(_dg);
        }

        private void SaveSelectedFile(DataGridView dataGridView)
        {
            // Initialize SaveFileDialog
            SaveFileDialog saveFile = new SaveFileDialog
            {
                DefaultExt = "*.txt",
                Filter = "TXT Files|*.txt"
            };

            // Show the dialog and check if the user selected a file
            if (saveFile.ShowDialog() != DialogResult.OK || saveFile.FileName.Length <= 0)
                return;

            try
            {
                // Create a StreamWriter to write to the selected file
                using (var sw = new StreamWriter(saveFile.FileName))
                {
                    // Iterate through DataGridView rows and columns
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            // Check if the cell value is not null
                            if (dataGridView.Rows[i].Cells[j].Value != null)
                            {
                                // Write the cell value followed by a tab character
                                sw.Write(dataGridView.Rows[i].Cells[j].Value.ToString() + "\t");
                            }
                        }
                        // Write a newline after each row
                        sw.WriteLine();
                    }
                }
                // Inform the user that the data was saved successfully
                MessageBox.Show("Дані збережено");
            }
            catch (Exception ex)
            {
                // Show an error message if something went wrong
                MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}");
            }
        }


        private async Task UpdateDataGrid(DataGridView dataGrid)
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                var items = new object[row.Cells.Count];
                for (var i = 0; i < row.Cells.Count; i++)
                {
                    items[i] = row.Cells[i].Value;
                }
                await AddRowAsync(dataGridView2, items);
            }
        }
        private async Task AddRowAsync(DataGridView dataGridView, object[] items)
        {
            await Task.Run(() =>
            {
                dataGridView.Invoke(new Action(() =>
                {
                    dataGridView.Rows.Add(items);
                    dataGridView.Update();
                }));
            });
        }

        private static async Task<List<string>> ProcessDirectoryAsync(string targetDirectory)
        {
            try
            {
                var fileEntries = new List<string>();

                var filesInTargetDirectory = await Task.Run(() => Directory.GetFiles(targetDirectory));
                fileEntries.AddRange(filesInTargetDirectory);

                var subdirectoryEntries = await Task.Run(() => Directory.GetDirectories(targetDirectory));
                var subdirectoryTasks = subdirectoryEntries.Select(ProcessDirectoryAsync).ToList();

                await Task.WhenAll(subdirectoryTasks);

                foreach (var subdirectoryTask in subdirectoryTasks)
                {
                    var filesInSubdirectory = await subdirectoryTask;
                    fileEntries.AddRange(filesInSubdirectory);
                }

                return fileEntries;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading directory");
                throw;
            }
        }

        private void openFolderMenuItemClick(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = folderBrowserDialog.SelectedPath;
                    string folderName = Path.GetFileName(selectedFolderPath.TrimEnd(Path.DirectorySeparatorChar));

                    label6.Text = "Папка: " + folderBrowserDialog.SelectedPath;

                    countByWordsButton.Enabled = true;
                    countBySymbolsButton.Enabled = true;
                }
            }
        }

        private void openBinningFileMenuItemClick(object sender, EventArgs e)
        {
            var dgv = dataGridView2;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            //for files in current folder
            try
            {
                dgv.ColumnCount = 6;
                dgv.Rows.Clear();
                if (fbd.ShowDialog() == DialogResult.OK && fbd.FileName.Length > 0)
                {
                    string[] lines = File.ReadAllLines(fbd.FileName, Encoding.UTF8);
                    progressBar1.Maximum = lines.Length;

                    foreach (string line in lines)
                    {
                        string[] res = Regex.Split(line, "\t");
                        dgv.Rows.Add(res);
                        progressBar1.Value++;

                    }


                }

            }
            catch (Exception)
            {
                MessageBox.Show("The process done");
            }

            counter = File.ReadLines(fbd.FileName).Count();
            textsAnalyzedLabel.Text = Convert.ToString("Текстів: " + counter);
            int M = Convert.ToInt32(binQuantityUpDown.Text);

        }

        private void fbd_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }


        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private double GetLogLinearRegression(double basePow)
        {
            var steps = 0;
            double res = 0;
            if (x != null)
            {
                while (res < x.Max())
                {
                    steps++;
                    res = Math.Pow(basePow, steps);
                }
                AverageMethod(x, y, steps, basePow, AverageType.ThirdAverage, out avgX, out avgY, out avgQuadY, out textCount3);

                var avgQuadYList = avgQuadY.ToList();
                var avgXList = avgX.ToList();

                for (var i = 0; i < avgQuadYList.Count;)
                {
                    if (avgQuadYList[i] == 0)
                    {
                        avgQuadYList.RemoveAt(i);
                        avgXList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                var avgQuadYLog = (from quads in avgQuadYList select Math.Log10(quads)).ToArray();
                var avgXLog = (from xs in avgXList select Math.Log10(xs)).ToArray();

                var sr = new Accord.Statistics.Models.Regression.Linear.OrdinaryLeastSquares();
                var regressionResult = sr.Learn(avgXLog, avgQuadYLog);
                return regressionResult.CoefficientOfDetermination(avgXLog, avgQuadYLog);
            }
            return 2.0;
        }

        #endregion
    }
}






