using NGramm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FolderWork;
using TableExport;
using Parsing;
using JiebaNet.Segmenter.Common;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace VolosIndiv 
{
    public partial class Form1 : Form
    {

        const string ChineeseParsingResourceFolder = "ChineseResources";
        const string JapaneseParsingResourceFolder = "JapaneseResources\\Dictionary";

        private enum AverageType
        {
            FirstAverage,
            SecondAverage,
            ThirdAverage,
        }
        public Form1()
        {
            InitializeComponent();
            InitializeAsianParsingResources();
            // Chart settings
            parsingResultsChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            parsingResultsChart.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            parsingResultsChart.Series[0].MarkerSize = 14;
            NgrammProcessor.IgnoreCase = ignoreRegexCheckbox.Checked;
            NgrammProcessor.ProcessSpaces = includingSpacesCheckbox.Checked;
            
            
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

        int parsedTexts;




        #region UIHandlers Methods

        /// <summary>
        /// Handles the change event for the ignore regex checkbox.
        /// Sets the ignore_case property in NgrammProcessor.
        /// </summary>
        private void IgnoreRegexChanged(object sender, EventArgs e)
        {
            NgrammProcessor.IgnoreCase = ignoreRegexCheckbox.Checked;
        }

        /// <summary>
        /// Handles the change event for the include spaces checkbox.
        /// Sets the ProcessSpaces property in NgrammProcessor.
        /// </summary>
        private void IncludeSpacesChanged(object sender, EventArgs e)
        {
            NgrammProcessor.ProcessSpaces = includingSpacesCheckbox.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event for the equal length radio button.
        /// Updates the binning grid using the _dg DataGridView.
        /// </summary>
        private async void radioEqualLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg);
        }

        /// <summary>
        /// Handles the CheckedChanged event for the different length radio button.
        /// Updates the binning grid using the _dg1 DataGridView.
        /// </summary>
        private async void radioDifferentLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg1);
        }

        /// <summary>
        /// Handles the CheckedChanged event for the growing length radio button.
        /// Updates the binning grid using the _dg2 DataGridView.
        /// </summary>
        private async void radioGrowLength_CheckedChanged(object sender, EventArgs e)
        {
            await RadioButtonCheckedChanged(_dg2);
        }

        /// <summary>
        /// Handles the click event for saving the dictionary grid view to a file.
        /// </summary>
        private void saveDictionaryMenuItemClick(object sender, EventArgs e)
        {
            TableExporter.SaveSelectedFile(dictionaryGridView); // Save data from `dataGridView1`
        }

        /// <summary>
        /// Handles the click event for saving the binning grid view to a file.
        /// </summary>
        private void saveBinningFileMenuItemClick(object sender, EventArgs e)
        {
            TableExporter.SaveSelectedFile(binningGridView); // Save data from `dataGridView2`
        }

        /// <summary>
        /// Exports the dictionary grid view to a CSV file.
        /// </summary>
        private void ExportDictionaryCSV(object sender, EventArgs e)
        {
            TableExport.TableExporter.SaveToCSV(dictionaryGridView);
        }

        /// <summary>
        /// Exports the binning grid view to a CSV file.
        /// </summary>
        private void ExportBinningFileCSV(object sender, EventArgs e)
        {
            TableExport.TableExporter.SaveToCSV(binningGridView);
        }

        /// <summary>
        /// Exports the dictionary grid view to an Excel (XLSX) file.
        /// </summary>
        private void ExportDictionaryXLSX(object sender, EventArgs e)
        {
            TableExport.TableExporter.SaveToExcel(dictionaryGridView);
        }

        /// <summary>
        /// Exports the binning grid view to an Excel (XLSX) file.
        /// </summary>
        private void ExportBinningXLSX(object sender, EventArgs e)
        {
            TableExport.TableExporter.SaveToExcel(binningGridView);
        }

        /// <summary>
        /// Handles the click event for counting by symbols.
        /// Clears form data and processes the selected folder by symbols.
        /// </summary>
        private async void countBySymbolsClick(object sender, EventArgs e)
        {
            clearFormData("Символи", "Кількість символів", "Кількість унікальних символів");

            await OpenFolder(selectedFolderPath, false);
        }

        /// <summary>
        /// Handles the click event for counting by words.
        /// Clears form data and processes the selected folder by words.
        /// </summary>
        private async void countByWordsClick(object sender, EventArgs e)
        {
            clearFormData("Слова", "Кількість слів", "Кількість унікальних слів");

            await OpenFolder(selectedFolderPath, true);
        }

        /// <summary>
        /// Handles the click event for the clear button.
        /// Clears all data and resets the form.
        /// </summary>
        private void clearButtonClick(object sender, EventArgs e)
        {
            ClearData();
            textToProcessLabel.Text = "Текст для обробки: не обрано";
            folderLabel.Text = "Папка: не обрана";
            textsAnalyzedLabel.Text = "Оброблено: 0";
            elapsedTimeLabel.Text = "Час виконання: _";
            countByWordsButton.Enabled = false;
            countBySymbolsButton.Enabled = false;
            
            clearFormData("Джерело не обрано", "Джерело не обрано", "Джерело не обрано");
        }

        /// <summary>
        /// Executes the given action on the UI thread if required.
        /// </summary>
        private void RunOnUiContext(Action action)
        {
            if (InvokeRequired)
                Invoke(action);

            else
                action();

        }

        /// <summary>
        /// Handles progress updates from a progress reporter and updates the progress bar.
        /// </summary>
        private void Reporter_ProgressChanged(object sender, int e)
        {
            RunOnUiContext(() =>
            {
                if (e > progressBar1.Maximum || e < progressBar1.Minimum) return;
                progressBar1.Value = e;
            });
        }

        /// <summary>
        /// Handles the click event for saving the chart as a PNG image.
        /// </summary>
        private void SaveChartClick(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png";
                saveDialog.Title = "Save Chart as PNG";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    parsingResultsChart.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                    MessageBox.Show("Експорт завершено успішно.", "Експорт PNG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Handles the click event for the experimental binning search button.
        /// Finds the best base for log-linear regression.
        /// </summary>
        private void expBinningSearchButtonClick(object sender, EventArgs e)
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

        /// <summary>
        /// Handles the click event for opening a folder.
        /// Opens a folder browser dialog and sets the selected folder path.
        /// </summary>
        private void OpenFolderMenuItemClick(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new CommonOpenFileDialog())
            {
                folderBrowserDialog.Title = "Виберіть папку з текстами";
                folderBrowserDialog.IsFolderPicker = true;
                folderBrowserDialog.RestoreDirectory = true;
                if (folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok && FolderChecker.IsValidFolder(folderBrowserDialog.FileName))
                {

                    selectedFolderPath = folderBrowserDialog.FileName;
                    string folderName = Path.GetFileName(selectedFolderPath.TrimEnd(Path.DirectorySeparatorChar));

                    folderLabel.Text = "Папка: " + folderBrowserDialog.FileName;

                    countByWordsButton.Enabled = true;
                    countBySymbolsButton.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Handles the SortCompare event for the dictionary grid view.
        /// Provides custom sorting logic for specific columns.
        /// </summary>
        private void dictionaryGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            switch (e.Column.Name)
            {
                case "Count":
                    {
                        if (!int.TryParse(e.CellValue1.ToString(), out var a))
                            a = 0;
                        if (!int.TryParse(e.CellValue2.ToString(), out var b))
                            b = 0;

                        e.SortResult = a.CompareTo(b);
                        break;
                    }
                case "Unique":
                    {
                        var a = DoubleParsers.doubleParseFromString(e.CellValue1.ToString());
                        var b = DoubleParsers.doubleParseFromString(e.CellValue2.ToString());
                        e.SortResult = DoubleParsers.CompareDouble(a, b, 1e-12);
                        break;
                    }
                case "NameDG":
                    e.SortResult = String.Compare(
                        e.CellValue1.ToString(),
                        e.CellValue2.ToString(),
                        CultureInfo.CurrentCulture,
                        CompareOptions.IgnoreCase
                    );
                    break;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Clears and resets the form's data and UI elements related to text analysis.
        /// </summary>
        /// <param name="chartName">The name to set for the chart series.</param>
        /// <param name="countColumnName">The header text for the "Count" column in the dictionary grid view.</param>
        /// <param name="uniqueColumnName">The header text for the "Unique" column in the dictionary grid view.</param>
        private void clearFormData(string chartName, string countColumnName, string uniqueColumnName)
        {
            //parsingResultsChart.ChartAreas[0].AxisX.IsLogarithmic = false;

            dictionaryGridView.Rows.Clear();
            binningGridView.Rows.Clear();
            parsingResultsChart.Series[0].Points.Clear();

            textsAnalyzedLabel.Text = string.Empty;

            parsingResultsChart.Legends[0].Title = chartName;
            parsingResultsChart.Series[0].Name = countColumnName;
            dictionaryGridView.Columns["Count"].HeaderText = countColumnName;
            dictionaryGridView.Columns["Unique"].HeaderText = uniqueColumnName;

            x = null;
            y = null;
            avgX = null;
            avgY = null;
            avgQuadY = null;
            textCount = null;
            textCount2 = null;
            textCount3 = null;
            parsedTexts = 0;

        }

        /// <summary>
        /// Handles the click event for opening a dictionary file.
        /// Loads the dictionary data into the grid view and updates the chart.
        /// </summary>
        private async void openDictionaryMenuItemClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            dialog.Title = "Виберіть файл зі словником";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = File.ReadAllLines(dialog.FileName);
                    dictionaryGridView.Rows.Clear();
                    x = new double[lines.Length];
                    y = new double[lines.Length];

                    for (int i = 0; i < lines.Length; i++)
                    {

                        var parts = lines[i].Split('\t');


                        if (parts.Length >= 3)
                        {
                            var (valX, valY) = DoubleParsers.doubleParseFromStrings(lines[i]);
                            dictionaryGridView.Rows.Add(parts[0], valX, valY);
                            x[i] = valX;
                            y[i] = valY;
                        }
                    }

                    parsedTexts = x.Length;
                    textsAnalyzedLabel.Text = $"Кількість текстів: {parsedTexts}";

                    // Побудова графіка
                    var xArray = x;
                    var yArray = y;

                    parsingResultsChart.Series[0].Points.Clear();



                    parsingResultsChart.ChartAreas[0].AxisX.Title = "Кількість слів";
                    parsingResultsChart.ChartAreas[0].AxisY.Title = "Частка унікальних слів";

                    for (int i = 0; i < xArray.Length; i++)
                    {
                        parsingResultsChart.Series[0].Points.AddXY(xArray[i], yArray[i]);
                    }

                    // Оновити бінування після завантаження
                    updateButton.Enabled = true;
                    updateButton.PerformClick();
                    updateButton.Enabled = false;
                    textToProcessLabel.Text = $"Обрано словник";
                    folderLabel.Text =  $"Словник: {Path.GetFileNameWithoutExtension(dialog.FileName)}" ;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при відкритті файлу: " + ex.Message);
                }
            }

        }

        /// <summary>
        /// Handles the click event for opening a binning file.
        /// Loads the binning data into the grid view.
        /// </summary>
        private void OpenBinningFileMenuItemClick(object sender, EventArgs e)
        {
            var dgv = binningGridView;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            //for files in current folder
            try
            {
                dgv.ColumnCount = 7;
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

            parsedTexts = File.ReadLines(fbd.FileName).Count();
            textsAnalyzedLabel.Text = Convert.ToString("Текстів: " + parsedTexts);
            int M = Convert.ToInt32(binQuantityUpDown.Text);

        }

        /// <summary>
        /// Handles the click event for updating the binning grid.
        /// Recalculates binning based on the selected method and updates the grid.
        /// </summary>
        private async void updateButtonClick(object sender, EventArgs e)
        {
            binningGridView.Rows.Clear();

            int M = (int)binQuantityUpDown.Value;


            if (!double.TryParse(powAUpDown.Value.ToString(), out var basePow))
            {
                MessageBox.Show("Введіть основу степеня!");
                return;
            }

            var (x, y) = LoadDataFromGrid(dictionaryGridView);


            if (M >= parsedTexts)
            {
                MessageBox.Show("Кількість бінів має бути меншою за кількість рядків у словнику.");
                return;
            }

            if (equalLengthRadio.Checked)
            {
                double step = (x.Max() - x.Min()) / (double)M;
                AverageMethod(x, y, M, 0, AverageType.FirstAverage, out avgX, out avgY, out avgQuadY, out textCount);
                for (int i = 0; i < M; i++)
                {
                    binningGridView.Rows.Add(i + 1, x.Min() + i * step, x.Min() + (i + 1) * step, avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }
            }
            else if (differentLengthRadio.Checked)
            {
                int step = x.Length / M;  // Цілий розмір перших бінів
                int remainder = x.Length % M;  // Залишок

                avgX = new double[M];
                avgY = new double[M];
                avgQuadY = new double[M];
                textCount2 = new int[M];
                Array.Sort(x, y);
                int totalAssigned = 0;

                for (int i = 0; i < M; i++)
                {
                    int currentBinSize = (i == M - 1) ? remainder + step : step; // Додаємо залишок в останній бін

                    int start = totalAssigned;
                    int end = start + currentBinSize;

                    double sumX = 0, sumY = 0, sumY2 = 0;
                    int count = 0;

                    for (int j = start; j < end; j++)
                    {
                        sumX += x[j];
                        sumY += y[j];
                        sumY2 += y[j] * y[j];
                        count++;
                    }

                    double avg_x = count > 0 ? sumX / count : 0;
                    double avg_y = count > 0 ? sumY / count : 0;
                    double std_y = (count > 1) ? Math.Sqrt((sumY2 - count * avg_y * avg_y) / (count - 1)) : 0;

                    avgX[i] = avg_x;
                    avgY[i] = avg_y;
                    avgQuadY[i] = std_y;
                    textCount2[i] = count;

                    binningGridView.Rows.Add(i + 1, x[start], x[end - 1], avg_x, avg_y, std_y, count);

                    totalAssigned = end;
                }

            }
            else if (growingLengthRadio.Checked)
            {
                Array.Sort(x, y);

                double maxValue = x.Max();
                int actualSteps = 0;

                // Визначаємо кількість бінів залежно від основи A та максимуму
                while (Math.Pow(basePow, actualSteps + 1) <= maxValue)
                {
                    actualSteps++;
                }
                actualSteps++; // включаємо останній бін, що охоплює maxValue

                // Обчислення
                AverageMethod(x, y, actualSteps, basePow, AverageType.ThirdAverage,
                    out avgX, out avgY, out avgQuadY, out textCount3);

                // Збираємо всі рядки для таблиці
                var rows = new List<(int binIndex, double left, double right, double avgX, double avgY, double stdY, int count)>();

                for (int i = 0; i < actualSteps; i++)
                {
                    double left = Math.Pow(basePow, i);
                    double right = Math.Pow(basePow, i + 1);

                    rows.Add((
                        i + 1,
                        Math.Round(left, 5),
                        Math.Round(right, 5),
                        avgX[i],
                        avgY[i],
                        avgQuadY[i],
                        textCount3[i]
                    ));
                }

                // Очищаємо таблицю перед оновленням
                binningGridView.Rows.Clear();

                // Додаємо у правильному порядку
                foreach (var row in rows.OrderBy(r => r.binIndex))
                {
                    binningGridView.Rows.Add(row.binIndex, row.left, row.right, row.avgX, row.avgY, row.stdY, row.count);
                }
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Adds a row to the dictionary grid view with the specified values.
        /// </summary>
        private void AddToDataGrid(string name, double count, double unique)
        {
            dictionaryGridView.Rows.Add(name, count.ToString(CultureInfo.InvariantCulture), unique.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Initializes resources required for Asian language parsing.
        /// </summary>
        private void InitializeAsianParsingResources()
        {

            // Initialize JiebaNet for Chinese and NgrammProcessor for Japanese
            if (FolderChecker.IsValidFolder(ChineeseParsingResourceFolder))
                JiebaNet.Segmenter.ConfigManager.ConfigFileBaseDir = ChineeseParsingResourceFolder;
            NgrammProcessor.InitializeJapaneseProcessing(JapaneseParsingResourceFolder);
        }

        /// <summary>
        /// Handles the logic for when a radio button is checked, updating the binning grid.
        /// </summary>
        /// <param name="dataGrid">The DataGridView to use for updating.</param>
        private async Task RadioButtonCheckedChanged(DataGridView dataGrid)
        {
            if (binningGridView.Rows.Count <= 2)
                return;
            binningGridView.Rows.Clear();
            ClearBinningData();

            await UpdateDataGrid(_dg);
        }

        /// <summary>
        /// Clears all binning data from the internal DataGridViews.
        /// </summary>
        private void ClearBinningData()
        {
            _dg.ColumnCount = 7;
            _dg1.ColumnCount = 7;
            _dg2.ColumnCount = 7;
            _dg.Rows.Clear();
            _dg1.Rows.Clear();
            _dg2.Rows.Clear();
        }

        /// <summary>
        /// Adds a row to the specified DataGridView asynchronously.
        /// </summary>
        /// <param name="dataGridView">The DataGridView to add the row to.</param>
        /// <param name="items">The items to add as a row.</param>
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

        /// <summary>
        /// Clears all data and resets the form state.
        /// </summary>
        private void ClearData()
        {
            ClearBinningData();
            selectedFolderPath = string.Empty;
            x = null;
            y = null;
            avgX = null;
            avgY = null;
            avgQuadY = null;
            textCount = null;
            textCount2 = null;
            textCount3 = null;
        }

        /// <summary>
        /// Updates the binning grid with sorted data from the specified DataGridView.
        /// </summary>
        /// <param name="dataGrid">The DataGridView to read data from.</param>
        private async Task UpdateDataGrid(DataGridView dataGrid)
        {
            var rows = dataGrid.Rows.Cast<DataGridViewRow>()
            .Where(r => r.Cells[0].Value != null)
            .OrderBy(r => r.Cells[0].Value.ToString()) // Сортування за першим стовпцем
            .ToList();

            foreach (var row in rows)
            {
                var items = new object[row.Cells.Count];
                for (var i = 0; i < row.Cells.Count; i++)
                {
                    items[i] = row.Cells[i].Value;
                }
                await AddRowAsync(binningGridView, items);
            }
        }

        /// <summary>
        /// Loads X and Y data arrays from the specified DataGridView.
        /// </summary>
        /// <param name="gv">The DataGridView to load data from.</param>
        /// <returns>Tuple of X and Y double arrays.</returns>
        private (double[], double[]) LoadDataFromGrid(DataGridView gv)
        {
            if (gv.Rows.Count > 0)
            {
                var xList = new List<double>();
                var yList = new List<double>();

                foreach (DataGridViewRow row in gv.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {

                        xList.Add(DoubleParsers.doubleParseFromString(row.Cells[1].Value.ToString()));
                        yList.Add(DoubleParsers.doubleParseFromString(row.Cells[2].Value.ToString()));
                    }
                }

                x = xList.ToArray();
                y = yList.ToArray();
                parsedTexts = x.Length;
                textsAnalyzedLabel.Text = $"Кількість текстів: {parsedTexts}";
                return (x, y);
            }
            else
            {
                MessageBox.Show("Словник не завантажено.");
                return (null, null);
            }
        }

        /// <summary>
        /// Recursively processes a directory and returns a list of all file paths.
        /// </summary>
        /// <param name="targetDirectory">The directory to process.</param>
        /// <returns>List of file paths.</returns>
        private static async Task<List<string>> ProcessDirectoryAsync(string targetDirectory)
        {
            try
            {
                if (!FolderChecker.IsValidFolder(targetDirectory))
                    throw new Exception("Invalid folder path or folder does not exist. ");
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
                MessageBox.Show(e.Message + "Error loading directory");
                throw;
            }
        }

        /// <summary>
        /// Calculates the coefficient of determination for log-linear regression with the specified base.
        /// </summary>
        /// <param name="basePow">The base for the logarithm.</param>
        /// <returns>Coefficient of determination (R^2).</returns>
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

        /// <summary>
        /// Calculates averages and standard deviations for binning data.
        /// </summary>
        /// <param name="x">X data array.</param>
        /// <param name="y">Y data array.</param>
        /// <param name="maxStep">Number of bins or steps.</param>
        /// <param name="basePow">Base for logarithmic binning.</param>
        /// <param name="type">Type of averaging to use.</param>
        /// <param name="L">Output: average X values per bin.</param>
        /// <param name="V">Output: average Y values per bin.</param>
        /// <param name="dV">Output: standard deviation of Y per bin.</param>
        /// <param name="textCount">Output: number of items per bin.</param>
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
                if (count > 1)
                {
                    dV[i] = Math.Sqrt(count * (avgQuadV - Math.Pow(avgResV, 2)) / (count - 1));
                }
                else
                {
                    dV[i] = 0; // Not enough data points for standard deviation
                }
                L[i] = avgResL;
                V[i] = avgResV;
            }
        }

        /// <summary>
        /// Processes the selected folder and updates the UI with analysis results.
        /// </summary>
        /// <param name="folderPath">The path to the folder to process.</param>
        /// <param name="byWords">If true, process by words; otherwise, by symbols.</param>
        private async Task OpenFolder(string folderPath, bool byWords)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please select a folder first.");
                return;
            }
            elapsedTimeLabel.Text = "Час виконання: Виконується";
            textsAnalyzedLabel.Text = string.Empty;
            parsedTexts = 0;



            ProgressReporter reporter = new ProgressReporter();
            reporter.ProgressChanged += Reporter_ProgressChanged;


            var files = new List<string>();

            // processing files
            try
            {
                files = await ProcessDirectoryAsync(folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            var xlist = new double[files.Count];
            var ylist = new double[files.Count];



            // calculations for tables
            await Task.WhenAll(files.Select((file, index) => Task.Run(async () =>
            {
                Invoke((Action)(() =>
                {
                    ++parsedTexts;
                    textsAnalyzedLabel.Text = $"Оброблено: {parsedTexts} з {files.Count}";
                    textToProcessLabel.Text = $"Текст: {Path.GetFileNameWithoutExtension(file)}";
                    reporter.Reset();

                }));


                NgrammProcessor processor = new NgrammProcessor(file, reporter);
                await processor.Preprocess();



                if (byWords)
                {
                    await processor.ProcessWordNGramms(1);
                    xlist[index] = processor.GetWordsCount();
                    ylist[index] = processor.GetWordsNgrams().ElementAt(0).absCount;
                }
                else
                {

                    await processor.ProcessSymbolNGramms(1);
                    xlist[index] = processor.GetSymbolsCount(NgrammProcessor.ProcessSpaces);
                    ylist[index] = processor.GetSymbolNgrams().ElementAt(0).absCount;
                }



            })));
            textsAnalyzedLabel.Text = "Перенесення данних у UI";


            var xList = new ArrayList(xlist.ToArray());
            var yList = new ArrayList(ylist.ToArray());


            // adding data to the grid
            for (var i = 0; i < xList.Count; i++)
            {
                parsingResultsChart.Series[0].Points.AddXY(xList[i], yList[i]);
                AddToDataGrid(Path.GetFileNameWithoutExtension(files[i]), xlist[i], ylist[i]);
            }

            dictionaryGridView.Columns["Count"].ValueType = typeof(Int32);

            await BinningScript(xList, yList);

            stopwatch.Stop();

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            stopwatch.Elapsed.TotalHours, stopwatch.Elapsed.TotalMinutes, stopwatch.Elapsed.TotalSeconds);
            this.elapsedTimeLabel.Text = $"Час виконання: {elapsedTime}";
            textsAnalyzedLabel.Text = $"Оброблено: {parsedTexts} з {files.Count}";


        }
        private void setBinningSettingsEnabled(bool enabled) {
            equalLengthRadio.Enabled = enabled;
            differentLengthRadio.Enabled = enabled;
            growingLengthRadio.Enabled = enabled;
            binQuantityUpDown.Enabled = enabled;
            powAUpDown.Enabled = enabled;
            updateButton.Enabled = enabled;
            expBinningSearchButton.Enabled = enabled;
        }


        private void switchBinningSettings(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 2)            
                setBinningSettingsEnabled(false);
            else
                setBinningSettingsEnabled(true);

        }

        /// <summary>
        /// Performs binning on the provided X and Y lists and updates the binning DataGridViews.
        /// </summary>
        /// <param name="xList">List of X values.</param>
        /// <param name="yList">List of Y values.</param>
        private async Task BinningScript(ArrayList xList, ArrayList yList)
        {
            ClearBinningData();
            var binCount = ((int)binQuantityUpDown.Value);
            x = xList.ToArray(typeof(double)) as double[];
            y = yList.ToArray(typeof(double)) as double[];



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
                    _dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step), Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }



                AverageMethod(x, y, maxSteps, basePow, AverageType.ThirdAverage, out avgX, out avgY, out avgQuadY, out textCount3);
                int stepN = 0;

                for (int i = 0; i < maxSteps; i++)
                {
                    stepN = i;

                    _dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)), Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
                }

                int step1 = (int)x.Length / binCount;

                AverageMethod(x, y, binCount, 0, AverageType.SecondAverage, out avgX, out avgY, out avgQuadY, out textCount2);

                for (int i = 0; i < binCount; i++)
                {
                    //MessageBox.Show($"AVG2 ITER = {i}; {x[i * step1]} - {x[(step1 * (i + 1) - 1)]}; Step = {step1}");
                    _dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]), Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i], textCount2[i]);
                }


                var selectedDataGrid = equalLengthRadio.Checked ? _dg :
                    differentLengthRadio.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }
        }

        #endregion
    }
}






