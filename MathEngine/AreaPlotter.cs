using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Abtract class providing a base class for all Graphers that integrate a function
    // This class inherits from the Plotter class and all subclasses are custom components
    // Class includes properties defining parameters of integration common to all subclasses
    public abstract class AreaPlotter : Plotter
    {
        // Determines whether the mouse is currently hovering over the Lower Limit
        protected bool IsOverLower;

        // Determines whether the mouse is currently hovering over the Upper Limit
        protected bool IsOverUpper;

        // The number of iterations for use in the integration algorithm of the subclass
        protected int IterationNum;

        // The lower limit of integration
        protected float LowerLimit;

        // The upper limit of integration
        protected float UpperLimit;

        // Determines whether the lower limit point is being dragged by the mouse
        protected bool PointLowerDrag;

        // Deterimines whether the upper limit is being dragged by the mouse
        protected bool PointUpperDrag;

        // Base constructor for all AreaPlotter objects.
        // Method gives default values to local fields and instantiates objects.
        protected AreaPlotter()
        {
            LowerLimit = 0;
            UpperLimit = 10;
            IterationNum = 6;
            Result = 0;
            Ordinates = new List<Ordinate>();
        }

        // List object property representing the Ordinates of the integration method at the current limits
        public List<Ordinate> Ordinates { protected set; get; }

        // Float property representing the Lower limit of integration
        public float Lower
        {
            get { return LowerLimit; }
            set
            {
                LowerLimit = value;

                // Repaint the grapher when the value is changed
                Invalidate();
            }
        }

        // Float property representing the Upper limit of integration
        public float Upper
        {
            get { return UpperLimit; }
            set
            {
                UpperLimit = value;
                // Repaint the grapher when the value is changed
                Invalidate();
            }
        }

        // Integer property representing the number of iterations to use
        public int Iterations
        {
            get { return IterationNum; }
            set
            {
                IterationNum = value;

                // Repaint the grapher when the value is changed
                Invalidate();
            }
        }

        // Float property representing the Result of the integration algorithm after execution
        public float Result { get; set; }

        // Returns the delta value for the current algorithm parameters
        // This value is used in the execution of the integration algorithm of the subclass
        protected float GetDelta()
        {
            return (float) Math.Round(Math.Abs(Upper - Lower) / Iterations, 4);
        }

        // Override the CopyToClipboard method of the base class to paint on integration parameters
        // Method adds an image of the current graph view the clipboard to allow for pasting in other applications
        public override void CopyToClipboard()
        {
            // Create a new Bitmap object to store the image of the graph
            var b = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            // Create a Graphics object to allow the graph to be drawn
            Graphics graphics = Graphics.FromImage(b);

            // Paint the graph to the Bitmap object
            GrapherPaint(null, new PaintEventArgs(graphics, new Rectangle(0, 0, Width, Height)));

            // Paint integration parameter strings to the Bitmap 
            graphics.DrawString("f(x) = " + Equation, new Font("Arial", 12), new SolidBrush(Color.Black), 10F, 10F);
            graphics.DrawString("Lower = " + Lower + ", Upper = " + Upper + ", Iterations = " + Iterations,
                                new Font("Arial", 12), new SolidBrush(Color.Black), 10F, 25F);
            graphics.DrawString(GetFormulaString(Ordinates), new Font("Arial", 12), new SolidBrush(Color.Black), 10F,
                                40F);
            graphics.DrawString("Integral = " + Result.ToString(CultureInfo.InvariantCulture), new Font("Arial", 12),
                                new SolidBrush(Color.Black), 10F, 55F);

            graphics.Dispose();

            // Add the Bitmap object to the clipboard for pasting in other applications
            Clipboard.SetDataObject(b, true);
        }

        // Returns the Ordinate o in a friendly string form for the formula string of the integration algorithm
        protected string GetOrdinateForFormula(Ordinate o)
        {
            if (o.Y < 0)
                return " " + Math.Round(o.Y, 4);

            // If the Y value is not negative, manually add a plus sign to the beginning
            return " + " + Math.Round(o.Y, 4);
        }

        // Draw the limits of integration onto the graph, specifying whether or not the mouse is hovering over
        // either limit to allow for different painting styles
        public void DrawEndPoints(Graphics g)
        {
            DrawPointOnXAxis(g, Upper, Color.Green, IsOverUpper);
            DrawPointOnXAxis(g, Lower, Color.Blue, IsOverLower);
        }

        // Overriden event handler method is fired when the mouse is moved
        protected override void GrapherMouseMove(object sender, MouseEventArgs e)
        {
            // If either point is being dragged
            if (PointLowerDrag || PointUpperDrag)
            {
                // Get the cartesian point of the current mouse location
                PointF point = ToCartesianPoint(e.Location.X, e.Location.Y);

                // If mouse is over the lower point, set the points position to the cartesian form 
                // of the mouse position
                if (IsOverLower)
                {
                    Lower = point.X;
                    Invalidate();
                }
                    // If mouse is over the upper point, set the points position to the cartesian form 
                    // of the mouse position
                else if (IsOverUpper)
                {
                    Upper = point.X;
                    Invalidate();
                }
            }
            else
            {
                // If not dragging any points determine whether the mouse is currently over either point
                base.GrapherMouseMove(sender, e);

                // Determine the cartesian coordinates of the current mouse position
                Point currentMousePos = e.Location;
                double x1 = currentMousePos.X - Width / 2 - (Lower - OriginX) * Scale;
                double x2 = currentMousePos.X - Width / 2 - (Upper - OriginX) * Scale;
                double y = currentMousePos.Y - YofX;

                // If the mouse is currently over the lower limit
                if (x1 * x1 + y * y < 64)
                {
                    IsOverLower = true;
                    IsOverUpper = false;
                }
                    // If the mouse is currently over the upper limit
                else if (x2 * x2 + y * y < 64)
                {
                    IsOverUpper = true;
                    IsOverLower = false;
                }
                    // If the mouse is not over either point
                else
                {
                    IsOverUpper = false;
                    IsOverLower = false;
                }
            }

            // Repaint the component to update changes to the points
            Invalidate();
        }

        // Overriden event handler method is fired when a mouse button is released
        protected override void GrapherMouseUp(object sender, MouseEventArgs e)
        {
            // Call the base class implementation of the method
            base.GrapherMouseUp(sender, e);

            // If the mouse button is released, neither point is being dragged
            PointUpperDrag = false;
            PointLowerDrag = false;
        }

        // Overriden event handler method is fired when a mouse button is pressed
        protected override void GrapherMouseDown(object sender, MouseEventArgs e)
        {
            // If the mouse is over either limit set the drag property to true
            if (IsOverLower || IsOverUpper)
            {
                PointUpperDrag = true;
                PointLowerDrag = true;
            }
            else
            {
                // Else just call the base class implementation
                base.GrapherMouseDown(sender, e);
            }
        }

        // A custom event that gets fired when the Result of the integration is changed
        // Allows for efficient updating of the result in the GUI
        public event EventHandler<PaintEventArgs> ResultChange;

        // Method fires the custom ResultChange event handler
        protected void FireResultChange(object sender, PaintEventArgs args)
        {
            EventHandler<PaintEventArgs> e = ResultChange;

            // If the event is being listened to
            if (e != null)
            {
                // Fire the event
                ResultChange(sender, args);
            }
        }

        // Abstract method returns the Formula String of the independant integration algorithm being
        // used in the subclass. As marked abstract, all subclasses must provide an implementation
        // Method takes a list of Ordinates as a parameter
        public abstract string GetFormulaString(List<Ordinate> ordinates);
    }

    // Wrapper class representing one Ordinate in the integration algorithm
    public class Ordinate
    {
        // Constructor takes in an X and Y value before assigning the properties to their values
        public Ordinate(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Float property representing the X value of the ordinate
        // Propety can only be set by methods in this class
        public float X { get; private set; }

        // Float property representing the X value of the ordinate
        // Propety can only be set by methods in this class
        public float Y { get; private set; }
    }
}