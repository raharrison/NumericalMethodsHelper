using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class represents a grapher that calculates the integral of a function using the mid ordinate rule
    // This class inherits from AreaPlotter and is a custom component
    public class MidOrdPlotter : AreaPlotter
    {
        // Overidden event handler method paints the graph to the component
        protected override void GrapherPaint(object sender, PaintEventArgs e)
        {
            // Call the base class paint method to draw the grid, axes and function to the component
            base.GrapherPaint(sender, e);

            // Get the current result
            float newRes = Result;

            // If the target function is valid
            if (!InvalidFunction)
            {
                try
                {
                    // Draw the mid ordinate rule onto the graph and get the new result
                    newRes = DrawMidOrd(e.Graphics, Lower, Upper, Iterations);
                }
                catch (Exception)
                {
                }

                // Draw the integration limits to the graph (Upper and Lower bounds)
                DrawEndPoints(e.Graphics);
            }

            // If the result has changed
            if (Math.Abs(newRes - Result) > 0.0000001)
            {
                // Set the Result field to the new result
                Result = newRes;

                // Fire the result change event to allow the background GUI to update the results
                FireResultChange(this, e);
            }
        }

        // Private methods draws the mid ordinate rule onto the graph
        // g - The Graphics object to allow the drawing of the rule
        // a - The lower bound of integration
        // b - The upper bound of integration
        // n - The number of iterations of the rule to use
        // Method returns the result of the integration using the mid ordinate rule
        private float DrawMidOrd(Graphics g, float a, float b, int n)
        {
            // Calculate variables needed for the rule
            float min = Math.Min(a, b);
            float delta = Math.Abs(b - a) / n;
            float result = 0F;

            // Create the new list of ordinates
            Ordinates = new List<Ordinate>();

            // For each iteration of the rule
            for (int i = 0; i < n; i++)
            {
                // Calculate the x coordinate of the current ordinate
                float x = min + (i + 0.5F) * delta;

                // Calulcate the y coordinate of the current ordinate
                float y = TargetFunction.EvaluateF(x.ToString(CultureInfo.InvariantCulture));

                // Add a new Ordinate object to the list with the current coordinates of the ordinate
                Ordinates.Add(new Ordinate(x, y));

                // Increment the result variable
                result += y;

                // Draw the rectangle representing the current ordinate to the graph
                DrawRectangle(g, a, b, min + i * delta, delta, y);
            }

            // Multiply the result by delta to obtain the final result
            result *= delta;

            // Error check to see if the upper limit is less then the lower limit
            if (b < a) result *= -1;

            // Return the result
            return result;
        }

        // Private method draws a Rectangle onto the graph
        // g - The Graphics object to allow the drawing of the Rectangle
        // a - The lower limit of integration to determine the colour of the rectangle
        // b - The upper limit of integration to determine the colour of the rectangle
        // x - The upper left x coordinate
        // width - the width of the rectangle
        // height - the height of the rectangle
        private void DrawRectangle(Graphics g, float a, float b, float x, float width, float height)
        {
            //If the height is valid then draw the rectangle
            if (!(Single.IsNaN(height) || Single.IsInfinity(height)))
            {
                var brush = new SolidBrush(Color.Red);

                // If the rectangle enters the negative portion of the graph, set the colour to blue
                if ((b - a) * height < 0)
                    brush.Color =
                        Color.FromArgb(
                            150, // transparency
                            0, // red.
                            127, // green.
                            255); // blue.

                else
                    // Set the colour to red for a positive ordinate
                    brush.Color =
                        Color.FromArgb(
                            150, // transparency
                            255, // red.
                            0, // green.
                            0); // blue.


                // Calculate the upper left coordinate of the rectangle
                PointF upperLeft = ToScreenPoint(x, (height > 0 ? height : 0));

                // Fill the rectangle on the graph
                g.FillRectangle(brush, upperLeft.X, upperLeft.Y, Math.Abs(width * Scale),
                                Math.Abs(height * Scale));

                var pen = new Pen(Color.Black, 2) {Alignment = PenAlignment.Inset};

                // Draw a border to the rectangle
                g.DrawRectangle(pen, upperLeft.X, upperLeft.Y, Math.Abs(width * Scale),
                                Math.Abs(height * Scale));
            }
        }

        // Overriden method returns a the formula string for the mid ordinate rule with 
        // the passed in list of ordinates
        public override string GetFormulaString(List<Ordinate> ordinates)
        {
            // Multiply everything by delta
            string e = GetDelta() + " * (";

            // Loop through each ordinate in the list and add its value to the formula string
            foreach (Ordinate o in ordinates)
            {
                e += GetOrdinateForFormula(o);
            }

            e += ")";

            // Return the final formula
            return e;
        }
    }
}