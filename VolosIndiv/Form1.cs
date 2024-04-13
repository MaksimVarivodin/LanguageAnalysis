using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace VolosIndiv
{
	public partial class Form1 : Form
	{
		string ss_regex_or = "\\\\|\\||\"|{|}|\\(|\\)|\\[|\\]|=|\\+|_|~|!|@|#|\\$|…|%|\\^|&|\\*|№|:|,|\\.|\\?|;";
        public Form1()
		{
			InitializeComponent();   
        }

        readonly DataGridView _dg = new DataGridView();
        readonly DataGridView _dg1 = new DataGridView();
        readonly DataGridView _dg2 = new DataGridView();

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

        private async void button1_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			DialogResult result = fbd.ShowDialog();
			label1.Text = "";
			counter = 0;
			ArrayList xList = new ArrayList();
			ArrayList yList = new ArrayList();

            double[] xlist = null, ylist = null ;
			List<string> fils = new List<string>();
			
            int processedFiles = 0;
            /// for subfolderstry
            try
			{
               fils = await ProcessDirectoryAsync(fbd.SelectedPath);
            }
			catch(Exception ex)
			{
				
			}
            progressBar1.Maximum = fils.Count();
			int progress = 0;
            xlist = new double[fils.Count];
            ylist = new double[fils.Count];
            //Parallel.For(0, fils.Count, i => 
            for(int i = 0; i < fils.Count; i++)
            {
				string localText = System.IO.File.ReadAllText(fils[i], Encoding.UTF8);
                localText = Regex.Replace(localText, @"[^\p{L}\d\s]", "");
                var collection = Regex.Matches(localText, @"\b\w{1,}\b");
                var wordsWithoutSpaces = collection.Cast<Match>().Select(m => m.Value).Where(word => !string.IsNullOrWhiteSpace(word));
                var uniqueMatches = collection.OfType<Match>().Select(m => m.Value).Distinct(StringComparer.CurrentCultureIgnoreCase);


					counter++;
                    xlist[i] = wordsWithoutSpaces.Count();
                    ylist[i] = uniqueMatches.Count();

					 processedFiles++;
					 progressBar1.Value = processedFiles;
					collection = null;
	               uniqueMatches = null;
                    localText = null;
					progressBar1.Value = progress++;
            }//);
			
            xList = new ArrayList(xlist.ToArray());
            yList = new ArrayList(ylist.ToArray());
            for (var i = 0; i < xlist.Length; i++)
            {
                chart1.Series[0].Points.AddXY(xlist[i], ylist[i]);
                AddToDataGrid(Path.GetFileNameWithoutExtension(fils[i]), xlist[i], ylist[i]);
            }

            dataGridView1.Columns["Count"].ValueType = typeof(Int32);
			label1.Text = "Кількість текстів = " + counter;


			/////////////////////////////////////////////////////////////////////////
			int M = Convert.ToInt32(textBox1.Text);
			x = xList.ToArray(typeof(double)) as double[];
			y = yList.ToArray(typeof(double)) as double[];


			_dg.ColumnCount = 6;
			_dg1.ColumnCount = 6;
			_dg2.ColumnCount = 6;
			_dg.Rows.Clear();
			_dg1.Rows.Clear();
			_dg2.Rows.Clear();
            double basePow = 0d;
            try { basePow = Convert.ToDouble(textBox2.Text); } 
            catch
            {
                MessageBox.Show("Введіть основу степеня!");
            }


			if (M < xList.Count)
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

				avg(x, y, M, out avgX, out avgY, out avgQuadY, out textCount);

				for (int i = 0; i < M; i++)
				{
					//MessageBox.Show($"AVG ITER = {i}; {x.Min() + i * step} - {x.Min() + (i + 1) * step}; Step = {step}");
					_dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
				}

				int step2 = (int)x.Length / M;

				avg3(x, y, maxSteps, basePow, out avgX, out avgY, out avgQuadY, out textCount3);
				int stepN = 0;

				for (int i = 0; i < maxSteps; i++)
				{
					stepN = i;
					//MessageBox.Show($"AVG3 ITER = {i}; {x.Min() + i * step2} - {x.Min() + (i + 1) * step2}; Step = {step2}");
					_dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)) + " - " + Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
				}

                int step1 = (int)x.Length / M;

                avg2(x, y, M, out avgX, out avgY, out avgQuadY, out textCount2);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG2 ITER = {i}; {x[i * step1]} - {x[(step1 * (i + 1) - 1)]}; Step = {step1}");
                    _dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]) + " - " + Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i], textCount2[i]);
                }


                var selectedDataGrid = radioButton1.Checked ? _dg :
                        radioButton2.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			dataGridView1.ColumnCount = 3;                                                                                                                                                                                                                      //if (File.Exists("..//..//Properties//vini_vici_namaste.wav")){ try { new System.Media.SoundPlayer("..//..//Properties//vini_vici_namaste.wav").Play(); } catch (Exception) { } }//
        }

		private void avg(double[] x, double[] y, int M, out double[] L, out double[] V, out double[] dV, out int[] textCount)
		{
			double step = (x.Max() - x.Min()) / (double)M;
			int count = 0;
			double avgQuadL = 0;
			double avgResL = 0;
			double avgQuadV = 0;
			double avgResV = 0;

			L = new double[M];
			V = new double[M];
			dV = new double[M];
			textCount = new int[M];

			for (int i = 0; i < M; i++)
			{
				avgQuadL = 0; avgQuadV = 0;
				avgResL = 0; avgResV = 0;
				count = 0;
				for (int j = 0; j < x.Length; j++)
				{
					//MessageBox.Show($"AVG: x.Min = {x.Min()}; i = {i}; step = {step}");
					if ((x.Min() + i * step <= x[j]) && (x.Min() + (i + 1) * step >= x[j]))
					{
						//MessageBox.Show($"{x.Min() + i * step} <= {x[j]} && {x.Min() + (i + 1) * step} >= {x[j]}");
						avgResL += x[j];
						avgQuadL += x[j] * x[j];
						avgResV += y[j];
						avgQuadV += y[j] * y[j];
						//MessageBox.Show($"avgResL = {avgResL}; avgQuadL = {avgQuadL}; avgResV = {avgResV}; avgQuadV = {avgQuadV}; ");
						count++;
					}
				}
				if (count != 0)
				{
					avgResL /= count;
					avgQuadL /= count;
					avgResV /= count;
					avgQuadV /= count;
				}
				else
				{
					avgResL = 0;
					avgQuadL = 0;
					avgResV = 0;
					avgQuadV = 0;
				}
				textCount[i] = count;
                //TODO: Test avgQuadV before creating!!!!
                //MessageBox.Show($"avgQuadV = {avgQuadV}; avgResV = {avgResV}; avgResV^2 = {Math.Pow(avgResV, 2)}");
                //if (count < 0) MessageBox.Show($"count = {count}");
				dV[i] = Math.Sqrt(avgQuadV - Math.Pow(avgResV, 2));
				L[i] = avgResL;
				V[i] = avgResV;
                //MessageBox.Show($"bin = {i}; dV = {dV[i]}; L = {L[i]}; V = {V[i]};");
			}

		}

		private void avg2(double[] x, double[] y, int M, out double[] L, out double[] V, out double[] dV, out int[] textCount2)
		{
			double step = (int)x.Length / M;
			int count = 0;
			double avgQuadL = 0;
			double avgResL = 0;
			double avgQuadV = 0;
			double avgResV = 0;

			L = new double[M];
			V = new double[M];
			dV = new double[M];
            textCount2 = new int[M];
            Array.Sort(x, y);

			for (int i = 0; i < M; i++)
			{
				avgQuadL = 0;
                avgQuadV = 0;
				avgResL = 0;
                avgResV = 0;
                count = 0;
                for (int j = 0; j < x.Length; j++)
				{
                    //MessageBox.Show($"AVG2: j = {j}; i = {i}; step = {step}");
                    if ((i * step <= j) && ((i + 1) * step > j))
					{
                        //MessageBox.Show($"{i * step} <= {j} && {(i + 1) * step} >= {j}");
                        avgResL += x[j];
						avgQuadL += x[j] * x[j];
						avgResV += y[j];
						avgQuadV += y[j] * y[j];
						count++;

					}
				}
				if (count != 0)
				{
					avgResL /= step;
					avgQuadL /= step;
					avgResV /= step;
					avgQuadV /= step;
				}
				else
				{
					avgResL = 0;
					avgQuadL = 0;
					avgResV = 0;
					avgQuadV = 0;
				}
                textCount2[i] = count;
                dV[i] = Math.Sqrt(avgQuadV - avgResV * avgResV);
				L[i] = avgResL;
				V[i] = avgResV;

			}


		}

		private void avg3(double[] x, double[] y, int maxSteps, double basePow, out double[] L, out double[] V, out double[] dV, out int[] textCount3)
		{
			double step = 0;
			int count = 0;
            UInt64 avgQuadL = 0;
			double avgResL = 0;
            double avgQuadV = 0;
			double avgResV = 0;

			L = new double[maxSteps];
			V = new double[maxSteps];
			dV = new double[maxSteps];
            textCount3 = new int[maxSteps];

            for (int i = 1; i < maxSteps; i++)
			{
                step = Math.Pow(basePow, i);
				avgQuadL = 0; avgQuadV = 0;
				avgResL = 0; avgResV = 0;
				count = 0;

                for (int j = 0; j < x.Length; j++)
				{
                    if ((Math.Pow(basePow, i) <= x[j]) && (Math.Pow(basePow, i + 1) >= x[j]))
					{
                        //MessageBox.Show($"TRUE: {Math.Pow(basePow, i)} <= {x[j]} <= {Math.Pow(basePow, i + 1)}");
                        avgResL += x[j];
						//avgQuadL += x[j] * x[j];
						avgResV += y[j];
						avgQuadV += y[j] * y[j];
						count++;
					}
                    else
                    {
                        //MessageBox.Show($"FALSE: {Math.Pow(basePow, i)} <= {x[j]} <= {Math.Pow(basePow, i+1)}");
                    }
				}
				if (count != 0)
				{
					avgResL /= count;
					//avgQuadL /= count;
					avgResV /= count;
                    avgQuadV /= (UInt64)count;
				}
				else
				{
					avgResL = 0;
					avgQuadL = 0;
					avgResV = 0;
					avgQuadV = 0;
				}
                textCount3[i] = count;
                //MessageBox.Show(count.ToString());
                dV[i] = Math.Sqrt(avgQuadV - avgResV * avgResV);
				L[i] = avgResL;
				V[i] = avgResV;

			}
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

                avg(x, y, M, out avgX, out avgY, out avgQuadY, out textCount);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG ITER = {i}; {x.Min() + i * step} - {x.Min() + (i + 1) * step}; Step = {step}");
                    _dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }

                int step2 = (int)x.Length / M;

                avg3(x, y, maxSteps, basePow, out avgX, out avgY, out avgQuadY, out textCount3);
                int stepN = 0;

                for (int i = 0; i < maxSteps; i++)
                {
                    stepN = i;
                    //MessageBox.Show($"AVG3 ITER = {i}; {x.Min() + i * step2} - {x.Min() + (i + 1) * step2}; Step = {step2}");
                    _dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)) + " - " + Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
                }

                int step1 = (int)x.Length / M;

                avg2(x, y, M, out avgX, out avgY, out avgQuadY, out textCount2);

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

						xList.Add(Convert.ToDouble(res[2]));
						yList.Add(Convert.ToDouble(res[3]));
					}
                    int progress = processedFiles * 100 / 100;
                    progressBar1.Value = progress;
                }
                
			}
			catch (Exception)
			{
				MessageBox.Show("The process done");
			}

			counter = File.ReadLines(fbd.FileName).Count();
			label1.Text = Convert.ToString("Count = " + counter);

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

				avg(x, y, M, out avgX, out avgY, out avgQuadY, out textCount);

				for (int i = 0; i < M; i++)
				{
					_dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i]);
				}


				int step1 = (int)x.Length / M;

				avg2(x, y, M, out avgX, out avgY, out avgQuadY, out textCount2);

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
            MessageBox.Show(string.Format("Done! The best base = {0}",b));
            textBox2.Text = b.ToString();
        }

        private double GetLogLinearRegression(double basePow)
        {
            int steps = 0;
            double res = 0;
            while (res < x.Max())
            {
                steps++;
                res = Math.Pow(basePow, steps);
            }
            avg3(x, y, steps, basePow, out avgX, out avgY, out avgQuadY, out textCount3);

            List<double> avgQuadYList = avgQuadY.ToList();
            List<double> avgXList = avgX.ToList();

            for (int i = 0; i < avgQuadYList.Count; )
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

            Accord.Statistics.Models.Regression.Linear.OrdinaryLeastSquares sr = new Accord.Statistics.Models.Regression.Linear.OrdinaryLeastSquares();
            var regressionresult = sr.Learn(avgXLog, avgQuadYLog);
            
            return regressionresult.CoefficientOfDetermination(avgXLog, avgQuadYLog);
        }

        private async void button8_Click(object sender, EventArgs e)
		{
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            label1.Text = "";
            counter = 0;
            ArrayList xList = new ArrayList();
            ArrayList yList = new ArrayList();

            double[] xlist = null, ylist = null;
			List<string> fils = new List<string>();
            
            int processedFiles = 0;
			try
			{
				fils = await ProcessDirectoryAsync(fbd.SelectedPath);
			}
			catch(Exception ex)
			{

			}
			progressBar1.Maximum = fils.Count();
			int progress = 0;
            xlist = new double[fils.Count];
            ylist = new double[fils.Count];
            //Parallel.For(0, fils.Count, i => 
            for (int i = 0; i < fils.Count; i++)
            {
                string localText = System.IO.File.ReadAllText(fils[i], Encoding.UTF8);
                int length = 0;
                HashSet<char> uniqueSymbols = new HashSet<char>();
                foreach (char character in localText)
                {
                    if (!char.IsWhiteSpace(character))
                    {
                        length++;
                        uniqueSymbols.Add(character);
                    }
                }
                

                int uniqueMatches = uniqueSymbols.Count; 

                int collection = length;

                counter++;
				xlist[i] = collection;
				ylist[i] = uniqueMatches;

                processedFiles++;
                progressBar1.Value = processedFiles;
				collection = 0;
				uniqueMatches = 0;
                localText = null;
                progressBar1.Value = progress++;
            }//);

            xList = new ArrayList(xlist.ToArray());
            yList = new ArrayList(ylist.ToArray());
            for (int i = 0; i < xlist.Length; i++)
            {
                chart1.Series[0].Points.AddXY(xlist[i], ylist[i]);
            }
            for (int i = 0; i < xlist.Length; i++)
            {
                AddToDataGrid(Path.GetFileNameWithoutExtension(fils[i]), xlist[i], ylist[i]);
            }

            dataGridView1.Columns["Count"].ValueType = typeof(Int32);
            label1.Text = "Кількість текстів = " + counter;


            /////////////////////////////////////////////////////////////////////////
            int M = Convert.ToInt32(textBox1.Text);
            x = xList.ToArray(typeof(double)) as double[];
            y = yList.ToArray(typeof(double)) as double[];


            _dg.ColumnCount = 6;
            _dg1.ColumnCount = 6;
            _dg2.ColumnCount = 6;
            _dg.Rows.Clear();
            _dg1.Rows.Clear();
            _dg2.Rows.Clear();
            double basePow = 0d;
            try { basePow = Convert.ToDouble(textBox2.Text); }
            catch
            {
                MessageBox.Show("Введіть основу степеня!");
            }


            if (M < xList.Count)
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

                avg(x, y, M, out avgX, out avgY, out avgQuadY, out textCount);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG ITER = {i}; {x.Min() + i * step} - {x.Min() + (i + 1) * step}; Step = {step}");
                    _dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i], textCount[i]);
                }

                int step2 = (int)x.Length / M;

                avg3(x, y, maxSteps, basePow, out avgX, out avgY, out avgQuadY, out textCount3);
                int stepN = 0;

                for (int i = 0; i < maxSteps; i++)
                {
                    stepN = i;
                    //MessageBox.Show($"AVG3 ITER = {i}; {x.Min() + i * step2} - {x.Min() + (i + 1) * step2}; Step = {step2}");
                    _dg2.Rows.Add(Convert.ToString(i + 1), Convert.ToString(Math.Pow(basePow, stepN)) + " - " + Convert.ToString(Math.Pow(basePow, stepN + 1)), avgX[i], avgY[i], avgQuadY[i], textCount3[i]);
                }

                int step1 = (int)x.Length / M;

                avg2(x, y, M, out avgX, out avgY, out avgQuadY, out textCount2);

                for (int i = 0; i < M; i++)
                {
                    //MessageBox.Show($"AVG2 ITER = {i}; {x[i * step1]} - {x[(step1 * (i + 1) - 1)]}; Step = {step1}");
                    _dg1.Rows.Add(Convert.ToString(i + 1), Convert.ToString(x[i * step1]) + " - " + Convert.ToString(x[(step1 * (i + 1) - 1)]), avgX[i], avgY[i], avgQuadY[i], textCount2[i]);
                }


                var selectedDataGrid = radioButton1.Checked ? _dg :
	                radioButton2.Checked ? _dg1 : _dg2;
                await UpdateDataGrid(selectedDataGrid);
            }
        }


        private async void button5_Click_1(object sender, EventArgs e)
		{

			ArrayList xList = new ArrayList();
			ArrayList yList = new ArrayList();


			//for files in current folder
			try
			{
				if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && fbd.FileName.Length > 0)
				{
					string[] lines = System.IO.File.ReadAllLines(fbd.FileName, Encoding.UTF8);

					foreach (string line in lines)
					{
                        //string[] res = Regex.Split(line, @"\+");
                        string[] res = Regex.Replace(line.Trim(), @"[^\p{L}0-9\s]", "").Split(' ');



                        xList.Add(Convert.ToDouble(res[0]));
						yList.Add(Convert.ToDouble(res[1]));
					}


				}

			}
			catch (Exception)
			{
				MessageBox.Show("The process done");
			}

			counter = File.ReadLines(fbd.FileName).Count();
			label1.Text = Convert.ToString("Count = " + counter);


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

				avg(x, y, M, out avgX, out avgY, out avgQuadY, out textCount);

				for (int i = 0; i < M; i++)
				{
					_dg.Rows.Add(i + 1, Convert.ToString(x.Min() + i * step) + " - " + Convert.ToString(x.Min() + (i + 1) * step), avgX[i], avgY[i], avgQuadY[i]);
				}


				int step1 = (int)x.Length / M;

				avg2(x, y, M, out avgX, out avgY, out avgQuadY, out textCount2);

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

		#region UIHandlers Methods

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
		#endregion

		#region Private helper methods
		
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
		
		#endregion
		
	}
}






