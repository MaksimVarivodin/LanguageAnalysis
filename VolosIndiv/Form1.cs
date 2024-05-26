using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


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
        
		double[] x, y;
		double[] avgX;
		double[] avgY;
		double[] avgQuadY;
		int avgA = 2;
		int avgN = 5;
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
			await OpenFolder(true);
		}
		
		private async void button8_Click(object sender, EventArgs e)
		{
			await OpenFolder(false);
		}
		
		private void button7_Click(object sender, EventArgs e)
		{
			double a = 1.1, b = 3d;
			double eps = 0.00002; //точність від лукавого
			double avalue = 0d, bvalue = 0d;

			for(; a < 3d; a += 0.01)
			{
				avalue = GetLogLinearRegression(a);
                
				if(avalue > bvalue)
				{
					b = a;
					bvalue = avalue;
				}
			}
			MessageBox.Show($"Done! The best base = {b}");
			textBox2.Text = b.ToString(CultureInfo.InvariantCulture);
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
		
		private async void buttonSaveDictionaryFile_Click(object sender, EventArgs e)
		{
			await SaveSelectedFile(dataGridView1);
		}

		private async void buttonSaveBinningFile_Click(object sender, EventArgs e)
		{
			await SaveSelectedFile(dataGridView2);
		}
		
		private void ClearButton_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			dataGridView2.Rows.Clear();
			chart1.Series[0].Points.Clear();
			label1.Text = string.Empty;
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

			int M = Convert.ToInt32(textBox1.Text);
            double basePow = 0d;
            try { basePow = Convert.ToDouble(textBox2.Text); }
            catch
            {
                MessageBox.Show("Введіть основу степеня!");
            }

            if (dataGridView1.RowCount == 0)
				counter = File.ReadLines(fbd.FileName).Count();
			label1.Text = Convert.ToString("Count = " + counter);

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

                var selectedDataGrid = radioButton1.Checked ? _dg :
	                radioButton2.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }

            else
            {
                MessageBox.Show("Папка не містить файлів формату .txt");
            }

            /////////////////
        }

		private async void button5_Click(object sender, EventArgs e)
		{

			ArrayList xList = new ArrayList();
			ArrayList yList = new ArrayList();
            progressBar1.Maximum = 100;
            int processedFiles = 0;
            //for files in current folder
            try
			{
				if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && fbd.FileName.Length > 0)
				{
					string[] lines = System.IO.File.ReadAllLines(fbd.FileName, Encoding.UTF8);
                    processedFiles++;
                    progressBar1.Value = processedFiles;
                    foreach (string line in lines)
					{
						string[] res = Regex.Split(line, "\t");

						xList.Add(Convert.ToDouble(res[1]));
						yList.Add(Convert.ToDouble(res[2]));
					}
                    int progress = processedFiles * 100 / 100;
                    progressBar1.Value = progress;
                }
                
			}
			catch (Exception)
			{
				MessageBox.Show("The process done");
			}

			try
			{
				counter = File.ReadLines(fbd.FileName).Count();
				label1.Text = Convert.ToString("Count = " + counter);
			}
			catch (Exception)
			{
				MessageBox.Show("File was not selected");
			}

			//textBox1.Text = "2";

			int M = Convert.ToInt32(textBox1.Text);
			x = xList.ToArray(typeof(double)) as double[];
			y = yList.ToArray(typeof(double)) as double[];


			_dg.ColumnCount = 5;
			_dg1.ColumnCount = 5;
			_dg.Rows.Clear();
			_dg1.Rows.Clear();


			if (M < counter)
			{


				double step = (x.Max() - x.Min()) / (double)M;

				AverageMethod(x, y, M, 0, AverageType.FirstAverage, out avgX, out avgY, out avgQuadY, out textCount);

				for (int i = 0; i < M; i++)
				{
					_dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i]);
				}


				int step1 = (int)x.Length / M;

				AverageMethod(x, y, M, 0, AverageType.SecondAverage, out avgX, out avgY, out avgQuadY, out textCount2);

				for (int i = 0; i < M; i++)
				{
					_dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]) + " - " + Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i]);
				}


				
				await UpdateDataGrid(_dg);
			}

			else
			{
				MessageBox.Show("Кількість інтервалів не може бути рівна або більша ніж кількість текстів");
				//textBox1.Text = "1";
			}
			/////////////////

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
			if(type is AverageType.SecondAverage) 
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
					avgQuadL /= type is AverageType.SecondAverage ? step : count;;
					avgResV /= type is AverageType.SecondAverage ? step : count;;
					avgQuadV /= type is AverageType.SecondAverage ? step : count;;
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
		
		private async Task OpenFolder(bool byWords)
		{
			label5.Text = "";
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			var folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.ShowDialog();
			label1.Text = "";
			counter = 0;

			var files = new List<string>();
	        
			var processedFiles = 0;
			var progress = 0;
			try
			{
				files = await ProcessDirectoryAsync(folderBrowserDialog.SelectedPath);
				progressBar1.Maximum = files.Count;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			var xlist = new double[files.Count];
			var ylist = new double[files.Count];
			Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (file, state, index) =>
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
			});
			var xList = new ArrayList(xlist.ToArray());
			var yList = new ArrayList(ylist.ToArray());
			for (var i = 0; i < xlist.Length; i++)
			{
				chart1.Series[0].Points.AddXY(xlist[i], ylist[i]);
				AddToDataGrid(Path.GetFileNameWithoutExtension(files[i]), xlist[i], ylist[i]);
			}

			dataGridView1.Columns["Count"].ValueType = typeof(Int32);
			label1.Text = $"Кількість текстів = {counter}";

			await SomeMagic(xList, yList);
			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;

			// Format and display the elapsed time
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			label5.Text += $"Time taken: {elapsedTime}";
		}
		
		private async Task CountWordsAsync(List<string> files, double[] xlist, double[] ylist, int iterationNum)
        {
	        try
	        {
		        string localText;
		        using (var streamReader = new StreamReader(files[iterationNum], Encoding.UTF8))
		        {
			        localText = await streamReader.ReadToEndAsync();
		        }

		        localText = Regex.Replace(localText, @"[^\p{L}\d\s]", "");
		        var collection = Regex.Matches(localText, @"\b\w{1,}\b");
		        var wordsWithoutSpaces = collection.Cast<Match>().Select(m => m.Value).Where(word => !string.IsNullOrWhiteSpace(word));
		        var uniqueMatches = collection.OfType<Match>().Select(m => m.Value).Distinct(StringComparer.CurrentCultureIgnoreCase);

		        xlist[iterationNum] = wordsWithoutSpaces.Count();
		        ylist[iterationNum] = uniqueMatches.Count();
		        progressBar1.Value = iterationNum + 1;
	        }
	        catch (Exception ex)
	        {
		        MessageBox.Show($"Error: {ex.Message}");
	        }
        }
        private async Task CountSymbolsAsync(List<string> files, double[] xlist, double[] ylist, int iterationNum)
        {
	        try
	        {
		        string localText;
		        using (var streamReader = new StreamReader(files[iterationNum], Encoding.UTF8))
		        {
			        localText = await streamReader.ReadToEndAsync();
		        }

		        var length = 0;
		        var uniqueSymbols = new HashSet<string>();
		        /*foreach (var character in localText)
		        {
			        if (character != ' ')
			        {
				        length++;
				        //uniqueSymbols.Add(char.ToLower(character));
				        //uniqueSymbols.Add(char.ToLower(character));
				        //uniqueSymbols.Add(radioButtonCaseSensitive.Checked ? character : char.ToLower(character));
			        }
		        }*/
		        for (int i = 0; i < localText.Length; i++)
		        {
			       
			        if (i - 1 < localText.Length)
			        { 
				        length++;
				        var ch = localText[i];

				        // we do NOT care about processing spaces
				        if (ch != ' ')
				        {
					        uniqueSymbols.Add(ch.ToString().ToLower());
				        }
			        }
		        }

		        xlist[iterationNum] = length;
		        ylist[iterationNum] = uniqueSymbols.Count;
		        progressBar1.Value = iterationNum + 1;
	        }
	        catch (Exception ex)
	        {
		        MessageBox.Show($"Error: {ex.Message}");
	        }
        }
        
        private (string, string) ProcessText(string filename)
		{
		    string rawText;
		    bool ignore_spaces = false, ignore_nlines = false, ignore_ends = false;

		    using (var sr = new StreamReader(filename))
		    {
		        rawText = PreprocessWithRegex(sr.ReadToEnd());
		    }

		    var unsignedTextBuilder = new StringBuilder();
		    var textAsRawBuilder = new StringBuilder();
			var addedToRaw = false;

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
		                ignore_spaces = ignore_ends = ignore_nlines = false;
		            }
		            else if (!ignore_spaces && (symbol == ' ' || symbol == '\t' || symbol == '\u00a0'))
		            {
		                addedToRaw = true;

		                textAsRawBuilder.Append(' ');
		                unsignedTextBuilder.Append(' ');
		                ignore_spaces = true;
		                ignore_ends = false;
		            }
		            else if (symbol == '\n' && !ignore_nlines)
		            {
		                addedToRaw = true;

		                textAsRawBuilder.Append(' ');
		                unsignedTextBuilder.Append(' ');
		                ignore_spaces = true;
		                ignore_nlines = true;
		                ignore_ends = false;
		            }
		            else if (StolenRegexp_ss.Contains(symbol))
		            {
		                addedToRaw = true;
		                textAsRawBuilder.Append(symbol);
		            }
		            else if (symbol == '-')
		            {
		                if (i != 0 && i != rawText.Length - 1)
		                {
		                    if (char.IsLetter(rawText[i - 1]) && char.IsLetter(rawText[i + 1]))
		                    {
		                        addedToRaw = true;
		                        textAsRawBuilder.Append(symbol);
		                        unsignedTextBuilder.Append(symbol);
		                    }
		                }
		            }
		            else if (symbol == '`' || symbol == '\'' || symbol == '’' || symbol == 'ʼ')
		            {
		                addedToRaw = true;
		                textAsRawBuilder.Append('\'');
		                if (i != 0 && i != rawText.Length - 1)
		                {
		                    if (char.IsLetter(rawText[i - 1]) && char.IsLetter(rawText[i + 1]))
							{
		                        unsignedTextBuilder.Append('\'');
		                    }
		                }
		            }
		            else if (Endsigns.Contains(symbol) && !ignore_ends)
		            {
						textAsRawBuilder.Append(symbol);
		                addedToRaw = true;
		                ignore_ends = true;
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
			// do NOT count spaces
			return text.Where(symbol => symbol != ' ').Count(); // text.Length;
		}

		private int GetUniqueSymbolsCount(string text)
		{
			var uniqueSymbols = new HashSet<string>();

			for (int i = 0; i < text.Length; i++)
			{
				if (i - 1 < text.Length)
				{ 
					var ch = text[i];

					// we do NOT care about processing spaces
					if (ch != ' ')
					{
						uniqueSymbols.Add(ch.ToString().ToLower());
					}
				}
			}

			return uniqueSymbols.Count;
		}
		
		private int GetAllWordsCount(string text)
		{
			var parsedWords = text
				.Split(new char[] { ' ' }, StringSplitOptions.None)
				.ToList();

			var lastWord = parsedWords.Last();

			if (lastWord == string.Empty)
			{
				parsedWords.Remove(lastWord);
			}

			return parsedWords.Count;
		}

		private int GetDictionaryCount(string text)
		{
			var words = text
				.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				.ToArray();

			var uniqueWords = new HashSet<string>();

			for (int j = 0; j < words.Length; j++)
			{
				var ngramBuilder = new StringBuilder();
				var word = words[j];

				if (word.Length > 1 && Endsigns.Contains(word[word.Length - 1]))
					word = word.Remove(word.Length - 1);

				if (ngramBuilder.Length == 0)
				{
					ngramBuilder.Append(word);
				}
				else if (!Endsigns.Contains(ngramBuilder[ngramBuilder.Length - 1]))
				{
					ngramBuilder.Append($" {word}");
				}
				else
				{
					throw new Exception("err parsing");
				}

				uniqueWords.Add(ngramBuilder.ToString().ToLower());
			}

			return uniqueWords.Count;
		}

        private async Task SomeMagic(ArrayList xList, ArrayList yList)
        {
	        var binCount = Convert.ToInt32(textBox1.Text);
            x = xList.ToArray(typeof(double)) as double[];
            y = yList.ToArray(typeof(double)) as double[];
            
            _dg.ColumnCount = 6;
            _dg1.ColumnCount = 6;
            _dg2.ColumnCount = 6;
            _dg.Rows.Clear();
            _dg1.Rows.Clear();
            _dg2.Rows.Clear();
            
            if (!double.TryParse(textBox2.Text, out var basePow))
	            MessageBox.Show("Введіть основу степеня!");
            
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


                var selectedDataGrid = radioButton1.Checked ? _dg :
	                radioButton2.Checked ? _dg1 : _dg2;
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

		private async Task SaveSelectedFile(DataGridView dataGridView)
		{
			var saveFile = new SaveFileDialog();

			saveFile.DefaultExt = "*.txt";
			saveFile.Filter = "TXT Files|*.txt";
			
			if (saveFile.ShowDialog() != DialogResult.OK || saveFile.FileName.Length <= 0)
				return;
			
			try
			{
				using (var sw = new StreamWriter(saveFile.FileName))
				{
					for (var i = 0; i < dataGridView.Rows.Count; i++)
					{
						for (var j = 0; j < dataGridView.Columns.Count; j++)
						{
							if (dataGridView.Rows[i].Cells[j].Value != null)
							{
								await sw.WriteAsync(dataGridView.Rows[i].Cells[j].Value.ToString() + "\t");
							}
						}
						await sw.WriteLineAsync("");
					}
				}
				MessageBox.Show("Дані збережено");
			}
			catch (Exception ex)
			{
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
		
		private double GetLogLinearRegression(double basePow)
		{
			var steps = 0;
			double res = 0;
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
				if(avgQuadYList[i] == 0)
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
		
		#endregion
	}
}






