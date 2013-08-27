namespace Plotter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabIntegration = new System.Windows.Forms.TabPage();
            this.FormulaLabel = new System.Windows.Forms.Label();
            this.OrdinatesLabel = new System.Windows.Forms.Label();
            this.FormulaTextBox = new System.Windows.Forms.TextBox();
            this.OrdinateListView = new System.Windows.Forms.ListView();
            this.XHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.YHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SimpsonsRuleRadioButton = new System.Windows.Forms.RadioButton();
            this.TrapeziumRuleRadioButton = new System.Windows.Forms.RadioButton();
            this.MidOrdRuleRadioButton = new System.Windows.Forms.RadioButton();
            this.StripsLabel = new System.Windows.Forms.Label();
            this.UpperLabel = new System.Windows.Forms.Label();
            this.LowerLabel = new System.Windows.Forms.Label();
            this.LowerLimitTextBox = new System.Windows.Forms.TextBox();
            this.UpperLimitTextBox = new System.Windows.Forms.TextBox();
            this.IterationsTextBox = new System.Windows.Forms.TextBox();
            this.IntegrationResultLabel = new System.Windows.Forms.Label();
            this.TabDifferentiation = new System.Windows.Forms.TabPage();
            this.NormalGradLabel = new System.Windows.Forms.Label();
            this.NormalEquationLabel = new System.Windows.Forms.Label();
            this.TangentLabel = new System.Windows.Forms.Label();
            this.TargetPointLabel = new System.Windows.Forms.Label();
            this.GradientLabel = new System.Windows.Forms.Label();
            this.TargetPointTextBox = new System.Windows.Forms.TextBox();
            this.TabExtrema = new System.Windows.Forms.TabPage();
            this.ExtremaListView = new System.Windows.Forms.ListView();
            this.XCoordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.YCoordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ExtremaTypeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FindExtremaButton = new System.Windows.Forms.Button();
            this.TabIteration = new System.Windows.Forms.TabPage();
            this.FirstIterationLabel = new System.Windows.Forms.Label();
            this.FirstIterationTextBox = new System.Windows.Forms.TextBox();
            this.CalculateIterationButton = new System.Windows.Forms.Button();
            this.ResultsListView = new System.Windows.Forms.ListView();
            this.IterationHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IterationsLabel = new System.Windows.Forms.Label();
            this.NumIterationsUpDown = new System.Windows.Forms.NumericUpDown();
            this.EquationTextBox = new System.Windows.Forms.TextBox();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.ExitButton = new System.Windows.Forms.ToolStripButton();
            this.CopyButton = new System.Windows.Forms.ToolStripButton();
            this.AboutLabel = new System.Windows.Forms.ToolStripLabel();
            this.EquationLabel = new System.Windows.Forms.Label();
            this.StartTimer = new System.Windows.Forms.Timer(this.components);
            this.TabControl.SuspendLayout();
            this.TabIntegration.SuspendLayout();
            this.TabDifferentiation.SuspendLayout();
            this.TabExtrema.SuspendLayout();
            this.TabIteration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumIterationsUpDown)).BeginInit();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.TabIntegration);
            this.TabControl.Controls.Add(this.TabDifferentiation);
            this.TabControl.Controls.Add(this.TabExtrema);
            this.TabControl.Controls.Add(this.TabIteration);
            this.TabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TabControl.Location = new System.Drawing.Point(12, 28);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(891, 619);
            this.TabControl.TabIndex = 0;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControlSelectedIndexChanged);
            // 
            // TabIntegration
            // 
            this.TabIntegration.Controls.Add(this.FormulaLabel);
            this.TabIntegration.Controls.Add(this.OrdinatesLabel);
            this.TabIntegration.Controls.Add(this.FormulaTextBox);
            this.TabIntegration.Controls.Add(this.OrdinateListView);
            this.TabIntegration.Controls.Add(this.SimpsonsRuleRadioButton);
            this.TabIntegration.Controls.Add(this.TrapeziumRuleRadioButton);
            this.TabIntegration.Controls.Add(this.MidOrdRuleRadioButton);
            this.TabIntegration.Controls.Add(this.StripsLabel);
            this.TabIntegration.Controls.Add(this.UpperLabel);
            this.TabIntegration.Controls.Add(this.LowerLabel);
            this.TabIntegration.Controls.Add(this.LowerLimitTextBox);
            this.TabIntegration.Controls.Add(this.UpperLimitTextBox);
            this.TabIntegration.Controls.Add(this.IterationsTextBox);
            this.TabIntegration.Controls.Add(this.IntegrationResultLabel);
            this.TabIntegration.Location = new System.Drawing.Point(4, 24);
            this.TabIntegration.Name = "TabIntegration";
            this.TabIntegration.Padding = new System.Windows.Forms.Padding(3);
            this.TabIntegration.Size = new System.Drawing.Size(883, 591);
            this.TabIntegration.TabIndex = 0;
            this.TabIntegration.Text = "Integration";
            this.TabIntegration.UseVisualStyleBackColor = true;
            // 
            // FormulaLabel
            // 
            this.FormulaLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FormulaLabel.AutoSize = true;
            this.FormulaLabel.Location = new System.Drawing.Point(6, 568);
            this.FormulaLabel.Name = "FormulaLabel";
            this.FormulaLabel.Size = new System.Drawing.Size(59, 15);
            this.FormulaLabel.TabIndex = 29;
            this.FormulaLabel.Text = "Formula -";
            // 
            // OrdinatesLabel
            // 
            this.OrdinatesLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OrdinatesLabel.AutoSize = true;
            this.OrdinatesLabel.Location = new System.Drawing.Point(468, 501);
            this.OrdinatesLabel.Name = "OrdinatesLabel";
            this.OrdinatesLabel.Size = new System.Drawing.Size(69, 15);
            this.OrdinatesLabel.TabIndex = 28;
            this.OrdinatesLabel.Text = "Ordinates - ";
            // 
            // FormulaTextBox
            // 
            this.FormulaTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FormulaTextBox.Location = new System.Drawing.Point(70, 565);
            this.FormulaTextBox.Name = "FormulaTextBox";
            this.FormulaTextBox.Size = new System.Drawing.Size(793, 23);
            this.FormulaTextBox.TabIndex = 27;
            // 
            // OrdinateListView
            // 
            this.OrdinateListView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OrdinateListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.XHeader,
            this.YHeader});
            this.OrdinateListView.Location = new System.Drawing.Point(542, 472);
            this.OrdinateListView.Name = "OrdinateListView";
            this.OrdinateListView.Size = new System.Drawing.Size(286, 87);
            this.OrdinateListView.TabIndex = 26;
            this.OrdinateListView.UseCompatibleStateImageBehavior = false;
            this.OrdinateListView.View = System.Windows.Forms.View.Details;
            // 
            // XHeader
            // 
            this.XHeader.Text = "X";
            this.XHeader.Width = 100;
            // 
            // YHeader
            // 
            this.YHeader.Text = "Y";
            this.YHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.YHeader.Width = 100;
            // 
            // SimpsonsRuleRadioButton
            // 
            this.SimpsonsRuleRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SimpsonsRuleRadioButton.AutoSize = true;
            this.SimpsonsRuleRadioButton.Location = new System.Drawing.Point(330, 537);
            this.SimpsonsRuleRadioButton.Name = "SimpsonsRuleRadioButton";
            this.SimpsonsRuleRadioButton.Size = new System.Drawing.Size(105, 19);
            this.SimpsonsRuleRadioButton.TabIndex = 25;
            this.SimpsonsRuleRadioButton.Text = "Simpson\'s Rule";
            this.SimpsonsRuleRadioButton.UseVisualStyleBackColor = true;
            this.SimpsonsRuleRadioButton.CheckedChanged += new System.EventHandler(this.AreaGrapherChange);
            // 
            // TrapeziumRuleRadioButton
            // 
            this.TrapeziumRuleRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TrapeziumRuleRadioButton.AutoSize = true;
            this.TrapeziumRuleRadioButton.Location = new System.Drawing.Point(330, 518);
            this.TrapeziumRuleRadioButton.Name = "TrapeziumRuleRadioButton";
            this.TrapeziumRuleRadioButton.Size = new System.Drawing.Size(107, 19);
            this.TrapeziumRuleRadioButton.TabIndex = 24;
            this.TrapeziumRuleRadioButton.Text = "Trapezium Rule";
            this.TrapeziumRuleRadioButton.UseVisualStyleBackColor = true;
            this.TrapeziumRuleRadioButton.CheckedChanged += new System.EventHandler(this.AreaGrapherChange);
            // 
            // MidOrdRuleRadioButton
            // 
            this.MidOrdRuleRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.MidOrdRuleRadioButton.AutoSize = true;
            this.MidOrdRuleRadioButton.Checked = true;
            this.MidOrdRuleRadioButton.Location = new System.Drawing.Point(330, 497);
            this.MidOrdRuleRadioButton.Name = "MidOrdRuleRadioButton";
            this.MidOrdRuleRadioButton.Size = new System.Drawing.Size(129, 19);
            this.MidOrdRuleRadioButton.TabIndex = 23;
            this.MidOrdRuleRadioButton.TabStop = true;
            this.MidOrdRuleRadioButton.Text = "Mid - Ordinate Rule";
            this.MidOrdRuleRadioButton.UseVisualStyleBackColor = true;
            this.MidOrdRuleRadioButton.CheckedChanged += new System.EventHandler(this.AreaGrapherChange);
            // 
            // StripsLabel
            // 
            this.StripsLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.StripsLabel.AutoSize = true;
            this.StripsLabel.Location = new System.Drawing.Point(161, 512);
            this.StripsLabel.Name = "StripsLabel";
            this.StripsLabel.Size = new System.Drawing.Size(44, 15);
            this.StripsLabel.TabIndex = 22;
            this.StripsLabel.Text = "Strips -";
            // 
            // UpperLabel
            // 
            this.UpperLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UpperLabel.AutoSize = true;
            this.UpperLabel.Location = new System.Drawing.Point(22, 512);
            this.UpperLabel.Name = "UpperLabel";
            this.UpperLabel.Size = new System.Drawing.Size(47, 15);
            this.UpperLabel.TabIndex = 21;
            this.UpperLabel.Text = "Upper -";
            // 
            // LowerLabel
            // 
            this.LowerLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LowerLabel.AutoSize = true;
            this.LowerLabel.Location = new System.Drawing.Point(22, 537);
            this.LowerLabel.Name = "LowerLabel";
            this.LowerLabel.Size = new System.Drawing.Size(47, 15);
            this.LowerLabel.TabIndex = 20;
            this.LowerLabel.Text = "Lower -";
            // 
            // LowerLimitTextBox
            // 
            this.LowerLimitTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LowerLimitTextBox.Location = new System.Drawing.Point(70, 537);
            this.LowerLimitTextBox.Name = "LowerLimitTextBox";
            this.LowerLimitTextBox.Size = new System.Drawing.Size(74, 23);
            this.LowerLimitTextBox.TabIndex = 15;
            this.LowerLimitTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AreaParamsChange);
            // 
            // UpperLimitTextBox
            // 
            this.UpperLimitTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UpperLimitTextBox.Location = new System.Drawing.Point(70, 511);
            this.UpperLimitTextBox.Name = "UpperLimitTextBox";
            this.UpperLimitTextBox.Size = new System.Drawing.Size(74, 23);
            this.UpperLimitTextBox.TabIndex = 16;
            this.UpperLimitTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AreaParamsChange);
            // 
            // IterationsTextBox
            // 
            this.IterationsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.IterationsTextBox.Location = new System.Drawing.Point(211, 509);
            this.IterationsTextBox.Name = "IterationsTextBox";
            this.IterationsTextBox.Size = new System.Drawing.Size(74, 23);
            this.IterationsTextBox.TabIndex = 18;
            this.IterationsTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AreaParamsChange);
            // 
            // IntegrationResultLabel
            // 
            this.IntegrationResultLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.IntegrationResultLabel.AutoSize = true;
            this.IntegrationResultLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.IntegrationResultLabel.Location = new System.Drawing.Point(160, 534);
            this.IntegrationResultLabel.Name = "IntegrationResultLabel";
            this.IntegrationResultLabel.Size = new System.Drawing.Size(26, 22);
            this.IntegrationResultLabel.TabIndex = 17;
            this.IntegrationResultLabel.Text = "= ";
            // 
            // TabDifferentiation
            // 
            this.TabDifferentiation.Controls.Add(this.NormalGradLabel);
            this.TabDifferentiation.Controls.Add(this.NormalEquationLabel);
            this.TabDifferentiation.Controls.Add(this.TangentLabel);
            this.TabDifferentiation.Controls.Add(this.TargetPointLabel);
            this.TabDifferentiation.Controls.Add(this.GradientLabel);
            this.TabDifferentiation.Controls.Add(this.TargetPointTextBox);
            this.TabDifferentiation.Location = new System.Drawing.Point(4, 24);
            this.TabDifferentiation.Name = "TabDifferentiation";
            this.TabDifferentiation.Padding = new System.Windows.Forms.Padding(3);
            this.TabDifferentiation.Size = new System.Drawing.Size(883, 591);
            this.TabDifferentiation.TabIndex = 1;
            this.TabDifferentiation.Text = "Differentiation";
            this.TabDifferentiation.UseVisualStyleBackColor = true;
            // 
            // NormalGradLabel
            // 
            this.NormalGradLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NormalGradLabel.AutoSize = true;
            this.NormalGradLabel.Font = new System.Drawing.Font("Arial", 12F);
            this.NormalGradLabel.Location = new System.Drawing.Point(248, 553);
            this.NormalGradLabel.Name = "NormalGradLabel";
            this.NormalGradLabel.Size = new System.Drawing.Size(135, 18);
            this.NormalGradLabel.TabIndex = 16;
            this.NormalGradLabel.Text = "Normal Gradient =";
            // 
            // NormalEquationLabel
            // 
            this.NormalEquationLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NormalEquationLabel.AutoSize = true;
            this.NormalEquationLabel.Font = new System.Drawing.Font("Arial", 12F);
            this.NormalEquationLabel.Location = new System.Drawing.Point(498, 553);
            this.NormalEquationLabel.Name = "NormalEquationLabel";
            this.NormalEquationLabel.Size = new System.Drawing.Size(128, 18);
            this.NormalEquationLabel.TabIndex = 15;
            this.NormalEquationLabel.Text = "Normal Equation:";
            // 
            // TangentLabel
            // 
            this.TangentLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TangentLabel.AutoSize = true;
            this.TangentLabel.Font = new System.Drawing.Font("Arial", 12F);
            this.TangentLabel.Location = new System.Drawing.Point(498, 528);
            this.TangentLabel.Name = "TangentLabel";
            this.TangentLabel.Size = new System.Drawing.Size(132, 18);
            this.TangentLabel.TabIndex = 14;
            this.TangentLabel.Text = "Tangent Equation:";
            // 
            // TargetPointLabel
            // 
            this.TargetPointLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TargetPointLabel.AutoSize = true;
            this.TargetPointLabel.Location = new System.Drawing.Point(12, 531);
            this.TargetPointLabel.Name = "TargetPointLabel";
            this.TargetPointLabel.Size = new System.Drawing.Size(77, 15);
            this.TargetPointLabel.TabIndex = 13;
            this.TargetPointLabel.Text = "TargetPoint -";
            // 
            // GradientLabel
            // 
            this.GradientLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.GradientLabel.AutoSize = true;
            this.GradientLabel.Font = new System.Drawing.Font("Arial", 12F);
            this.GradientLabel.Location = new System.Drawing.Point(248, 528);
            this.GradientLabel.Name = "GradientLabel";
            this.GradientLabel.Size = new System.Drawing.Size(81, 18);
            this.GradientLabel.TabIndex = 11;
            this.GradientLabel.Text = "Gradient =";
            // 
            // TargetPointTextBox
            // 
            this.TargetPointTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TargetPointTextBox.Location = new System.Drawing.Point(95, 528);
            this.TargetPointTextBox.Name = "TargetPointTextBox";
            this.TargetPointTextBox.Size = new System.Drawing.Size(147, 23);
            this.TargetPointTextBox.TabIndex = 10;
            this.TargetPointTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TargetPointTextBoxKeyDown);
            // 
            // TabExtrema
            // 
            this.TabExtrema.Controls.Add(this.ExtremaListView);
            this.TabExtrema.Controls.Add(this.FindExtremaButton);
            this.TabExtrema.Location = new System.Drawing.Point(4, 24);
            this.TabExtrema.Name = "TabExtrema";
            this.TabExtrema.Padding = new System.Windows.Forms.Padding(3);
            this.TabExtrema.Size = new System.Drawing.Size(883, 591);
            this.TabExtrema.TabIndex = 2;
            this.TabExtrema.Text = "Extrema";
            this.TabExtrema.UseVisualStyleBackColor = true;
            // 
            // ExtremaListView
            // 
            this.ExtremaListView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ExtremaListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.XCoordHeader,
            this.YCoordHeader,
            this.ExtremaTypeHeader});
            this.ExtremaListView.Location = new System.Drawing.Point(147, 475);
            this.ExtremaListView.Name = "ExtremaListView";
            this.ExtremaListView.Size = new System.Drawing.Size(369, 110);
            this.ExtremaListView.TabIndex = 13;
            this.ExtremaListView.UseCompatibleStateImageBehavior = false;
            this.ExtremaListView.View = System.Windows.Forms.View.Details;
            // 
            // XCoordHeader
            // 
            this.XCoordHeader.Text = "X";
            this.XCoordHeader.Width = 100;
            // 
            // YCoordHeader
            // 
            this.YCoordHeader.Text = "Y";
            this.YCoordHeader.Width = 100;
            // 
            // ExtremaTypeHeader
            // 
            this.ExtremaTypeHeader.Text = "Extrema Type";
            this.ExtremaTypeHeader.Width = 160;
            // 
            // FindExtremaButton
            // 
            this.FindExtremaButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FindExtremaButton.Location = new System.Drawing.Point(522, 531);
            this.FindExtremaButton.Name = "FindExtremaButton";
            this.FindExtremaButton.Size = new System.Drawing.Size(125, 23);
            this.FindExtremaButton.TabIndex = 12;
            this.FindExtremaButton.Text = "Find Local Extrema";
            this.FindExtremaButton.UseVisualStyleBackColor = true;
            this.FindExtremaButton.Click += new System.EventHandler(this.FindExtremaButtonClick);
            // 
            // TabIteration
            // 
            this.TabIteration.Controls.Add(this.FirstIterationLabel);
            this.TabIteration.Controls.Add(this.FirstIterationTextBox);
            this.TabIteration.Controls.Add(this.CalculateIterationButton);
            this.TabIteration.Controls.Add(this.ResultsListView);
            this.TabIteration.Controls.Add(this.IterationsLabel);
            this.TabIteration.Controls.Add(this.NumIterationsUpDown);
            this.TabIteration.Location = new System.Drawing.Point(4, 24);
            this.TabIteration.Name = "TabIteration";
            this.TabIteration.Padding = new System.Windows.Forms.Padding(3);
            this.TabIteration.Size = new System.Drawing.Size(883, 591);
            this.TabIteration.TabIndex = 3;
            this.TabIteration.Text = "Iterations";
            this.TabIteration.UseVisualStyleBackColor = true;
            // 
            // FirstIterationLabel
            // 
            this.FirstIterationLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FirstIterationLabel.AutoSize = true;
            this.FirstIterationLabel.Location = new System.Drawing.Point(440, 511);
            this.FirstIterationLabel.Name = "FirstIterationLabel";
            this.FirstIterationLabel.Size = new System.Drawing.Size(26, 15);
            this.FirstIterationLabel.TabIndex = 15;
            this.FirstIterationLabel.Text = "x1 -";
            // 
            // FirstIterationTextBox
            // 
            this.FirstIterationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FirstIterationTextBox.Location = new System.Drawing.Point(470, 508);
            this.FirstIterationTextBox.Name = "FirstIterationTextBox";
            this.FirstIterationTextBox.Size = new System.Drawing.Size(137, 23);
            this.FirstIterationTextBox.TabIndex = 10;
            this.FirstIterationTextBox.Text = "0.5";
            this.FirstIterationTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParamsChangeKeyDown);
            // 
            // CalculateIterationButton
            // 
            this.CalculateIterationButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CalculateIterationButton.Location = new System.Drawing.Point(470, 550);
            this.CalculateIterationButton.Name = "CalculateIterationButton";
            this.CalculateIterationButton.Size = new System.Drawing.Size(137, 23);
            this.CalculateIterationButton.TabIndex = 8;
            this.CalculateIterationButton.Text = "Calculate!";
            this.CalculateIterationButton.UseVisualStyleBackColor = true;
            this.CalculateIterationButton.Click += new System.EventHandler(this.CalculateIterationButtonClick);
            // 
            // ResultsListView
            // 
            this.ResultsListView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IterationHeader,
            this.ResultHeader});
            this.ResultsListView.GridLines = true;
            this.ResultsListView.Location = new System.Drawing.Point(239, 63);
            this.ResultsListView.Name = "ResultsListView";
            this.ResultsListView.Size = new System.Drawing.Size(368, 428);
            this.ResultsListView.TabIndex = 12;
            this.ResultsListView.UseCompatibleStateImageBehavior = false;
            this.ResultsListView.View = System.Windows.Forms.View.Details;
            // 
            // IterationHeader
            // 
            this.IterationHeader.Text = "Iteration";
            this.IterationHeader.Width = 155;
            // 
            // ResultHeader
            // 
            this.ResultHeader.Text = "Result";
            this.ResultHeader.Width = 209;
            // 
            // IterationsLabel
            // 
            this.IterationsLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.IterationsLabel.AutoSize = true;
            this.IterationsLabel.Location = new System.Drawing.Point(229, 511);
            this.IterationsLabel.Name = "IterationsLabel";
            this.IterationsLabel.Size = new System.Drawing.Size(64, 15);
            this.IterationsLabel.TabIndex = 14;
            this.IterationsLabel.Text = "Iterations -";
            // 
            // NumIterationsUpDown
            // 
            this.NumIterationsUpDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NumIterationsUpDown.Location = new System.Drawing.Point(300, 509);
            this.NumIterationsUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumIterationsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumIterationsUpDown.Name = "NumIterationsUpDown";
            this.NumIterationsUpDown.Size = new System.Drawing.Size(128, 23);
            this.NumIterationsUpDown.TabIndex = 11;
            this.NumIterationsUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumIterationsUpDown.ValueChanged += new System.EventHandler(this.NumIterationsUpDownValueChanged);
            // 
            // EquationTextBox
            // 
            this.EquationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EquationTextBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.EquationTextBox.Location = new System.Drawing.Point(95, 653);
            this.EquationTextBox.Name = "EquationTextBox";
            this.EquationTextBox.Size = new System.Drawing.Size(702, 29);
            this.EquationTextBox.TabIndex = 1;
            this.EquationTextBox.Text = "sinx";
            this.EquationTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EquationTextBoxKeyDown);
            // 
            // ToolStrip
            // 
            this.ToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitButton,
            this.CopyButton,
            this.AboutLabel});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(922, 25);
            this.ToolStrip.TabIndex = 3;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // ExitButton
            // 
            this.ExitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(29, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.ToolTipText = "Exit The Program";
            this.ExitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // CopyButton
            // 
            this.CopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CopyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(111, 22);
            this.CopyButton.Text = "Copy To Clipboard";
            this.CopyButton.ToolTipText = "Copy Data to Clipboard";
            this.CopyButton.Click += new System.EventHandler(this.CopyButtonClick);
            // 
            // AboutLabel
            // 
            this.AboutLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.AboutLabel.Name = "AboutLabel";
            this.AboutLabel.Size = new System.Drawing.Size(122, 22);
            this.AboutLabel.Text = "Ryan Harrison © 2012";
            // 
            // EquationLabel
            // 
            this.EquationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EquationLabel.AutoSize = true;
            this.EquationLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EquationLabel.Location = new System.Drawing.Point(37, 656);
            this.EquationLabel.Name = "EquationLabel";
            this.EquationLabel.Size = new System.Drawing.Size(52, 21);
            this.EquationLabel.TabIndex = 30;
            this.EquationLabel.Text = "f(x) =";
            // 
            // StartTimer
            // 
            this.StartTimer.Enabled = true;
            this.StartTimer.Interval = 25;
            this.StartTimer.Tick += new System.EventHandler(this.StartTimerTick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 687);
            this.Controls.Add(this.EquationLabel);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.EquationTextBox);
            this.Controls.Add(this.TabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(938, 725);
            this.Name = "MainForm";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Numerical Methods Helper";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.TabControl.ResumeLayout(false);
            this.TabIntegration.ResumeLayout(false);
            this.TabIntegration.PerformLayout();
            this.TabDifferentiation.ResumeLayout(false);
            this.TabDifferentiation.PerformLayout();
            this.TabExtrema.ResumeLayout(false);
            this.TabIteration.ResumeLayout(false);
            this.TabIteration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumIterationsUpDown)).EndInit();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabIntegration;
        private System.Windows.Forms.TabPage TabDifferentiation;
        private System.Windows.Forms.TabPage TabExtrema;
        private System.Windows.Forms.TabPage TabIteration;
        private System.Windows.Forms.TextBox EquationTextBox;
        private System.Windows.Forms.Label FirstIterationLabel;
        private System.Windows.Forms.TextBox FirstIterationTextBox;
        private System.Windows.Forms.Button CalculateIterationButton;
        private System.Windows.Forms.ListView ResultsListView;
        private System.Windows.Forms.ColumnHeader IterationHeader;
        private System.Windows.Forms.ColumnHeader ResultHeader;
        private System.Windows.Forms.Label IterationsLabel;
        private System.Windows.Forms.NumericUpDown NumIterationsUpDown;
        private System.Windows.Forms.Label TargetPointLabel;
        private System.Windows.Forms.Label GradientLabel;
        private System.Windows.Forms.TextBox TargetPointTextBox;
        private System.Windows.Forms.Button FindExtremaButton;
        private System.Windows.Forms.ListView OrdinateListView;
        private System.Windows.Forms.ColumnHeader XHeader;
        private System.Windows.Forms.ColumnHeader YHeader;
        private System.Windows.Forms.RadioButton SimpsonsRuleRadioButton;
        private System.Windows.Forms.RadioButton TrapeziumRuleRadioButton;
        private System.Windows.Forms.RadioButton MidOrdRuleRadioButton;
        private System.Windows.Forms.Label StripsLabel;
        private System.Windows.Forms.Label UpperLabel;
        private System.Windows.Forms.Label LowerLabel;
        private System.Windows.Forms.TextBox LowerLimitTextBox;
        private System.Windows.Forms.TextBox UpperLimitTextBox;
        private System.Windows.Forms.TextBox IterationsTextBox;
        private System.Windows.Forms.Label IntegrationResultLabel;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton ExitButton;
        private System.Windows.Forms.ToolStripButton CopyButton;
        private System.Windows.Forms.TextBox FormulaTextBox;
        private System.Windows.Forms.Label FormulaLabel;
        private System.Windows.Forms.Label OrdinatesLabel;
        private System.Windows.Forms.Label EquationLabel;
        private System.Windows.Forms.Label NormalGradLabel;
        private System.Windows.Forms.Label NormalEquationLabel;
        private System.Windows.Forms.Label TangentLabel;
        private System.Windows.Forms.ToolStripLabel AboutLabel;
        private System.Windows.Forms.Timer StartTimer;
        private System.Windows.Forms.ListView ExtremaListView;
        private System.Windows.Forms.ColumnHeader XCoordHeader;
        private System.Windows.Forms.ColumnHeader ExtremaTypeHeader;
        private System.Windows.Forms.ColumnHeader YCoordHeader;
    }
}