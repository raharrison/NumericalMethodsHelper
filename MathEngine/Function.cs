using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace MathEngine
{
    // This class encapusulates an equation of a function, whilst also providing an interface to
    // evaluate the function at a specific target point.
    public class Function
    {
        // Private Evaluator instance to evaluate the function at a given point
        private readonly Evaluator evaluator;

        // First constructor takes just the string for the equation.
        // Defaults values for the main constructor are set to 'x' and black
        // This constructor calls the second main constructor that assigns values to the properties
        public Function(string equation)
            : this(equation, "x", Color.Black)
        {
        }

        // Second constructor takes a string equation, string variable and a color
        // This constructor is called by the other constructor to assign properties and instantiates the Evaluator object
        public Function(string equation, string variable, Color colour)
        {
            Equation = equation;
            Variable = variable;
            Colour = colour;
            evaluator = new Evaluator();
        }

        // String property representing the equation of the function
        // Property can only be set privately and not by callers or other classes
        public string Equation { get; private set; }

        // String property representing the variable of the function
        // Property can only be set privately and not by callers or other classes
        public string Variable { get; private set; }

        // Colour property representing the color of the function
        // Property can only be set privately and not by callers or other classes
        public Color Colour { get; private set; }

        // Evaluate the function when Variable = value
        // Returns a double representing the evaluated result
        public double Evaluate(string value)
        {
            string val = Convert.ToDouble(value).ToString("F8");
            var vars = new Dictionary<string, string> {{Variable, val}};

            // Use the private Evaluator instance to perform the evaluation, passing in
            // a dictionary of variables and values.
            return evaluator.Evaluate(Equation, vars);
        }

        // Evaluate the function when Variable = value
        // Returns a float representing the evaluation of the function
        public float EvaluateF(string value)
        {
            string val = Convert.ToSingle(value).ToString("F8");
            var vars = new Dictionary<string, string> {{Variable, val}};

            // Use the private Evaluator instance to perform the evaluation, passing in
            // a dictionary of variables and values.
            return Convert.ToSingle(evaluator.Evaluate(Equation, vars));
        }

        // Returns a delegate that can be used to evaluate the function with a simpler syntax
        // e.g - 
        // var f = functionObject.GetDelegate();
        // double result = f("3");
        public Func<double, double> GetDelegate()
        {
            return x => Evaluate(x.ToString(CultureInfo.InvariantCulture));
        }
    }
}