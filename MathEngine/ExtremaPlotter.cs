using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class defines a Plotter that calculates local extrema to a function
    // between the bounds of the current graph view
    // This class inherits from the Plotter class and can be used as a custom component
    public class ExtremaPlotter : Plotter
    {
        // Determines whether the grapher is currrently finding local extrema
        private bool finding;

        // The current target point to calculate local extrema
        // This value will change with progress of the graphers calculation
        // and will be drawn to represent progress
        private float target;

        // Constructor for the class sets default values to private fields
        public ExtremaPlotter()
        {
            finding = false;
        }

        // Method overrides the base class paint method
        protected override void GrapherPaint(object sender, PaintEventArgs e)
        {
            base.GrapherPaint(sender, e);

            // If the grapher is finding extrema, draw the current point
            if (finding)
                DrawTargetPoint(e.Graphics);
        }

        // Method tells the grapher to find any local extrema within the graph view
        // Method returns a list of Extrema objects, each representing a local minimum or maximum
        // A function has a minimum or maxmimum when the first derivative is zero
        // If the second derivative at a minimum or maximum is negative, then the function is at a
        // maximum. Otherwise the function is at a minimum
        // Currently this method can produce repeated and innaccurate results due to innacuracies in the
        // calculation of the derivatives
        public List<Extrema> FindExtrema()
        {
            // Get the values of bounds of the current graph view
            // These values will become the bounds of the searching radius
            PointF min = ToCartesianPoint(0, 5);
            PointF max = ToCartesianPoint(Width, 5);

            // Set the current target and visible target to the minimum visible point on the graph
            float currentTarget = min.X;

            // The target variable defines the point visible to the user during the calculation
            target = min.X;

            // Set the finding field to true
            finding = true;

            // Obtain an object to estimate the derivatives of the target function
            var m = new CentralDifferenceMethod(TargetFunction, currentTarget);

            // Make a list object to hold the extrema that have been found
            var ex = new List<Extrema>();

            try
            {
                // While the target is still in the searching bounds
                while (currentTarget < max.X)
                {
                    // Estimate the first derivative
                    double res = m.DeriveFirst();

                    // If the first derivative is within a tolerance to zero
                    if (Math.Abs(Math.Round(res, 3)) < 0.0025)
                    {
                        // Calculate the second derivative
                        double second = m.DeriveSecond();

                        // If the function is at a maximum
                        if (second < 0)
                        {
                            // Add a new Extrema to the list with the current target point
                            ex = AddExtrema(ex, currentTarget, "Local Maximum");
                        }
                        else
                        {
                            // The function is at a minimum
                            // Add a new Extrema to the list with the current target point
                            ex = AddExtrema(ex, currentTarget, "Local Minimum");
                        }
                    }

                    // Increment the target points
                    currentTarget += 0.001F;
                    target += 0.08F;

                    // Set the target point of the differentiation object to get ready for thex next calculation
                    m.TargetPoint = currentTarget;

                    // If the grapher is still searching, repaint the graph to update the user on progress
                    if (target < max.X)
                    {
                        Invalidate();
                        Update();
                    }
                }
            }
            catch (Exception)
            {
                // Prompt the user if any errors occur during the calculations
                MessageBox.Show("Unable to find extrema", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Prompt the user that the grapher has finished searching
            MessageBox.Show("Finished finding extrema", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset variables and return the list of extrema found
            finding = false;
            target = max.X;
            Invalidate();
            return ex;
        }

        // Private method adds a new extrema to the list if it doesnt' already exist in the current result set
        // results - The list of extrema to add the new result to
        // currentTarget - The target point of the extrema
        // type - The type of extrema
        // Methods returns the new list of extrema objects after the new values has been added
        private List<Extrema> AddExtrema(List<Extrema> results, float currentTarget, string type)
        {
            // Calculate the y value of the function at currentTarget
            float y = TargetFunction.EvaluateF(currentTarget.ToString(CultureInfo.InvariantCulture));

            // If the results list is empty, then add the extrema and return the new list
            if (results.Count == 0)
            {
                results.Add(new Extrema(currentTarget, y, type));
                return results;
            }

            // Round the target to avoid innacuracies
            double ex = Math.Round(currentTarget, 2);

            // Determine whether the new extrema should be added to the list
            bool add = true;

            // Loop through each result already in the list
            foreach (Extrema result in results)
            {
                // If a result has the same value as the currentTarget, then set the add boolean to false
                if (Math.Round(result.Target, 2) == ex)
                    add = false;
            }

            // If no duplicate value exists in the list, add the new extrema and return the new list
            if (add)
                results.Add(new Extrema(currentTarget, y, type));

            return results;
        }

        // Method draws the current target point to the component
        // Method takes a Graphics object to allow the point to be drawn
        public void DrawTargetPoint(Graphics g)
        {
            DrawPoint(g, target, TargetFunction.EvaluateF(target.ToString(CultureInfo.InvariantCulture)), Color.Blue,
                      false);
        }

        // Overriden event handler method is fired when the mouse is moved
        protected override void GrapherMouseMove(object sender, MouseEventArgs e)
        {
            // If the grapher is not finding extrema, then the graph can be updated by calling the base class mouse move event handler
            if (!finding)
                base.GrapherMouseMove(sender, e);
        }
    }

    // Class represents a local extrema to a function
    public class Extrema
    {
        // Constructor takes a target, the corresponding y value of the target and the type of extrema
        // Constructor sets the public properties to the values passed in
        public Extrema(float target, float yCoord, string type)
        {
            Target = target;
            YCoordinate = yCoord;
            Type = type;
        }

        // Public float property representing the X coordinate of the extrema
        public float Target { get; private set; }

        // Public float property representing the Y coordinate of the extrema
        public float YCoordinate { get; private set; }

        // Public string property representing the type of extrema (either maximum or minimum)
        public string Type { get; private set; }
    }
}