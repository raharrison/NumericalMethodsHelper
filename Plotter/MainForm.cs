using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using MathEngine;

namespace Plotter
{
    // Class provides the main form in which all graphs and results will be displayed
    // This file provides event handlers for components within the form
    public partial class MainForm : Form
    {
        // An AreaPlotter instance for the integration tab. By using polymorphism, this field can be any subclass of the AreaPlotter class
        private AreaPlotter areaPlotter;

        // And instance of the DifferentiationPlotter class for the differentiation tab
        private DifferentiationPlotter differentiationPlotter;

        // And instance of the ExtremaPlotter class for the extrema tab
        private ExtremaPlotter extremaPlotter;

        // Constructor initialises the components on the form
        public MainForm()
        {
            InitializeComponent();
        }

        // Method is called whenever a graph control is resized
        // Method resizes the graph control to fill the form, yet leave an area below to house the parameters
        private void PlotterResize(object sender, EventArgs e)
        {
            // Cast the sender to a Plotter object
            var grapher = (MathEngine.Plotter) sender;

            // If the current height is greater than 160, set the graphers height property to 260 less than the height of the form  
            if (Height > 160)
            {
                grapher.Height = Height - 260;
            }
            else
            {
                // Else set the height of the grapher to zero
                grapher.Height = 0;
            }
        }

        // Iterations Tab

        // Method is called when the Calculate button on the Iteration tab is clicked
        // Method simply calls for the results to be added to the listview
        private void CalculateIterationButtonClick(object sender, EventArgs e)
        {
            // Calculate the new iteration results and populate the listview
            CalculateResults();
        }

