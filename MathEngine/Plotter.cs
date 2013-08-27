using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace MathEngine
{
    // Class defines a graph with one target function
    // Graphing area can be interactively changed using the mouse
    // Class inherits from Panel and is a custom component that can be added to any form
    // Class paints the graph onto a standard Panel component
    public class Plotter : Panel
    {
        // Determines the number of pixels per unit on the graph
        protected const float Pixels = 100.0F;

        // Array of floats representing each zoom level
        protected readonly float[] Units = {
                                               0.00005F, 0.0001F, 0.0002F, 0.0005F, 0.001F, 0.002F, 0.005F, 0.01F, 0.02F
                                               ,
                                               0.05F, 0.1F,
                                               0.2F, 0.5F, 1.0F, 2.0F, 5.0F, 10.0F, 20.0F, 50.0F, 100.0F, 200.0F, 500.0F
                                               ,
                                               1000.0F
                                           };

        // The position of the old origin when the graph has been dragged
        protected Point CurrentPoint;

        // Determine whether or not the graph is being dragged
        protected bool IsDrag;

        // The X coordinate of the center of the graph (in terms of units on the axis)
        public float OriginX;

        // The Y coordinate of the center of the graph (in terms of units on the axis)
        public float OriginY;

        // Determines the number of pixels per unit on the screen
        protected new float Scale;

        // The Function object representing the equation of the function to draw
        protected Function TargetFunction;

        // The X coordinate of y-axis (in terms of pixels)
        protected float XofY;

        // The Y coordinate of x-axis (in terms of pixels)
        protected float YofX;

        // The current zoom level
        public int Zoom = 14;

        // Constructor sets default values to fields and adds event handlers
        public Plotter()
        {
            TargetFunction = new Function("x^2-3");
            BackColor = Color.White;
            InvalidFunction = false;
            Paint += GrapherPaint;
            MouseClick += GrapherMouseClick;
            MouseEnter += GrapherMouseEnter;
            MouseMove += GrapherMouseMove;
            MouseWheel += GrapherMouseWheel;
            MouseDown += GrapherMouseDown;
            MouseUp += GrapherMouseUp;
            KeyDown += GrapherKeyDown;
            DoubleBuffered = true;
        }

        // Public string property representing the equation of the target function
        public string Equation
        {
            // Getter method returns the Equation property of the TargetFunction Function object
            get { return TargetFunction.Equation; }

            // Setter method determines whether or not the function is valid and sets the new equation
            set
            {
                TargetFunction = new Function(value);
                InvalidFunction = true;
                string error;
                // Validate the function
                InvalidFunction = ValidateFunction(out error);

                // If the function is invalid, show a messagebox to prompt the user
                if (InvalidFunction)
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Repaint the control to update changes
                Invalidate();
            }
        }

        // Protected boolean representing whether or not the function is invalid
        // Property can only be accessed by this class or any subclasses
        // Property can only be set by this class
        protected bool InvalidFunction { get; private set; }

        // Method validates the current TargetFunction
        // Method returns true if the function is invalid, otherwise false
        // Error messages are passed back to the caller through an out parameter (a pointer to the string in memory)
        protected bool ValidateFunction(out string errorMessage)
        {
            try
            {
                // Attempt to get a value from the function
                float t = TargetFunction.EvaluateF((1).ToString(CultureInfo.InvariantCulture));
                errorMessage = "";

                // If the result is not defined
                if (Single.IsNaN(t))
                {
                    // Set the error message and return true
                    errorMessage = "Invalid Function";
                    return true;
                }

                // If no errors occured and the result is defined, return false
                return false;
            }
            catch (Exception)
            {
                // If an error occured, set the error message and return true
                errorMessage = "Invalid Function";
                return true;
            }
        }

        // Method adds an image of the current graph view the clipboard to allow for pasting in other applications
        // Method is marked as virtual and so can be overriden in subclasses
        public virtual void CopyToClipboard()
        {
            // Create a new Bitmap object to store the image of the graph
            var b = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            // Create a Graphics object to allow the graph to be drawn
            Graphics graphics = Graphics.FromImage(b);

            // Paint the current graph view to the Bitmap
            GrapherPaint(null, new PaintEventArgs(graphics, new Rectangle(0, 0, Width, Height)));

            // Add the equation of the target function to the image
            graphics.DrawString("f(x) = " + Equation, new Font("Arial", 12), new SolidBrush(Color.Black), 10F, 10F);
            graphics.Dispose();

            // Add the image to the clipboard to allow other applications to paste the graph view
            Clipboard.SetDataObject(b, true);
        }

        // Paint event handler method draws the gridlines, axis and function onto the Panel
        // Method is virtual and so can be overidden in subclasses
        protected virtual void GrapherPaint(object sender, PaintEventArgs e)
        {
            // Add antialising to smooth the graphics
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Determine the current pixel scale
            Scale = Pixels / Units[Zoom];

            // Fill the current view with plain white to remove previous graphics
            e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);

            // Draw the grid lines
            DrawGridLines(e.Graphics);

            try
            {
                // If the current function is not invalid, draw the function onto the graph
                if (!InvalidFunction)
                    DrawFunction(TargetFunction, e.Graphics, TargetFunction.Colour);
            }
            catch (Exception)
            {
            }

            // Draw the x and y axis lines
            DrawAxes(e.Graphics);

            // Draw the origin point
            DrawPoint(e.Graphics, 0, 0, 3F, Color.Black);

            // Draw the crosshair to denote the center of the current view
            DrawCrosshair(e.Graphics);
        }

        // Method draws the crosshair onto the graph
        // The crosshair will appear in the center of the graphing view with radius of 5 pixels and width of 0.5
        // Method can only be accessed in this class and in any subclasses
        protected void DrawCrosshair(Graphics g)
        {
            g.DrawLine(new Pen(new SolidBrush(Color.Gray), 0.5F), Width / 2 - 5, Height / 2,
                       Width / 2 + 5, Height / 2);
            g.DrawLine(new Pen(new SolidBrush(Color.Gray), 0.5F), Width / 2, Height / 2 - 5,
                       Width / 2, Height / 2 + 5);
        }

        // Method draws a defined point onto the graph
        // Method can only be accessed in this class and in any subclasses
        // g - The Graphics object used to draw the point
        // x - The x coordinate of the point (cartesian)
        // y - The y coordinate of the point (cartesian)
        // colour - The colour of the point
        protected void DrawPoint(Graphics g, float x, float y, float radius, Color colour)
        {
            // Convert the cartesian points to pixel values on the component
            PointF p = ToScreenPoint(x, y);

            // Draw the point with specified radius and colour
            g.FillEllipse(new SolidBrush(colour), p.X - radius, p.Y - radius, 2 * radius, 2 * radius);
        }

        // Method draws grid lines onto the graph
        // g - The Graphics object used to draw the grid lines
        // Method can only be accessed in this class or in subclasses
        protected void DrawGridLines(Graphics g)
        {
            //Create a pen object to draw the grid lines in light gray with a width of 0.2
            var gridPen = new Pen(new SolidBrush(Color.LightGray), 0.2F);

            float maxWidth = (OriginY + Height / 2 / Scale) / Units[Zoom] + 1;
            float maxHeight = (OriginX + Width / 2 / Scale) / Units[Zoom] + 1;

            // Whilst the current position is still in the bounds of the graph, draw the X grid lines
            // Increment the counter variable by 0.2 (the width of the pen)
            for (float a = (int) ((OriginY - Height / 2 / Scale) / Units[Zoom]) - 1;
                 a < maxWidth;
                 a = a + 0.2F)
            {
                // Draw the X grid line
                g.DrawLine(gridPen, 0F, Height / 2 - (a * Units[Zoom] - OriginY) * Scale,
                           Width,
                           Height / 2 - (a * Units[Zoom] - OriginY) * Scale);
            }

            // Whilst the current position is still in the bounds of the graph, draw the Y grid lines
            // Increment the counter variable by 0.2 (the width of the pen)
            for (float a = (int) ((OriginX - Width / 2 / Scale) / Units[Zoom]) - 1;
                 a < maxHeight;
                 a = a + 0.2F)
            {
                g.DrawLine(gridPen, Width / 2 + (a * Units[Zoom] - OriginX) * Scale, 0F,
                           Width / 2 + (a * Units[Zoom] - OriginX) * Scale,
                           Height);
            }

            // Dipose of the pen to free resources
            gridPen.Dispose();
        }

        // Method draws the function onto the graph in the specified colour
        // Method calculates two points of the function very close together and adds a line joining
        // the points together to simulate a smooth line
        // function - The function object to draw
        // g - The Graphics object used to draw the grid lines
        // The colour to draw the function in
        // Method can only be accessed in this class or in subclasses
        protected void DrawFunction(Function function, Graphics g, Color color)
        {
            // Create a pen object with the specified colour and a width of 3.5
            var funcPen = new Pen(new SolidBrush(color), 4.0F);

            // Create a graphics path object which will store the points of the function
            var line = new GraphicsPath();

            // Calculate the minimum point of the x axis on the current graph view
            float target = (OriginX - Width / 2 / Scale);

            // Calculate the corresponding y value to the first point
            float y0 = Height / 2 -
                       (function.EvaluateF(target.ToString(CultureInfo.InvariantCulture)) - OriginY) * Scale;

            // Loop through each point on the x axis, calculate the y value and add or draw the line
            for (float i = 0.0F; i < Width; i += 1.2F)
            {
                // Calculate the new x value at the current iteration of the counter variable
                target = (OriginX + (i + 1 - Width / 2) / Scale);

                // Calculate the corresponding y value to the new target
                float y1 = Height / 2 -
                           (function.EvaluateF(target.ToString(CultureInfo.InvariantCulture)) - OriginY) * Scale;

                try
                {
                    // If the Y value is defined
                    if (!float.IsNaN(y0) && !float.IsNaN(y1))
                    {
                        // If the current equation includes logarithms or exponentials, draw the line without adding to the path to improve
                        // drawing speed when dealing with these operators (as they can grow very quickly and get out of the bounds of the path)
                        // Else add the current line to the graphics path
                        if (Equation.Contains("ln") || Equation.Contains("e"))
                            g.DrawLine(funcPen, i, y0, i + 1, y1);
                        else
                            line.AddLine(i, y0, i + 1, y1);
                    }
                }
                catch (Exception)
                {
                    // Exit the method if any errors occur
                    return;
                }

                // Assign the previous point to the current point to move the line onwards 
                // whilst still keeping a record of the previous point to join the points together
                y0 = y1;
            }

            // Draw the path representing the function onto the graph
            g.DrawPath(funcPen, line);

            // Dispose of the pen object to free resources
            funcPen.Dispose();
        }

        // Method draws the X and Y axis onto the graph as well as numerical labels
        // g - The Graphics object used to draw the grid lines
        // Method can only be accessed in this class or in subclasses
        protected void DrawAxes(Graphics g)
        {
            // Create a new Font object for use when drawing the labels
            var axFont = new Font("Arial", 9);

            // Create a new Pen object for use when drawing the axis lines with a black colour and width of 2
            var axPen = new Pen(new SolidBrush(Color.Black), 2);

            // Get the screen coordinates in pixels of the origin in cartesian coordinates
            PointF originPoint = ToScreenPoint(0, 0);

            // Set the x position of the y axis to the calculated X value
            XofY = originPoint.X;

            // Bounds check the position to make sure it isnt less the minimum value and is not greater than the width
            // This is to make sure that the axis does not disappear when the current graph view does not include the axis line 
            if (originPoint.X < 10)
                XofY = 10;
            else if (originPoint.X > Width - 10)
                XofY = Width - 15;

            // Set they y position of the x axis to the calculated Y value
            YofX = originPoint.Y;

            // Bounds check the position to make sure it isnt less the minimum value and is not greater than the width
            // This is to make sure that the axis does not disappear when the current graph view does not include the axis line 
            if (originPoint.Y < 10)
                YofX = 10;
            else if (originPoint.Y > Height - 10)
                YofX = Height - 15;

            // Draw the Y axis line
            g.DrawLine(axPen, XofY, 0F, XofY, Height);

            // Draw the X axis line
            g.DrawLine(axPen, 0F, YofX, Width, YofX);

            string str;

            // Label the Y axis

            // Calculate the maximum value of the y axis in the current view
            float maxHeight = (OriginY + Height / 2 / Scale) / Units[Zoom] + 1;

            // Loop through values on the Y axis whilst the current position is less than the maximum position on the axis
            for (int a = (int) ((OriginY - Height / 2 / Scale) / Units[Zoom]) - 1;
                 a < maxHeight;
                 a++)
            {
                // If the current value on the y axis is zero, do not label the value (prevents zero appearing at the origin)
                if (a != 0)
                {
                    // Convert the current value on the y axis to a string ready to label
                    str = (a * Units[Zoom]).ToString(CultureInfo.InvariantCulture);

                    // Draw the current string onto the graph at the current position on the x axis
                    g.DrawString(str, axFont, new SolidBrush(Color.Black), XofY,
                                 Height / 2 - (a * Units[Zoom] - OriginY) * Scale - 7);
                }
            }

            // Label the X axis

            // Calculate the maximum value of the y axis in the current view
            float maxWidth = (OriginX + Width / 2 / Scale) / Units[Zoom] + 1;

            // Loop through values on the X axis whilst the current position is less than the maximum position on the axis
            for (int a = (int) ((OriginX - Width / 2 / Scale) / Units[Zoom]) - 1;
                 a < maxWidth;
                 a++)
            {
                // If the current value on the x axis is zero, do not label the value (prevents zero appearing at the origin)
                if (a != 0)
                {
                    // Convert the current value on the x axis to a string ready to label
                    str = (a * Units[Zoom]).ToString(CultureInfo.InvariantCulture);

                    // Draw the current string onto the graph at the current position on the x axis
                    g.DrawString(str, axFont, new SolidBrush(Color.Black),
                                 Width / 2 + (a * Units[Zoom] - OriginX) * Scale - 7, YofX + 2);
                }
            }

            // Dispose of the font and pen objects to free resources
            axFont.Dispose();
            axPen.Dispose();
        }

        // Method draws a point on the x axis of the graph
        // g - The Graphics object to allow the drawing of the point
        // x - The x coordinate of the point to draw on the x axis
        // color - The colour to draw the point
        // over - a boolean to determine whether or not the mouse is over the point. Another ellipse will be drawn if true
        // Method can only be accessed in this class or in subclasses
        protected void DrawPointOnXAxis(Graphics g, float x, Color color, bool over)
        {
            // The radius of the point to draw
            const int radius = 4;

            // Get the screen coordinate of the x value when y is zero
            PointF xValOnAxisPoint = ToScreenPoint(x, 0);

            // Create a new rectangle object to represent the circle to draw
            var arc = new RectangleF(xValOnAxisPoint.X - radius, YofX - radius, 2 * radius, 2 * radius);

            // If over is true, draw another slightly larger ellipse around the existing ellipse
            if (over)
            {
                g.DrawEllipse(new Pen(Color.Black, 1.5F),
                              new RectangleF(xValOnAxisPoint.X - radius - 3, YofX - radius - 3, 2 * radius + 6, 2 * radius + 6));
            }

            // Fill the ellipse or draw the point in the defined colour
            g.FillEllipse(new SolidBrush(color), arc);

            // Draw the border of the point to improve clarity
            g.DrawEllipse(new Pen(Color.Black, 1.0F), arc);
        }

        // Method draws a point onto the graph
        // g - The Graphics object to allow the drawing of the point
        // x - The x coordinate of the point to draw
        // y - The y coordinate of the point to draw
        // color - The colour to draw the point
        // over - a boolean to determine whether or not the mouse is over the point. Another ellipse will be drawn if true
        // Method can only be accessed in this class or in subclasses
        protected void DrawPoint(Graphics g, float x, float y, Color colour, bool over)
        {
            // The radius of the point to draw
            const int radius = 4;

            // Get the screen coordinates of the passed in cartesian points
            PointF point = ToScreenPoint(x, y);

            // Create a new rectangle object to represent the circle to draw
            var arc = new RectangleF(point.X - radius, point.Y - radius, 2 * radius, 2 * radius);

            // If over is true, draw another slightly larger ellipse around the existing ellipse
            if (over)
            {
                g.DrawEllipse(new Pen(Color.Black, 1.5F),
                              new RectangleF(point.X - radius - 3, point.Y - radius - 3, 2 * radius + 6, 2 * radius + 6));
            }

            // Fill the ellipse or draw the point in the defined colour
            g.FillEllipse(new SolidBrush(colour), arc);

            // Draw the border of the point to improve clarity
            g.DrawEllipse(new Pen(Color.Black, 1.0F), arc);
        }

        // Method draws a line onto the graph
        // g - The Graphics object to allow the drawing of the point
        // colour - The colour of the line to draw
        // lineWidth - The width of the line to draw
        // a - The x coordinate of the first point
        // b - The first y coordinate of the first point
        // x - The x coordinate of the second point
        // y - The y coordinate of the second point
        // dashed -  Determines whether the line to draw will be dashed
        // Method can only be accessed in this class or in subclasses
        protected void DrawLine(Graphics g, Color colour, float lineWidth, float a, float b, float x, float y,
                                bool dashed)
        {
            // If all four points are defined then draw the line
            if (!Single.IsNaN(a) && !Single.IsNaN(b) && !Single.IsNaN(x) && !Single.IsNaN(y))
            {
                // Get the corresponding screen coordinates of the passed in cartesian points
                PointF x1Point = ToScreenPoint(a, b);
                PointF x2Point = ToScreenPoint(x, y);

                // Create a new pen with the specified colour and width
                var p = new Pen(colour, lineWidth);

                // If the dashed parameter is true, set the pen style to dashed
                if (dashed)
                {
                    p.DashStyle = DashStyle.Dash;
                }

                // Draw the line
                g.DrawLine(p, x1Point.X, x1Point.Y, x2Point.X, x2Point.Y);
            }
        }

        // Method converts a cartesian coordinate into its corresponding screen coordinate (pixel values of the component)
        // x - The x coordinate
        // y - The y coordinate
        // Method returns a Point object representing the corresponding screen coordinate
        protected PointF ToScreenPoint(float a, float b)
        {
            return new PointF(Width / 2 + (a - OriginX) * Scale, Height / 2 - (b - OriginY) * Scale);
        }

        // Method converts a screen coordinate (pixel values of the component) into its corresponding cartesian coordinate
        // x - The x coordinate
        // y - The y coordinate
        // Method returns a Point object representing the corresponding cartesian coordinate
        public PointF ToCartesianPoint(float x, float y)
        {
            return new PointF(OriginX + (x - Width / 2) / Scale, OriginY - (y - Height / 2) / Scale);
        }

        // Method zooms the current graph view in
        public void ZoomIn()
        {
            // Determine whether the minimum zoom level has been reached
            Zoom = Math.Max(Zoom - 1, 0);

            // Update the new pixel scale
            Scale = Pixels / Units[Zoom];
        }

        // Method zooms the current graph view out
        public void ZoomOut()
        {
            // Determine whether or not the max zoom level has been reached
            Zoom = Math.Min(Zoom + 1, Units.Length - 1);

            // Update the new pixel scale
            Scale = Pixels / Units[Zoom];
        }

        // Event handler is fired when a key is pressed
        private void GrapherKeyDown(object sender, KeyEventArgs e)
        {
            // Handle the key that has been pressed
            HandleKeyDown(e.KeyCode);
        }

        // Method handles key presses from the parent form
        // key - the key that has been preesed
        protected void HandleKeyDown(Keys key)
        {
            switch (key)
            {
                    // If the space key has been hit, reset the graphing view
                case Keys.Space:
                    OriginX = 0;
                    OriginY = 0;
                    Zoom = 14;
                    break;
            }

            // Repaint the graph to update changes
            Invalidate();
        }

        // Event handler method is fired when the mouse moves over the graphing view
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseMove(object sender, MouseEventArgs e)
        {
            // If the graph is being dragged
            if (IsDrag)
            {
                // Calculate the new coordinates of the origin to simulate interactively changing the graphing view
                OriginX -= (e.Location.X - CurrentPoint.X) / Scale;
                OriginY += (e.Location.Y - CurrentPoint.Y) / Scale;
                CurrentPoint = e.Location;

                // Repaint the graph to update changes
                Invalidate();
            }

            EventHandler<MouseEventArgs> ev = PositionMove;

            // If the PositionMove event is being listened to
            if (ev != null)
            {
                // Fire the event, passing in the current mouse coordinates
                PositionMove(this, new MouseEventArgs(MouseButtons.None, 0, e.X, e.Y, 0));
            }
        }

        // Event is fired when the mouse position is moved within the graphing area
        public event EventHandler<MouseEventArgs> PositionMove;

        // Event handler method is fired when the mouse enters the graphing view
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseEnter(object sender, EventArgs e)
        {
            // Focus the graph when the mouse enters
            Focus();
        }

        // Event handler method is fired when the mouse is clicked
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseClick(object sender, MouseEventArgs e)
        {
            // Set the old point position to the current mouse position
            CurrentPoint = e.Location;
        }

        // Event handler is fired when the mouse wheel is moved
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseWheel(object sender, MouseEventArgs e)
        {
            // Determine which may mouse wheel was scrolled in
            int notches = e.Delta;

            // If the mouse wheel was moved back, zoom the graph out
            if (notches <= 0)
            {
                ZoomOut();
            }
            else
            {
                // Else the mouse wheel mouse moved forward so zoom in
                ZoomIn();
            }

            // Repaint the component to update changes
            Invalidate();
        }

        // Event handler method is fired when the mouse button is released
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseUp(object sender, MouseEventArgs e)
        {
            // When the mouse button is released, the graph is not being dragged
            IsDrag = false;
        }

        // Event handler method is fired when the mouse button is pressed
        // Method is marked virtual and so can be overriden in subclasses
        protected virtual void GrapherMouseDown(object sender, MouseEventArgs e)
        {
            // Set the old point position to the current mouse position
            CurrentPoint = e.Location;

            // Set the drag property to true
            IsDrag = true;

            // Focus the component
            Focus();
        }
    }
}