using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class represents a grapher that calculates the integral of a function using the trapezium rule
    // This class inherits from AreaPlotter and is a custom component
    public class TrapeziumPlotter : AreaPlotter
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
                    // Draw the trapezium rule to the graph and get the new result
                    newRes = DrawTrapeziumRule(e.Graphics, Lower, Upper, Iterations);
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

        // Private methods draws the trapezium rule onto the graph
        // g - The Graphics object to allow the drawing of the rule
        // a - The lower bound of integration
        // b - The upper bound of integration
        // n - The number of iterations of the rule to use
        // Method returns the result of the integration using the trapezium rule
        private float DrawTrapeziumRule(Graphics g, float a, float b, int n)
        {
            // Calculate variables used in the trapezium rule
            float min = Math.Min(a, b);
            float delta = Math.Abs(b - a) / n;
            float result = 0F;

            // Get the y value of the minimum bound
            float y0 = TargetFunction.EvaluateF(min.ToString(CultureInfo.InvariantCulture));

            // Add the first ordinate to the new list
            Ordinates = new List<Ordinate> {new Ordinate(min, y0)};

            // Get the points of the ordinate in terms of pixels on the component
            PointF minPoint = ToScreenPoint(min, 0);
            PointF minYPoint = ToScreenPoint(min, y0);

            // Create objects to draw the trapezia
            Color red = Color.FromArgb(150, 255, 0, 0);
            Color blue = Color.FromArgb(150, 0, 127, 255);

            var brush = new SolidBrush(red);
            var pen = new Pen(Color.Black, 2);

            // For each iteration of the rule
            for (int i = 0; i < n; i++)
            {
                // Calculate the X value of the next ordinate
                float x = min + (i + 1) * delta;

                // Calculate the new Y value of the next ordinate
                float y1 = TargetFunction.EvaluateF(x.ToString(CultureInfo.InvariantCulture));

                // Add the new ordinate to the list
                Ordinates.Add(new Ordinate(x, y1));

                // Get the points of the new ordinate in terms of pixels on the component
                PointF ordinatePoint = ToScreenPoint(x, y1);
                PointF xAxisPoint = ToScreenPoint(x, 0);

                // Increment the result variable to add the new ordinate
                result += y0 + y1;

                brush.Color = red;

                // If the y values are defined
                if (!(Single.IsNaN(y0) || Single.IsNaN(y1) || Single.IsInfinity(y0) || Single.IsInfinity(y1)))
                {
                    // Create a new GraphicsPath object to represent the path of the trapezium ordinate on the graph
                    GraphicsPath path;

                    // If the trapezium does not cross a root of the target function
                    if (y0 * y1 >= 0)
                    {
                        // Determine whether the trapezium is below or above the x axis
                        // Set the colour to blue if it is
                        if (a < b && (y0 < 0 || (y0 == 0 && y1 < 0)))
                            brush.Color = blue;
                        else if (a > b && (y0 > 0 || (y0 == 0 && y1 > 0)))
                            brush.Color = blue;

                        // Add the trapezium to the path
                        path = new GraphicsPath();
                        // Add lines to the path using the points of the trapezium to generate the shape
                        path.AddLine(minPoint.X, minPoint.Y, minYPoint.X, minYPoint.Y);
                        path.AddLine(minYPoint.X, minYPoint.Y, ordinatePoint.X, ordinatePoint.Y);
                        path.AddLine(ordinatePoint.X, ordinatePoint.Y, xAxisPoint.X, xAxisPoint.Y);

                        // Fill the trapezium
                        g.FillPath(brush, path);

                        // Draw a border around the trapezium
                        g.DrawPath(pen, path);
                    }
                    else
                    {
                        // Calculate the upper left hand corner of the trapezium
                        PointF t = ToScreenPoint(min + i * delta + y0 * delta / (y0 - y1), 0);

                        // Draw the first half of the trapezium (the piece to the left of the root)

                        // Determine whether the first half of the trapezium is below or above the x axis
                        // Set the colour to blue if it is
                        if ((a < b && y0 < 0) || (a > b && y0 > 0))
                            brush.Color = blue;

                        // Add the trapezium to the path
                        path = new GraphicsPath();

                        // Add lines to the path using the points of the trapezium to generate the shape
                        path.AddLine(minPoint.X, minPoint.Y, minYPoint.X, minYPoint.Y);
                        path.AddLine(minYPoint.X, minYPoint.Y, t.X, t.Y);

                        // Fill the trapezium
                        g.FillPath(brush, path);

                        // Draw a border around the trapezium
                        g.DrawPath(pen, path);

                        brush.Color = red;

                        // Draw the first half of the trapezium (the piece to the right of the root)

                        // Determine whether the second half of the trapezium is below or above the x axis
                        // Set the colour to blue if it is
                        if ((a < b && y0 > 0) || (a > b && y0 < 0))
                            brush.Color = blue;

                        // Add the trapezium to the path
                        path = new GraphicsPath();

                        // Add lines to the path using the points of the trapezium to generate the shape
                        path.AddLine(ordinatePoint.X, ordinatePoint.Y, xAxisPoint.X, xAxisPoint.Y);
                        path.AddLine(xAxisPoint.X, xAxisPoint.Y, t.X, t.Y);

                        // Fill the trapezium
                        g.FillPath(brush, path);

                        // Draw a border around the trapezium
                        g.DrawPath(pen, path);
                    }
                }

                // Change ordinate values to get ready for the next iteration
                y0 = y1;
                minPoint = xAxisPoint;
                minYPoint = ordinatePoint;
            }

            // Multiply the result by delta and divide by two to get the final result
            result *= delta / 2.0F;

            // Error check to see if the upper limit is less then the lower limit
            if (b < a) result *= -1;

            // Return the final result of the integration using the trapezium rule
            return result;
        }

        // Overriden method returns a the formula string for the trapezium rule with 
        // the passed in list of ordinates
        public override string GetFormulaString(List<Ordinate> ordinates)
        {
            // Multiply everything by 0.5 * delta
            string e = "1/2 * " + GetDelta() + " * (";

            //Add the first and last ordinate
            e += GetOrdinateForFormula(ordinates[0]) + " " + GetOrdinateForFormula(ordinates[ordinates.Count - 1]);
            e += " + 2 * (";

            // Add two times every other ordinate
            for (int i = 1; i < ordinates.Count - 1; i++)
            {
                e += GetOrdinateForFormula(ordinates[i]);
            }

            e += "))";

            // Return the final formula
            return e;
        }
    }
}