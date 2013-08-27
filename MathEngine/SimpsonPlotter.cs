using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class represents a grapher that calculates the integral of a function using Simpsons rule
    // This class inherits from AreaPlotter and is a custom component
    public class SimpsonPlotter : AreaPlotter
    {
        // Overidden event handler method paints the graph to the component
        protected override void GrapherPaint(object sender, PaintEventArgs e)
        {
            // Call the base class paint method to draw the grid, axes and function to the component
            base.GrapherPaint(sender, e);

            // Get the current result
            float newRes = Result;

            // Get the current list of ordinates
            List<Ordinate> oldOrd = Ordinates;

            // If the target function is valid
            if (!InvalidFunction)
            {
                try
                {
                    // Draw Simpsons rule onto the graph and get the new result
                    newRes = DrawSimpson(e.Graphics, Lower, Upper, Iterations);
                }
                catch (Exception)
                {
                }

                // Draw the integration limits to the graph (Upper and Lower bounds)
                DrawEndPoints(e.Graphics);
            }

            // Set the Result property to the new result
            Result = newRes;

            // If the ordinates have changed fire the result change event
            if (!IsSameOrdinates(oldOrd, Ordinates))
            {
                // Fire the result change event to allow the background GUI to update the results
                FireResultChange(this, e);
            }
        }

        // Private method determines whether or not the two lists contain the same ordinates
        // oldOrdinates - the first list of ordinates to check equality with
        // newOrdinates - the second list of ordinates to check equality with
        private static bool IsSameOrdinates(List<Ordinate> oldOrdinates, List<Ordinate> newOrdinates)
        {
            // If the two lists contain different amounts of ordinates, return false
            if (oldOrdinates.Count < newOrdinates.Count)
                return false;

            // Loop through each ordinate
            for (int i = 0; i < oldOrdinates.Count; i++)
            {
                // If the X and Y values of the ordinate in each list do not match, return false
                if (!(oldOrdinates[i].X.Equals(newOrdinates[i].X) && oldOrdinates[i].Y.Equals(newOrdinates[i].Y)))
                {
                    return false;
                }
            }

            // Return true if all ordinates match
            return true;
        }

        // Private methods draws Simpsons rule onto the graph
        // Method does not draw Simpsons Rule accuractely, merely fills all of the area beneath the curve
        // As Simpsons Rule is the most accurate, for most circumstances this will look the same
        // g - The Graphics object to allow the drawing of the rule
        // a - The lower bound of integration
        // b - The upper bound of integration
        // n - The number of iterations of the rule to use
        // Method returns the result of the integration
        private float DrawSimpson(Graphics g, float a, float b, int n)
        {
            // Get a delegate for the target function to allow for faster calculations of Y values
            Func<double, double> f = TargetFunction.GetDelegate();

            // Get the result of the integration using Simpsons Rule
            float result = SimpsonsRule(a, b, n);

            // Create Color objects for use when drawing
            Color red = Color.FromArgb(150, Color.Red);
            Color blue = Color.FromArgb(150, 0, 127, 255);

            // Get the minimum bounds for use when calculating the starting and stopping values
            double min = Math.Min(a, b);
            double max = Math.Max(a, b);

            // Calculate the the start and stop positions
            double start = Math.Max(0, Width / 2 + (min - OriginX) * Scale);
            double stop = Math.Min(Width, Width / 2 + (max - OriginX) * Scale);

            // While there is still more painting to do before the bounds have been reached
            while (start <= stop)
            {
                double y0 = 0;

                // Create a path object to allow the area to be drawn and filled
                var path = new GraphicsPath();

                // Calculate the current position
                var currentPosition = new PointF((float) start, (Height / 2 + OriginY * Scale));

                // Fill the positive section of the graph while the y values are positive and defined
                while (start <= stop && y0 >= 0 && !Double.IsNaN(y0))
                {
                    // Add a new line from the current position
                    path.AddLine(currentPosition.X, currentPosition.Y, (float) start,
                                 (float) (Height / 2 - (y0 - OriginY) * Scale));

                    // Increment the start variable to move to the next position
                    start++;
                    
                    // Modify the currentPosition to move to the next target point
                    currentPosition.X = (float) start;
                    currentPosition.Y = (float) (Height / 2 - (y0 - OriginY) * Scale);

                    // Determine the next y value
                    double vartemp2 = OriginX + (start - Width / 2) / Scale;
                    y0 = f(vartemp2);
                }

                // Add the final line to the path and draw the path to the graph
                path.AddLine(currentPosition.X, currentPosition.Y, (float) start, (Height / 2 + OriginY * Scale));
                g.FillPath(new SolidBrush(a > b ? blue : red), path);


                // Reset the path for the negative portion of the area
                path = new GraphicsPath();

                // Fill the negative section of the graph
                while (start <= stop && y0 < 0 && !Double.IsNaN(y0))
                {
                    // Add a new line from the current position
                    path.AddLine(currentPosition.X, currentPosition.Y, (float)start,
                                 (float)(Height / 2 - (y0 - OriginY) * Scale));

                    // Increment the start variable to move to the next position
                    start++;

                    // Modify the currentPosition to move to the next target point
                    currentPosition.X = (float)start;
                    currentPosition.Y = (float)(Height / 2 - (y0 - OriginY) * Scale);

                    // Determine the next y value
                    double vartemp2 = OriginX + (start - Width / 2) / Scale;
                    y0 = f(vartemp2);
                }

                // Add the final line to the path and draw the path to the graph
                path.AddLine(currentPosition.X, currentPosition.Y, (float) start, (Height / 2 + OriginY * Scale));

                // Fill the path onto the graph
                g.FillPath(new SolidBrush(a > b ? red : blue), path);
            }

            // Return the result of the integration using Simpsons Rule
            return result;
        }

        // Private methods calculates the result of the integration using Simpsons rule
        // g - The Graphics object to allow the drawing of the rule
        // a - The lower bound of integration
        // b - The upper bound of integration
        // n - The number of iterations of the rule to use
        // Method returns the result of the integration
        private float SimpsonsRule(float a, float b, int n)
        {
            // Calculate variables used in Simpsons rule
            float min = Math.Min(a, b);
            float delta = Math.Abs(b - a) / n;

            // Calculate the Y coordinate of the first ordinate
            float y0 = TargetFunction.EvaluateF(min.ToString(CultureInfo.InvariantCulture));

            // Add the first ordinate to the list
            Ordinates = new List<Ordinate> {new Ordinate(min, y0)};

            // For each iteration of Simpsons rule
            for (int i = 0; i < n; i++)
            {
                // Calculate the x coordinate of the new ordinate
                float x = min + (i + 1) * delta;

                // Calculate the y coordinate of the new ordinate
                float y1 = TargetFunction.EvaluateF(x.ToString(CultureInfo.InvariantCulture));

                // Add the new ordinate to the list
                Ordinates.Add(new Ordinate(x, y1));
            }

            // Sum of odd and even ordinates
            float odd = 0F;
            float even = 0F;

            // Loop through each ordinate in the list
            for (int j = 1; j < Ordinates.Count - 1; j++)
            {
                // If it is an even ordinate, add to the even sum
                if (j % 2 == 0)
                    even += Ordinates[j].Y;
                    // Else if it is an odd ordinate, add to the odd sum
                else
                    odd += Ordinates[j].Y;
            }

            // Calculate the sum by adding the first and last ordinates together, then add four times the sum of every odd ordinates
            // and finally add two times the sum of every even ordinate
            float sum = Ordinates[0].Y + Ordinates[Ordinates.Count - 1].Y + (4 * odd) + (2 * even);

            // Obtain the final result by multiplying the sum by one third times delta
            float result = 0.333333F * delta * sum;

            return result;
        }

        // Overriden method returns a the formula string for the mid ordinate rule with 
        // the passed in list of ordinates
        public override string GetFormulaString(List<Ordinate> ordinates)
        {
            // Multiply everything by (1/3) * delta
            string e = "(1/3) * " + GetDelta() + " * (";

            // Add the first and last ordinate in the list to the formula
            e += GetOrdinateForFormula(ordinates[0]) + GetOrdinateForFormula(ordinates[ordinates.Count - 1]);

            //Multiply every remaining odd ordinate by 4
            e += " + 4 * (";

            for (int i = 1; i < ordinates.Count - 1; i += 2)
            {
                e += GetOrdinateForFormula(ordinates[i]);
            }
            // Multiply every remaining even ordinate by 2
            e += ") + 2 * (";

            for (int i = 2; i < ordinates.Count - 1; i += 2)
            {
                e += GetOrdinateForFormula(ordinates[i]);
            }

            e += "))";

            // Return the final formula
            return e;
        }
    }
}