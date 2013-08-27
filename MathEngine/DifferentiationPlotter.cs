using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class to plot tangents and normals of an equation at a specified point
    // The user can change the target point by dragging the point drawn onto the panel
    // This class inherits from the Plotter class
    // As this class inherits from Plotter, objects can be used as custom components on a form
    public class DifferentiationPlotter : Plotter
    {
        // Determines whether or not the mouse is over the point of differentiation / target point
        private bool isOverDiffPoint;

        // Determines whether or not the mouse is dragging the point of differentiation / target point
        private bool pointDrag;

        // The point of differentiation for tangents and normals to the equation
        private float targetPoint;

        // Constructor for the class sets default values to properties
        public DifferentiationPlotter()
        {
            Gradient = 0F;
            TargetPoint = 1.5F;
        }

        // The gradient of the tangent to the equation at targetPoint
        public float Gradient { get; set; }

        // The gradient of the normal to the equation at targetPoint
        public float NormalGrad { get; set; }

        // The equation of the tangent to the target equation at targetPoint in human readable form
        public string Tangent { get; set; }

        // The equation of the normal to the target equation at targetPoint in human readable form
        public string Normal { get; set; }

        // Public property for the targetPoint allows other classes to set the values of the targetPoint field
        public float TargetPoint
        {
            get { return targetPoint; }
            set
            {
                // Repaint the component when the targetPoint is set
                targetPoint = value;
                Invalidate();
            }
        }

        // Custom event fired when the results have changed
        public event EventHandler<PaintEventArgs> ResultChange;

        // Overrides the base class paint method
        // Paints the graph onto the panel then paints the tangent and normal to the equation at targetPoint
        // Also paints the targetPoint to allow for interaction by the user
        protected override void GrapherPaint(object sender, PaintEventArgs e)
        {
            // Call the paint method of the base class to paint the basic graph and equation
            base.GrapherPaint(sender, e);

            // Get the currrent gradient
            float newRes = Gradient;

            // If the target equation is valid then draw the tangent and normal
            if (!InvalidFunction)
            {
                // Get the new gradient and draw the tangent and normals
                newRes = DrawTangentAndNormalLines(e.Graphics, TargetPoint);

                // If the new gradient is defined then draw the target point
                if (!Single.IsNaN(newRes) && newRes < 1250)
                    DrawTargetPoint(e.Graphics);
                else
                {
                    // Else set the gradient to NaN and fire the result change event
                    // to allow background GUI to update the results
                    Gradient = Single.NaN;
                    FireResultChange(this, e);
                }
            }

            // If the new gradient is different to the old gradient and it its defined
            if (Math.Abs(newRes - Gradient) > 0.0000001 || Single.IsNaN(Gradient))
            {
                // Round the gradient to avoid inaccuracies
                Gradient = (float) Math.Round(newRes, 3);

                // Fire the result change event to allow for the background GUI to update the results
                FireResultChange(this, e);
            }
        }

        // Override the base class method to copy the tangents and normals to the clipboard
        // Method adds an image of the current graph view the clipboard to allow for pasting in other applications
        public override void CopyToClipboard()
        {
            // Create a new Bitmap object to store the image of the graph
            var b = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            // Create a Graphics object to allow the graph to be drawn
            Graphics graphics = Graphics.FromImage(b);

            // Paint the current graph view to the Bitmap using the overriden paint method
            GrapherPaint(null, new PaintEventArgs(graphics, new Rectangle(0, 0, Width, Height)));

            // Draw parameter strings to the image
            graphics.DrawString("f(x) = " + Equation, new Font("Arial", 12), new SolidBrush(Color.Black), 10F, 10F);
            graphics.DrawString(
                "Gradient = " + Math.Round(Gradient, 5).ToString(CultureInfo.InvariantCulture) + " at x = " +
                TargetPoint, new Font("Arial", 12), new SolidBrush(Color.Black), 10, 25);
            graphics.DrawString("Normal Gradient = " + NormalGrad, new Font("Arial", 12), new SolidBrush(Color.Black),
                                10, 40);
            graphics.DrawString("Equation of tangent: " + Tangent, new Font("Arial", 12), new SolidBrush(Color.Black),
                                10, 55);
            graphics.DrawString("Equation of normal: " + Normal, new Font("Arial", 12), new SolidBrush(Color.Black), 10,
                                70);

            graphics.Dispose();

            // Add the image to the clipboard to allow other applications to paste the current graph view
            Clipboard.SetDataObject(b, true);
        }

        // Method fires the custom ResultChange event handler
        private void FireResultChange(object sender, PaintEventArgs args)
        {
            EventHandler<PaintEventArgs> e = ResultChange;

            // If the event is being listened to
            if (e != null)
            {
                // Fire the event
                ResultChange(sender, args);
            }
        }

        // Draw the tangent and normal to the equation at diffAt
        // Method takes a Graphics object to draw to the panel and the target of differention set by the user
        private float DrawTangentAndNormalLines(Graphics g, float diffAt)
        {
            // Create a new object to allow the estimation of the derivative of the target equation TargetFunction at diffAt
            var diff = new CentralDifferenceMethod(TargetFunction, diffAt);

            // The gradient of the function at diffAt
            double grad;

            try
            {
                // Evaluate the target function at diffAt
                float fa = TargetFunction.EvaluateF(diffAt.ToString(CultureInfo.InvariantCulture));

                // Obtain the value of the first derivative of the functiona at diffAt
                // This will become the gradient of the tangent line
                grad = diff.DeriveFirst();


                PointF originPoint = ToCartesianPoint(0, 0);
                PointF widthPoint = ToCartesianPoint(Width, 0);

                // If the gradient is defined
                if (!Double.IsNaN(grad))
                {
                    // Convert the gradient to a float to allow for drawing and get the normal gradient
                    float tangentGrad = Convert.ToSingle(grad);
                    float normalGrad = Convert.ToSingle(-1 / grad);

                    // Set the NormalGrad property to the gradient of the normal line, rounding to avoid innacuracies
                    NormalGrad = (float) Math.Round(normalGrad, 3);

                    // If the gradient is defined and can be drawn
                    if (Math.Abs(grad) < 1250)
                    {
                        // Draw the tangent to the function 
                        DrawLine(g, Color.Red, 3F, originPoint.X, fa + tangentGrad * (originPoint.X - diffAt), widthPoint.X, fa + tangentGrad * (widthPoint.X - diffAt), false);

                        // If the normal is defined and can be drawn
                        if (Math.Abs(grad) > 0.010)
                        {
                            // Draw the normal with a dashed line
                            DrawLine(g, Color.Gray, 3F, originPoint.X, fa + normalGrad * (originPoint.X - diffAt), widthPoint.X, fa + normalGrad * (widthPoint.X - diffAt), true);
                        }
                    }

                    // Get the equations of the tangent and normal and set the corresponding fields to their values
                    Tangent = "y - " + Math.Round(fa, 3) + " = " + Math.Round(grad, 3) + " (x - " +
                              Math.Round(diffAt, 3) + ")";
                    Normal = "y - " + Math.Round(fa, 3) + " = " + NormalGrad + " (x - " + Math.Round(diffAt, 3) + ")";
                }
                else
                {
                    // If the gradient is not defined return NaN
                    return Single.NaN;
                }
            }
            catch (Exception)
            {
                // If any errors occured return NaN
                return Single.NaN;
            }

            // Return the gradient of the function
            return Convert.ToSingle(grad);
        }

        // Method draws the target point of differentiation to the panel
        // If the mouse is currently over the target, a circle will be drawn around the point for feedback
        // Method takes a Graphics object to allow the point to be drawn
        public void DrawTargetPoint(Graphics g)
        {
            // Draw the target point onto the component
            DrawPoint(g, TargetPoint, TargetFunction.EvaluateF(TargetPoint.ToString(CultureInfo.InvariantCulture)),
                      Color.Blue, isOverDiffPoint);
        }

        // Overriden event handler method is fired when a mouse is moved
        protected override void GrapherMouseMove(object sender, MouseEventArgs e)
        {
            // If the target point is being dragged
            if (pointDrag)
            {
                // Calculate the cartesian coordinates of the current mouse position
                PointF point = ToCartesianPoint(e.Location.X, e.Location.Y);

                // If the mouse if over the target point
                if (isOverDiffPoint)
                {
                    // Set the value of the target point to the X cartesian coordinate of the mouse position
                    TargetPoint = point.X;

                    // Repaint the component to show changes
                    Invalidate();
                }
            }
            else
            {
                // Call the base class mouse move event if the point is not being dragged
                base.GrapherMouseMove(sender, e);

                // If the function is valid, determine whether the mouse if over the target point
                if (!InvalidFunction)
                {
                    Point currrentMousePos = e.Location;
                    double x = currrentMousePos.X - Width / 2 - (TargetPoint - OriginX) * Scale;

                    double y = currrentMousePos.Y - Height / 2 +
                                (TargetFunction.Evaluate(TargetPoint.ToString(CultureInfo.InvariantCulture)) - OriginY) *
                                Scale;

                    // Determine of the mouse is over the target point and set the field correspondingly
                    isOverDiffPoint = x * x + y * y < 64;
                }
            }
            // Repaint the component
            Invalidate();
        }

        // Overriden event handler method is fired when a mouse button is lifted
        protected override void GrapherMouseUp(object sender, MouseEventArgs e)
        {
            // Call the base class mouse up event handler
            base.GrapherMouseUp(sender, e);

            // If the mouse button is up, the target point is not being dragged
            pointDrag = false;
        }

        // Overriden event handler method is fired when a mouse button is pressed
        protected override void GrapherMouseDown(object sender, MouseEventArgs e)
        {
            // If the mouse is over the target point and the mouse button is pressed
            if (isOverDiffPoint)
            {
                // Set the drag field to true
                pointDrag = true;
            }
            else
            {
                // If the mouse is not being dragged, call the base class mouse down event handler
                base.GrapherMouseDown(sender, e);
            }
        }
    }
}