namespace VolosIndiv
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.countByWordsButton = new System.Windows.Forms.Button();
            this.fbd = new System.Windows.Forms.OpenFileDialog();
            this.saveFiles = new System.Windows.Forms.SaveFileDialog();
            this.clearButton = new System.Windows.Forms.Button();
            this.textsAnalyzedLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.equalLengthRadio = new System.Windows.Forms.RadioButton();
            this.differentLengthRadio = new System.Windows.Forms.RadioButton();
            this.growingLengthRadio = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.expBinningSearchButton = new System.Windows.Forms.Button();
            this.countBySymbolsButton = new System.Windows.Forms.Button();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.ignoreRegexCheckbox = new System.Windows.Forms.CheckBox();
            this.includingSpaces = new System.Windows.Forms.CheckBox();
            this.folderLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.binQuantityUpDown = new System.Windows.Forms.NumericUpDown();
            this.powAUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.binningGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RightBorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dictionaryGridView = new System.Windows.Forms.DataGridView();
            this.NameDG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unique = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.parsingResultsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.відкритиПапкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.папкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.словникToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлБінуванняToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.зберегтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.словникToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.файлБінуванняToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binQuantityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powAUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binningGridView)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dictionaryGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parsingResultsChart)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // countByWordsButton
            // 
            this.countByWordsButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.countByWordsButton.Enabled = false;
            this.countByWordsButton.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.countByWordsButton.Location = new System.Drawing.Point(13, 49);
            this.countByWordsButton.Name = "countByWordsButton";
            this.countByWordsButton.Size = new System.Drawing.Size(194, 30);
            this.countByWordsButton.TabIndex = 0;
            this.countByWordsButton.Text = "Слова";
            this.countByWordsButton.UseVisualStyleBackColor = true;
            this.countByWordsButton.Click += new System.EventHandler(this.countByWordsClick);
            // 
            // fbd
            // 
            this.fbd.AddExtension = false;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearButton.Location = new System.Drawing.Point(9, 417);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(216, 30);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Очистити";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButtonClick);
            // 
            // textsAnalyzedLabel
            // 
            this.textsAnalyzedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textsAnalyzedLabel.AutoSize = true;
            this.textsAnalyzedLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textsAnalyzedLabel.Location = new System.Drawing.Point(6, 55);
            this.textsAnalyzedLabel.Name = "textsAnalyzedLabel";
            this.textsAnalyzedLabel.Size = new System.Drawing.Size(88, 18);
            this.textsAnalyzedLabel.TabIndex = 6;
            this.textsAnalyzedLabel.Text = "Текстів: 0";
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.updateButton.Location = new System.Drawing.Point(9, 379);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(216, 30);
            this.updateButton.TabIndex = 9;
            this.updateButton.Text = "Оновити";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButtonClick);
            // 
            // equalLengthRadio
            // 
            this.equalLengthRadio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.equalLengthRadio.AutoSize = true;
            this.equalLengthRadio.Checked = true;
            this.equalLengthRadio.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.equalLengthRadio.Location = new System.Drawing.Point(9, 171);
            this.equalLengthRadio.Name = "equalLengthRadio";
            this.equalLengthRadio.Size = new System.Drawing.Size(186, 22);
            this.equalLengthRadio.TabIndex = 10;
            this.equalLengthRadio.TabStop = true;
            this.equalLengthRadio.Text = "З однаковою довжиною";
            this.equalLengthRadio.UseVisualStyleBackColor = true;
            this.equalLengthRadio.CheckedChanged += new System.EventHandler(this.radioEqualLength_CheckedChanged);
            // 
            // differentLengthRadio
            // 
            this.differentLengthRadio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.differentLengthRadio.AutoSize = true;
            this.differentLengthRadio.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.differentLengthRadio.Location = new System.Drawing.Point(9, 201);
            this.differentLengthRadio.Name = "differentLengthRadio";
            this.differentLengthRadio.Size = new System.Drawing.Size(162, 22);
            this.differentLengthRadio.TabIndex = 11;
            this.differentLengthRadio.Text = "Різні за довжиною";
            this.differentLengthRadio.UseVisualStyleBackColor = true;
            this.differentLengthRadio.CheckedChanged += new System.EventHandler(this.radioDifferentLength_CheckedChanged);
            // 
            // growingLengthRadio
            // 
            this.growingLengthRadio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.growingLengthRadio.AutoSize = true;
            this.growingLengthRadio.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.growingLengthRadio.Location = new System.Drawing.Point(9, 231);
            this.growingLengthRadio.Name = "growingLengthRadio";
            this.growingLengthRadio.Size = new System.Drawing.Size(162, 22);
            this.growingLengthRadio.TabIndex = 14;
            this.growingLengthRadio.Text = "Зростаюча довжина";
            this.growingLengthRadio.UseVisualStyleBackColor = true;
            this.growingLengthRadio.CheckedChanged += new System.EventHandler(this.radioGrowLength_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Основа степеня А";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(9, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 18);
            this.label3.TabIndex = 17;
            this.label3.Text = "Кількість бінів";
            // 
            // expBinningSearchButton
            // 
            this.expBinningSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expBinningSearchButton.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.expBinningSearchButton.Location = new System.Drawing.Point(9, 455);
            this.expBinningSearchButton.Name = "expBinningSearchButton";
            this.expBinningSearchButton.Size = new System.Drawing.Size(216, 75);
            this.expBinningSearchButton.TabIndex = 20;
            this.expBinningSearchButton.Text = "Основа експоненційного бінування";
            this.expBinningSearchButton.UseVisualStyleBackColor = true;
            this.expBinningSearchButton.Click += new System.EventHandler(this.expBinningSearchButtonClick);
            // 
            // countBySymbolsButton
            // 
            this.countBySymbolsButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.countBySymbolsButton.Enabled = false;
            this.countBySymbolsButton.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.countBySymbolsButton.Location = new System.Drawing.Point(13, 19);
            this.countBySymbolsButton.Name = "countBySymbolsButton";
            this.countBySymbolsButton.Size = new System.Drawing.Size(194, 30);
            this.countBySymbolsButton.TabIndex = 21;
            this.countBySymbolsButton.Text = "Символи";
            this.countBySymbolsButton.UseVisualStyleBackColor = true;
            this.countBySymbolsButton.Click += new System.EventHandler(this.countBySymbolsClick);
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elapsedTimeLabel.AutoSize = true;
            this.elapsedTimeLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.elapsedTimeLabel.Location = new System.Drawing.Point(512, 26);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(120, 18);
            this.elapsedTimeLabel.TabIndex = 23;
            this.elapsedTimeLabel.Text = "Час виконання:";
            // 
            // ignoreRegexCheckbox
            // 
            this.ignoreRegexCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ignoreRegexCheckbox.AutoSize = true;
            this.ignoreRegexCheckbox.Checked = true;
            this.ignoreRegexCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignoreRegexCheckbox.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ignoreRegexCheckbox.Location = new System.Drawing.Point(9, 16);
            this.ignoreRegexCheckbox.Name = "ignoreRegexCheckbox";
            this.ignoreRegexCheckbox.Size = new System.Drawing.Size(171, 22);
            this.ignoreRegexCheckbox.TabIndex = 24;
            this.ignoreRegexCheckbox.Text = "Ігнорувати регістр";
            this.ignoreRegexCheckbox.UseVisualStyleBackColor = true;
            this.ignoreRegexCheckbox.CheckedChanged += new System.EventHandler(this.IgnoreRegexChanged);
            // 
            // includingSpaces
            // 
            this.includingSpaces.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.includingSpaces.AutoSize = true;
            this.includingSpaces.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.includingSpaces.Location = new System.Drawing.Point(9, 46);
            this.includingSpaces.Margin = new System.Windows.Forms.Padding(2);
            this.includingSpaces.Name = "includingSpaces";
            this.includingSpaces.Size = new System.Drawing.Size(179, 22);
            this.includingSpaces.TabIndex = 26;
            this.includingSpaces.Text = "Враховувати пробіли";
            this.includingSpaces.UseVisualStyleBackColor = true;
            this.includingSpaces.CheckedChanged += new System.EventHandler(this.IncludeSpacesChanged);
            // 
            // folderLabel
            // 
            this.folderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.folderLabel.AutoSize = true;
            this.folderLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.folderLabel.Location = new System.Drawing.Point(512, 55);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(56, 18);
            this.folderLabel.TabIndex = 27;
            this.folderLabel.Text = "Папка:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.binQuantityUpDown);
            this.groupBox1.Controls.Add(this.powAUpDown);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.ignoreRegexCheckbox);
            this.groupBox1.Controls.Add(this.includingSpaces);
            this.groupBox1.Controls.Add(this.expBinningSearchButton);
            this.groupBox1.Controls.Add(this.clearButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.updateButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.equalLengthRadio);
            this.groupBox1.Controls.Add(this.growingLengthRadio);
            this.groupBox1.Controls.Add(this.differentLengthRadio);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(676, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 541);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // binQuantityUpDown
            // 
            this.binQuantityUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.binQuantityUpDown.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.binQuantityUpDown.Location = new System.Drawing.Point(9, 346);
            this.binQuantityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.binQuantityUpDown.Name = "binQuantityUpDown";
            this.binQuantityUpDown.Size = new System.Drawing.Size(216, 25);
            this.binQuantityUpDown.TabIndex = 33;
            this.binQuantityUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // powAUpDown
            // 
            this.powAUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.powAUpDown.DecimalPlaces = 1;
            this.powAUpDown.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.powAUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.powAUpDown.Location = new System.Drawing.Point(9, 287);
            this.powAUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.powAUpDown.Name = "powAUpDown";
            this.powAUpDown.Size = new System.Drawing.Size(216, 25);
            this.powAUpDown.TabIndex = 32;
            this.powAUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.countByWordsButton);
            this.groupBox2.Controls.Add(this.countBySymbolsButton);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(9, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 87);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Рахувати";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.binningGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(650, 513);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Бінування";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // binningGridView
            // 
            this.binningGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.binningGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.binningGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.RightBorder,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.binningGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binningGridView.Location = new System.Drawing.Point(0, 0);
            this.binningGridView.Name = "binningGridView";
            this.binningGridView.RowHeadersVisible = false;
            this.binningGridView.RowHeadersWidth = 51;
            this.binningGridView.Size = new System.Drawing.Size(650, 513);
            this.binningGridView.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Бін #";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Ліва Межа";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // RightBorder
            // 
            this.RightBorder.HeaderText = "Права Межа";
            this.RightBorder.Name = "RightBorder";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Ls";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Vs";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "dV";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "К-сть текстів";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dictionaryGridView);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(650, 513);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Словник";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dictionaryGridView
            // 
            this.dictionaryGridView.AllowUserToAddRows = false;
            this.dictionaryGridView.AllowUserToDeleteRows = false;
            this.dictionaryGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dictionaryGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dictionaryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dictionaryGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameDG,
            this.Count,
            this.Unique});
            this.dictionaryGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dictionaryGridView.Location = new System.Drawing.Point(3, 3);
            this.dictionaryGridView.Name = "dictionaryGridView";
            this.dictionaryGridView.ReadOnly = true;
            this.dictionaryGridView.RowHeadersWidth = 51;
            this.dictionaryGridView.Size = new System.Drawing.Size(644, 507);
            this.dictionaryGridView.TabIndex = 1;
            this.dictionaryGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView1_SortCompare);
            // 
            // NameDG
            // 
            this.NameDG.HeaderText = "Назва";
            this.NameDG.MinimumWidth = 6;
            this.NameDG.Name = "NameDG";
            this.NameDG.ReadOnly = true;
            // 
            // Count
            // 
            this.Count.HeaderText = "Кількість слів";
            this.Count.MinimumWidth = 6;
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // Unique
            // 
            this.Unique.HeaderText = "Кількість різних слів";
            this.Unique.MinimumWidth = 6;
            this.Unique.Name = "Unique";
            this.Unique.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.parsingResultsChart);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(650, 513);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Графік";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // parsingResultsChart
            // 
            chartArea1.Name = "ChartArea1";
            this.parsingResultsChart.ChartAreas.Add(chartArea1);
            this.parsingResultsChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            legend1.Title = "Парсинг";
            legend1.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.parsingResultsChart.Legends.Add(legend1);
            this.parsingResultsChart.Location = new System.Drawing.Point(3, 3);
            this.parsingResultsChart.Name = "parsingResultsChart";
            this.parsingResultsChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            series1.Legend = "Legend1";
            series1.Name = "Кількість";
            this.parsingResultsChart.Series.Add(series1);
            this.parsingResultsChart.Size = new System.Drawing.Size(644, 507);
            this.parsingResultsChart.TabIndex = 0;
            this.parsingResultsChart.Text = "chart1";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(5, 26);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(489, 26);
            this.progressBar1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 544);
            this.tabControl1.TabIndex = 5;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.folderLabel);
            this.groupBox5.Controls.Add(this.progressBar1);
            this.groupBox5.Controls.Add(this.elapsedTimeLabel);
            this.groupBox5.Controls.Add(this.textsAnalyzedLabel);
            this.groupBox5.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox5.Location = new System.Drawing.Point(16, 577);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(895, 84);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Виконання";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(921, 27);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.відкритиПапкуToolStripMenuItem,
            this.зберегтиToolStripMenuItem});
            this.файлToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 23);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // відкритиПапкуToolStripMenuItem
            // 
            this.відкритиПапкуToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.папкуToolStripMenuItem,
            this.словникToolStripMenuItem,
            this.файлБінуванняToolStripMenuItem});
            this.відкритиПапкуToolStripMenuItem.Name = "відкритиПапкуToolStripMenuItem";
            this.відкритиПапкуToolStripMenuItem.Size = new System.Drawing.Size(150, 24);
            this.відкритиПапкуToolStripMenuItem.Text = "Відкрити";
            // 
            // папкуToolStripMenuItem
            // 
            this.папкуToolStripMenuItem.Name = "папкуToolStripMenuItem";
            this.папкуToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.папкуToolStripMenuItem.Text = "Папку";
            this.папкуToolStripMenuItem.Click += new System.EventHandler(this.OpenFolderMenuItemClick);
            // 
            // словникToolStripMenuItem
            // 
            this.словникToolStripMenuItem.Name = "словникToolStripMenuItem";
            this.словникToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.словникToolStripMenuItem.Text = "Словник";
            this.словникToolStripMenuItem.Click += new System.EventHandler(this.openDictionaryMenuItemClick);
            // 
            // файлБінуванняToolStripMenuItem
            // 
            this.файлБінуванняToolStripMenuItem.Name = "файлБінуванняToolStripMenuItem";
            this.файлБінуванняToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.файлБінуванняToolStripMenuItem.Text = "Файл бінування";
            this.файлБінуванняToolStripMenuItem.Click += new System.EventHandler(this.OpenBinningFileMenuItemClick);
            // 
            // зберегтиToolStripMenuItem
            // 
            this.зберегтиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.словникToolStripMenuItem1,
            this.файлБінуванняToolStripMenuItem1});
            this.зберегтиToolStripMenuItem.Name = "зберегтиToolStripMenuItem";
            this.зберегтиToolStripMenuItem.Size = new System.Drawing.Size(150, 24);
            this.зберегтиToolStripMenuItem.Text = "Зберегти";
            // 
            // словникToolStripMenuItem1
            // 
            this.словникToolStripMenuItem1.Name = "словникToolStripMenuItem1";
            this.словникToolStripMenuItem1.Size = new System.Drawing.Size(204, 24);
            this.словникToolStripMenuItem1.Text = "Словник";
            this.словникToolStripMenuItem1.Click += new System.EventHandler(this.saveDictionaryMenuItemClick);
            // 
            // файлБінуванняToolStripMenuItem1
            // 
            this.файлБінуванняToolStripMenuItem1.Name = "файлБінуванняToolStripMenuItem1";
            this.файлБінуванняToolStripMenuItem1.Size = new System.Drawing.Size(204, 24);
            this.файлБінуванняToolStripMenuItem1.Text = "Файл бінування";
            this.файлБінуванняToolStripMenuItem1.Click += new System.EventHandler(this.saveBinningFileMenuItemClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 673);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(937, 712);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binQuantityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powAUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.binningGridView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dictionaryGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parsingResultsChart)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label elapsedTimeLabel;

        #endregion

        private System.Windows.Forms.Button countByWordsButton;
        private System.Windows.Forms.OpenFileDialog fbd;
        private System.Windows.Forms.SaveFileDialog saveFiles;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label textsAnalyzedLabel;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.RadioButton equalLengthRadio;
        private System.Windows.Forms.RadioButton differentLengthRadio;
        private System.Windows.Forms.RadioButton growingLengthRadio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button expBinningSearchButton;
        private System.Windows.Forms.Button countBySymbolsButton;
        private System.Windows.Forms.CheckBox ignoreRegexCheckbox;
        private System.Windows.Forms.CheckBox includingSpaces;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView binningGridView;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dictionaryGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameDG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unique;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ProgressBar progressBar1;
        
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.NumericUpDown powAUpDown;
        private System.Windows.Forms.NumericUpDown binQuantityUpDown;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem відкритиПапкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem папкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem словникToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлБінуванняToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem зберегтиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem словникToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem файлБінуванняToolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RightBorder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataVisualization.Charting.Chart parsingResultsChart;
    }
}