        // Method calculates an iterational equation up to a certain number
        // At first a user specified number is inserted into the equation and a result is calculated
        // For every subsequent iteration, the most recent calculated value is inserted to give a new result
        // Method populates the listview control with the results
        private void CalculateResults()
        {
            // Create a new Function object with the equation entered in the equation textbox to allow the equation
            // to be evaulated
            var function = new Function(EquationTextBox.Text);
			var eval = new Evaluator();

            // Clear the listview component to remove previous results
            ResultsListView.Items.Clear();

            // A variable to hold the current calculated value
            double result;

            // The number of iterations to use
            int numIterations = (int) NumIterationsUpDown.Value - 1;

            // If the number entered in the first iteration textbox is numerical
			
			try
			{
				// Evaluate the first iteration
				result = eval.Evaluate(FirstIterationTextBox.Text);
			}
			catch(Exception)
			{
				// Prompt the user if there was an error evaluating
				MessageBox.Show("Invalid first iteration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			//Add the first item to the listview. The first iteration is just the first value entered by the user
			var item1 = new ListViewItem("1");
			item1.SubItems.Add(result.ToString(CultureInfo.InvariantCulture));

			// Add the first result to the listview
			ResultsListView.Items.Add(item1);

			try
			{
				// Loop through each iteration
				for (int i = 0; i < numIterations; i++)
				{
					// Calculate the new result by evaluating the function at the current result
					double newRes = Math.Round(function.Evaluate(result.ToString(CultureInfo.InvariantCulture)), 5);

					// Add the new result to the listview control
					var item = new ListViewItem((i + 2).ToString(CultureInfo.InvariantCulture));
					item.SubItems.Add(newRes.ToString(CultureInfo.InvariantCulture));
					ResultsListView.Items.Add(item);

					// Set the current result to the newly calculated result to get ready for the next iteration
					result = newRes;
				}
			}
			catch (Exception)
			{
				// Prompt user if there were errors evaluating the function
				MessageBox.Show("Invalid Equation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        // Method is called when the user presses a key in the first iteration textbox
        // Method calculates new results if the enter key is pressed
        private void ParamsChangeKeyDown(object sender, KeyEventArgs e)
        {
            // If the enter key has been pressed
            if (e.KeyCode == Keys.Enter)
                // Calculate the new results
                CalculateResults();
        }

        // Method is called when the value in the numerical up/ down control representing the number of iterations is changed
        // Method calculates the new results when the value is changed
        private void NumIterationsUpDownValueChanged(object sender, EventArgs e)
        {
            // Calculate the new results
            CalculateResults();
        }

        // Differentiation Tab

        // Method is called whenever the result in the differentiation grapher changes
        // Method updates control on the form to show the new results
        private void DifferentiationPlotterResultChange(object sender, PaintEventArgs e)
        {
            // Set corresponding controls on the form to show the new results
            GradientLabel.Text = string.Format("Gradient = {0}",
                                               Math.Round(differentiationPlotter.Gradient, 3).ToString(CultureInfo.InvariantCulture));
            TargetPointTextBox.Text = differentiationPlotter.TargetPoint.ToString(CultureInfo.InvariantCulture);

            NormalGradLabel.Text = "Normal Gradient = " +
                                   Math.Round(differentiationPlotter.NormalGrad, 3).ToString(CultureInfo.InvariantCulture);
            TangentLabel.Text = "Tangent Equation: " + differentiationPlotter.Tangent;
            NormalEquationLabel.Text = "Normal Equation: " + differentiationPlotter.Normal;

            EquationTextBox.Text = differentiationPlotter.Equation;
        }

        // Method is called when the user hits a key when in the target point textbox on the differentiation tab
        // If the user presses the enter key, update the TargetPoint control in the DifferentiationPlotter object instance
        private void TargetPointTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            // If the enter key has been pressed
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    // Create an Evaluator object to evaluate the text in the target point textbox
                    // This allows the user to enter input such as 'pi' into the textbox
                    var eval = new Evaluator();
					
					// Evaluate the expression and get a result
					var result = (float) eval.Evaluate(TargetPointTextBox.Text);
					
					if((result >= -9999F) && (result <= 9999F))
					{
						// If the result is valid, assign the TargetPoint of the grapher to the result
						differentiationPlotter.TargetPoint = (float) result;
					}
					else
					{
						// Prompt the user if the value is not valid
						MessageBox.Show("Target Point must be between -9999 and 9999", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
                }
                catch (Exception)
                {
                    // Prompt the user if the text entered could not be evaluated into a numerical result
                    MessageBox.Show("Invalid Target Point", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Extrema Tab

        // Method is called when the FindExtrema button in the extrema tab is pressed
        // Method calls for the ExtremaPlotter to begin looking for extrema
        // When finished the method populates a listview with the results
        private void FindExtremaButtonClick(object sender, EventArgs e)
        {
            // Clear the results listview to remove any previous results
            ExtremaListView.Items.Clear();
			
			this.Cursor = Cursors.WaitCursor;

            // Get the list of extrema found from the ExtremaPlotter instance
            List<Extrema> extremaList = extremaPlotter.FindExtrema();

            // If there have been no errors when calculating the extrema
            if (extremaList != null)
            {
                // If no extrema have been found
                if (extremaList.Count == 0)
                {
                    // Add one item to the listview to prompt the user that no results have been found
                    var item = new ListViewItem("NaN");
                    item.SubItems.Add("NaN");
                    item.SubItems.Add("No Local Extrema Found");
                    ExtremaListView.Items.Add(item);
                }
                else
                {
                    // Loop through each Extrema object in the results list
                    foreach (Extrema ex in extremaList)
                    {
                        // Add a new item to the listview with the results found in the current Extrema object
                        var item = new ListViewItem(Math.Round(ex.Target, 4).ToString(CultureInfo.InvariantCulture));
                        item.SubItems.Add(Math.Round(ex.YCoordinate, 4).ToString(CultureInfo.InvariantCulture));
                        item.SubItems.Add(ex.Type);

                        // Add the new item to the listview
                        ExtremaListView.Items.Add(item);
                    }
                }
            }
			this.Cursor = Cursors.Default;
        }

        // Integration Tab

        // Method is called when the results of the AreaPlotter change
        // Method update controls on the form to show the user the new results
        private void AreaPlotterResultChange(object sender, PaintEventArgs e)
        {
            // Set text properties to show the new results
            IntegrationResultLabel.Text = "Result = " +
                                          Math.Round(areaPlotter.Result, 4).ToString(CultureInfo.InvariantCulture);
            LowerLimitTextBox.Text = areaPlotter.Lower.ToString(CultureInfo.InvariantCulture);
            UpperLimitTextBox.Text = areaPlotter.Upper.ToString(CultureInfo.InvariantCulture);

            // Clear the current ordinates listview to remove previous results
            OrdinateListView.Items.Clear();

            // Loop through each Ordinate object in the Ordinate list of the AreaPlotter
            foreach (Ordinate ord in areaPlotter.Ordinates)
            {
                // Add a new item to the listview with the X and Y coordinates found in the current Ordinate object
                var item = new ListViewItem(Math.Round(ord.X, 4).ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(Math.Round(ord.Y, 4).ToString(CultureInfo.InvariantCulture));

                // Add the new item to the OrdinateListView to show the results
                OrdinateListView.Items.Add(item);
            }

            // Update the formula textbox with the new formula string generated from the AreaPlotter object
            FormulaTextBox.Text = areaPlotter.GetFormulaString(areaPlotter.Ordinates);
        }

        // Method is called when the user presses a key in any of the integration parameter textboxes
        // If the enter key has been pressed, update the properties in the AreaPlotter instance with the new parameters
        private void AreaParamsChange(object sender, KeyEventArgs e)
        {
            // If the enter key ahs been pressed
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    // Create an Evaluator object to evaluate the text in each parameter textbox
                    // This allows the user to enter input such as 'pi' into a textbox
                    var eval = new Evaluator();

                    // Update the properties in the AreaPlotter with the evaluated results
                    areaPlotter.Lower = (float) eval.Evaluate(LowerLimitTextBox.Text);
                    areaPlotter.Upper = (float) eval.Evaluate(UpperLimitTextBox.Text);
                    var t = (int) eval.Evaluate(IterationsTextBox.Text);

                    // Bounds check the number of iterations to prevent performance issues
                    if (t > 99)
                        throw new Exception("Iterations must be less than 99");
						
					if(t <= 0)
						throw new Exception("Number of iterations must be greater than zero");

                    // If the current graph is a SimpsonPlotter, then the number of iterations cannot be odd
                    if (t % 2 != 0 && areaPlotter is SimpsonPlotter)
                        throw new Exception("Must have even number of strips");

                    areaPlotter.Iterations = (int) eval.Evaluate(IterationsTextBox.Text);

                    e.Handled = true;
                }
                catch (Exception exc)
                {
                    // Prompt the user if any parameter inserted could not be evaluated into a numerical result
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method is called whenever the user presses a radio button to change the method of integration
        // Method removes the current grapher from the tab and adds a corresponding AreaPlotter depending on which
        // rule of integration has been chosen
        // This method demonstrates the dynamic creation of user controls
        private void AreaGrapherChange(object sender, EventArgs e)
        {
            // Cast the sender object to a RadioButton object to determine which radiobutton was pressed
            var button = sender as RadioButton;

            // If the sender was not a RadioButton instance, exit the method
            if (button == null) return;

            // Get the properties of the current AreaPlotter instance and store in variables
            float x = areaPlotter.OriginX;
            float y = areaPlotter.OriginY;
            int zoom = areaPlotter.Zoom;

            float low = areaPlotter.Lower;
            float upper = areaPlotter.Upper;
            int iter = areaPlotter.Iterations;

            // Remove the current AreaPlotter instance from the TabControl
            TabIntegration.Controls.Remove(areaPlotter);

            // Dispose of the AreaPlotter instance to free resources
            areaPlotter.Dispose();

            // If the Mid Ordinate rule RadioButton was pressed
            if (button.Name.Equals("MidOrdRuleRadioButton"))
            {
                // Set the AreaPlotter instance to become a new Mid Ordinate Plotter
                areaPlotter = new MidOrdPlotter();
            }
                // If the Trapezium rule RadioButton was pressed
            else if (button.Name.Equals("TrapeziumRuleRadioButton"))
            {
                // Set the AreaPlotter instance to become a new Trapezium Plotter
                areaPlotter = new TrapeziumPlotter();
            }
                // If the Simpsons rule RadioButton was pressed
            else
            {
                // Set the AreaPlotter instance to become a new Simpsons Plotter
                areaPlotter = new SimpsonPlotter();
            }

            // Add event handlers to the new AreaPlotter
            areaPlotter.ResultChange += AreaPlotterResultChange;
            areaPlotter.Resize += PlotterResize;
            areaPlotter.PositionMove += PlotterPositionMove;

            // Set the default size of the AreaPlotter
            areaPlotter.Size = new Size(605, 500);

            // Add the new AreaPlotter to the TabControl
            TabIntegration.Controls.Add(areaPlotter);

            // Assign the properties of the new AreaPlotter instance to that of the old AreaPlotter
            areaPlotter.OriginX = x;
            areaPlotter.OriginY = y;
            areaPlotter.Zoom = zoom;

            areaPlotter.Lower = low;
            areaPlotter.Upper = upper;
            areaPlotter.Iterations = iter;
            areaPlotter.Equation = EquationTextBox.Text;
            areaPlotter.Dock = DockStyle.Fill;

            // Repaint the AreaPlotter to show changes
            areaPlotter.Invalidate();

            // Update the parameters and result label
            AreaParamsChange(null, new KeyEventArgs(Keys.Return));
            IntegrationResultLabel.Text = Math.Round(areaPlotter.Result, 4).ToString(CultureInfo.InvariantCulture);
        }

        // Method is called when the current tab selected is changed
        // Depending on which tab is selected, method removes and adds the corresponding grapher instance
        // This method demonstrates the dynamic creation of user controls
        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            // If the Integration tab is selected
            if (TabControl.SelectedIndex == 0)
            {
                // Remove the current AreaPlotter instance from the TabControl
                TabIntegration.Controls.Remove(areaPlotter);

                // Create a new default Mid Ordinate Plotter instance with default properties
                areaPlotter = new MidOrdPlotter();
                areaPlotter.ResultChange += AreaPlotterResultChange;
                areaPlotter.Size = new Size(605, 500);
                areaPlotter.Lower = 0;
                areaPlotter.Upper = 10;

                areaPlotter.Equation = EquationTextBox.Text;
                areaPlotter.Dock = DockStyle.Fill;
                areaPlotter.Resize += PlotterResize;
                areaPlotter.PositionMove += PlotterPositionMove;

                // Update the parameter controls on the form to show changes
                UpperLimitTextBox.Text = areaPlotter.Upper.ToString(CultureInfo.InvariantCulture);
                LowerLimitTextBox.Text = areaPlotter.Lower.ToString(CultureInfo.InvariantCulture);
                IterationsTextBox.Text = areaPlotter.Iterations.ToString(CultureInfo.InvariantCulture);

                // Add the new AreaPlotter to the TabControl
                TabIntegration.Controls.Add(areaPlotter);

                // Focus the new AreaPlotter control
                areaPlotter.Focus();
            }
                // If the Differentiation tab is selected
            else if (TabControl.SelectedIndex == 1)
            {
                // Remove the current DifferentiationPlotter instance from the TabControl
                TabDifferentiation.Controls.Remove(differentiationPlotter);

                // Create a new default DifferentiationPlotter instance with default properties
                differentiationPlotter = new DifferentiationPlotter();
                differentiationPlotter.ResultChange += DifferentiationPlotterResultChange;
                differentiationPlotter.Size = new Size(605, 527);

                differentiationPlotter.Equation = EquationTextBox.Text;
                differentiationPlotter.Dock = DockStyle.Fill;
                differentiationPlotter.Resize += PlotterResize;
                differentiationPlotter.PositionMove += PlotterPositionMove;

                // Add the new DifferentiationPlotter to the TabControl
                TabDifferentiation.Controls.Add(differentiationPlotter);

                // Focus the new DifferentiationPlotter control
                differentiationPlotter.Focus();
            }
                // If the Extrema tab is selected
            else if (TabControl.SelectedIndex == 2)
            {
                // Remove the current ExtremaPlotter instance from the TabControl
                TabExtrema.Controls.Remove(extremaPlotter);

                // Create a new default ExtremaPlotter instance with default properties
                extremaPlotter = new ExtremaPlotter();

                extremaPlotter.Dock = DockStyle.Fill;
                extremaPlotter.Size = new Size(605, 527);
                extremaPlotter.Equation = EquationTextBox.Text;

                extremaPlotter.Resize += PlotterResize;
                extremaPlotter.PositionMove += PlotterPositionMove;

                // Add the new ExtremaPlotter to the TabControl
                TabExtrema.Controls.Add(extremaPlotter);

                // Focus the new ExtremaPlotter control
                extremaPlotter.Focus();
            }
                // If the Iterations tab is selected
            else if (TabControl.SelectedIndex == 3)
            {
                // Calculate a new results set and populate the listview control
                CalculateResults();
            }
        }

        // Method is called when the current position of the mouse in the current graph view changes
        // Method updates the title bar to tell the user the coordinates of the mouse position
        private void PlotterPositionMove(object sender, MouseEventArgs e)
        {
            // Cast the sender to a Plotter object
            var grapher = (MathEngine.Plotter) sender;

            // Get the cartesian coordinates of the current mouse position
            PointF point = grapher.ToCartesianPoint(e.X, e.Y);

            // Update the title bar with the new coordinates
            Text = string.Format("Numerical Methods Helper ({0}, {1})", Math.Round(point.X, 4), Math.Round(point.Y, 4));
        }

        // Method is called when the form loads
        // Method calls the tabchanged method to create a grapher or update results depending on which tab is selected
        // without the user having to press a button
        private void MainFormLoad(object sender, EventArgs e)
        {
            // Create a grapher object and add it to the form depending on what tab is currently selected
            // Alternatively calculate new results if the Iterations tab is selected
            TabControlSelectedIndexChanged(null, null);
        }

        // Method is called when the user presses a key in the EquationTextbox control
        private void EquationTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            // If the user presses the enter key
            if (e.KeyCode == Keys.Enter)
            {
                // If the integration tab is currently selected
                if (TabControl.SelectedIndex == 0)
                {
                    // Update the Equation property in the AreaPlotter instance
                    areaPlotter.Equation = EquationTextBox.Text;
                }
                    // If the differentiation tab is currently selected
                else if (TabControl.SelectedIndex == 1)
                {
                    // Update the Equation property in the DifferentiationPlotter instance
                    differentiationPlotter.Equation = EquationTextBox.Text;
                }
                    // If the extrema tab is currently selected
                else if (TabControl.SelectedIndex == 2)
                {
                    // Update the Equation property in the ExtremaPlotter instance
                    extremaPlotter.Equation = EquationTextBox.Text;
                }
                    // If the iterations tab is currently selected
                else if (TabControl.SelectedIndex == 3)
                {
                    // Calculate a new results set with the new equation
                    CalculateResults();
                }
            }
        }

        // Method is called when the Exit button on the ToolStrip is pressed
        // Method simply exits the application
        private void ExitButtonClick(object sender, EventArgs e)
        {
            // Exit the application
            Application.Exit();
        }

        // Method is called when the Copy To Clipboard button on the ToolStrip is pressed
        private void CopyButtonClick(object sender, EventArgs e)
        {
            // If the integration tab is currently selected
            if (TabControl.SelectedIndex == 0)
            {
                // Tell the AreaPlotter object to copy the current graph view to the clipboard
                areaPlotter.CopyToClipboard();
            }
                // If the differentiation tab is currently selected
            else if (TabControl.SelectedIndex == 1)
            {
                // Tell the DifferentiationPlotter object to copy the current graph view to the clipboard
                differentiationPlotter.CopyToClipboard();
            }
                // If the extrema tab is currently selected
            else if (TabControl.SelectedIndex == 2)
            {
                // Tell the ExtremaPlotter object to copy the current graph view to the clipboard
                extremaPlotter.CopyToClipboard();
            }
                // If the iterations tab is currently selected
            else if (TabControl.SelectedIndex == 3)
            {
                // Add iterations parameters to a string
                string result = "f(x) = " + EquationTextBox.Text;
                result += "\nx1 = " + FirstIterationTextBox.Text;
                result += "\nIterations = " + NumIterationsUpDown.Value + "\n";

                // Loop through every iteration results currently in the results listview
                foreach (ListViewItem item in ResultsListView.Items)
                {
                    // Add the result to the string on a new line
                    result += item.Text + "    " + item.SubItems[1].Text.ToString(CultureInfo.InvariantCulture) + "\n";
                }

                // Trim the string of any trailing whitespace
                result = result.Trim();

                // Add the string to the clipboard to allow for pasting in other applications
                Clipboard.SetText(result);
            }
			
			MessageBox.Show("Successfully Copied to Clipboard", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		
        }

        // Method is called each time the StartTimer timer ticks
        private void StartTimerTick(object sender, EventArgs e)
        {
            // Increase the Opacity of the form to make the form more visible
            Opacity += 0.05;

            // If the form is at 100% opacity (or fully visible)
            if (Math.Abs(Opacity - 100) < 0.05)
            {
                // Stop the timer
                StartTimer.Enabled = false;

                // Dispose of the timer to free resources
                StartTimer.Dispose();
            }
        }
    }
}