using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MathEngine
{
    // This class provides provides a method of evaluating functions and equations
    // This class uses Regular expressions to match built in operators. It then replaces the operator and operands
    // with the result of the calculation. This is repeatedly done until a final result is obtained
    // The class uses a cache to speed up repeated evaluations
    public class Evaluator
    {
        // Regular expression to match parenthesis within the expression
        // expression will match a pair of parenthesis with an expression in between
        private readonly Regex parenthesisRegex = new Regex(@"([a-z]*)\(([^\(\)]+)\)(\^|!?)", RegexOptions.Compiled);
		
        // Regular Expression pattern to match a single number. Can be negative with one decimal point
		private const string NumberPattern = @"(-?\d+\.?\d*)";

        // Dictionary will hold an Operator object for every supported string operator in the Evaluator
        // Each operator in a string form is assigned to an Operator object that provides the Regex and delegate
        private readonly Dictionary<string, Operator> operators = new Dictionary<string, Operator>();

        // Cache to store the results of previously evaluated expressions
        private readonly Dictionary<string, double> cache = new Dictionary<string, double>();

        // Constructor fills the operators Dictionary with a string of every supported operator and an Operator object providing
        // a Regular expression to match the operator and a pointer to a method to obtain the result of the operator
        public Evaluator()
        {	
		    operators.Add("^", new TwoArgumentOperator(new Regex(@NumberPattern + @"\^" + NumberPattern, RegexOptions.Compiled), Power));
		    operators.Add("/", new TwoArgumentOperator(new Regex(@NumberPattern + @"/" + NumberPattern, RegexOptions.Compiled), Div));
            operators.Add("*", new TwoArgumentOperator(new Regex(@NumberPattern + @"\*" + NumberPattern, RegexOptions.Compiled), Mult));
            operators.Add("+", new TwoArgumentOperator(new Regex(@NumberPattern + @"\+" + NumberPattern, RegexOptions.Compiled), Add));
            operators.Add("-", new TwoArgumentOperator(new Regex(@NumberPattern + @"-" + NumberPattern, RegexOptions.Compiled), Sub));
			
            operators.Add("asin", new OneArgumentOperator(new Regex(@"asin" + NumberPattern, RegexOptions.Compiled), Asin));
            operators.Add("acos", new OneArgumentOperator(new Regex(@"acos" + NumberPattern, RegexOptions.Compiled), Acos));
            operators.Add("atan", new OneArgumentOperator(new Regex(@"atan" + NumberPattern, RegexOptions.Compiled), Atan));
			
            operators.Add("sin", new OneArgumentOperator(new Regex(@"sin" + NumberPattern, RegexOptions.Compiled), Sin));
            operators.Add("cos", new OneArgumentOperator(new Regex(@"cos" + NumberPattern, RegexOptions.Compiled), Cos));
            operators.Add("tan", new OneArgumentOperator(new Regex(@"tan" + NumberPattern, RegexOptions.Compiled), Tan));

            operators.Add("abs", new OneArgumentOperator(new Regex(@"abs" + NumberPattern, RegexOptions.Compiled), Abs));
            operators.Add("sqrt", new OneArgumentOperator(new Regex(@"sqrt" + NumberPattern, RegexOptions.Compiled), Sqrt));
            operators.Add("ln", new OneArgumentOperator(new Regex(@"ln" + NumberPattern, RegexOptions.Compiled), Ln));
            operators.Add("e", new OneArgumentOperator(new Regex(@"e" + NumberPattern, RegexOptions.Compiled), Exp));
        }

        // Method matches pairs of parenthesis in the equation
        // Method then replaces the arithmetic inside the parenthesis with the result and returns a string without brackets
        private string MatchParenthesis(string expression)
        {
            // Match the expression parameter with the parenthesis regular expression
            var parentMatch = parenthesisRegex.Match(expression);

            // While there are more matches
            while (parentMatch.Success)
            {
                // Replace the parenthesis along with the arithmetic in between with the result of the calculation
                expression = expression.Replace("(" + parentMatch.Groups[2].Value + ")", SearchForOperators(parentMatch.Groups[2].Value));

                // Match the new expression to check if there are more parenthesis present
                parentMatch = parenthesisRegex.Match(expression);
            }

            // Return a parenthesis free expression
            return expression;
        }

        // Method returns a double from a providing string expression
        public double Evaluate(string expression)
        {
            // Replace all whitespace and convert to lower case
            expression = expression.Replace(" ", "").ToLower();

            // Replace pi and euler with their numerical values
            expression = expression.Replace("pi", Math.PI.ToString(CultureInfo.InvariantCulture));
            expression = expression.Replace("euler", Math.E.ToString(CultureInfo.InvariantCulture));

            // If the cache already contains the expression
            if (cache.ContainsKey(expression))
            {
                // Return the result of the expression in the cache
                return cache[expression];
            }

            // Get a copy of the string before evaluation
            string first = expression;

            // Match and remove all parenthesis within the expression
            expression = MatchParenthesis(expression).ToLower();

            // String result will hold the final expression
            string result = "";

            try
            {
                // If the string already contains NaN, return NaN
                if (result.Contains("nan"))
                    return Double.NaN;

                // If the string already contains infinity, return infinity
                if (result.Contains("infinity"))
                    return Double.PositiveInfinity;

                // Evaluate the expression
                result = SearchForOperators(expression).ToLower();

                // If the result contains NaN, return NaN
                if (result.Contains("nan"))
                    return Double.NaN;

                // If the result contains infinity, return infinity
                if (result.Contains("infinity"))
                    return Double.PositiveInfinity;

                // Convert the final expression into a double
                double res = Convert.ToDouble(result);

                // Add the first string and the result to the cache
                cache.Add(first, res);

                // Return the double result
                return res;
            }
            catch (Exception)
            {
                // If any errors occur, throw a new Exception
                throw new Exception("Invalid function");
            }
        }

        // Method returns a double from a string expression expression with a Dictionary of variables
        // Method replaces the variables in the expression with the corresponding values in the dictionary
        // before returning the result
        public double Evaluate(string expression, Dictionary<string, string> vars)
        {
			// Convert the expression to lower case
			expression = expression.ToLower();
			
            // Loop through each variable in the dictionary
            foreach (var pair in vars)
            {
                // Replace the variable in the pair with the value
                expression = expression.Replace(pair.Key, "(" + pair.Value + ")");
            }

            // Return the evaluated expression
            return Evaluate(expression);
        }

        // Method takes a string expression, checks if it contains any of the supported operators an uses the corresponding
        // regular expression to replace the string with the result of the arithmetic
        private string SearchForOperators(string expression)
        {
            // Search the string for an operator, if it exists, apply the operator with the corresponding regular expression
            // and a delegate to the method corresponding to the operator from the Dictionary of supported operators
			
			foreach(KeyValuePair<string, Operator> op in operators)
			{
				if(expression.IndexOf(op.Key, StringComparison.Ordinal) != -1)
				{
					if(op.Value is OneArgumentOperator)
					{
						expression = ApplyOneArgOperator(op.Value.RegularExpression, expression, ((OneArgumentOperator)op.Value).Calculation);
					}
					else
					{
						expression = ApplyTwoArgOperator(op.Value.RegularExpression, expression, ((TwoArgumentOperator)op.Value).Calculation);
					}
				}
			}
			
			return expression;
        }

        // Method replaces all instances of an operator with one argument from an expression
        // operatorRegex - The regular expression of the operator
        // expression - The string expression to search
        // operatorFunc - The delegate with one argument provides the calculation method for the operator
        private static string ApplyOneArgOperator(Regex operatorRegex, string expression, Func<string, string> operatorFunc)
        {
            // Get a collection of all the matches for the operatorRegex
            var matches = operatorRegex.Matches(expression);

            // If there are not matches, return the existing expression
            if (matches.Count == 0)
                return expression;

            // Loop through each match in the collection
            foreach (Match match in matches)
            {
                // Replace the match value with the result of the operator from the passed in delegate
                expression = expression.Replace(match.Groups[0].Value, operatorFunc(match.Groups[1].Value));
            }

            // Recursively call this method to account for expressions that could be created after an expression has
            // been replaced
            expression = ApplyOneArgOperator(operatorRegex, expression, operatorFunc);

			expression = expression.Replace("--","+");
			
            // Return the final expression
            return expression;
        }

        // Method replaces all instances of an operator with two arguments from an expression
        // operatorRegex - The regular expression of the operator
        // expression - The string expression to search
        // operatorFunc - The delegate with two arguments provides the calculation method for the operator
        private static string ApplyTwoArgOperator(Regex operatorRegex, string expression, Func<string, string, string> operatorFunc)
        {
            // Get a collection of all the matches for the operatorRegex
            var matches = operatorRegex.Matches(expression);

            // If there are not matches, return the existing expression
            if (matches.Count == 0)
                return expression;

            // Loop through each match in the collection
            foreach (Match operatorMatch in matches)
            {
                // Replace the match value with the result of the operator from the passed in delegate
                expression = expression.Replace(operatorMatch.Groups[0].Value, operatorFunc(operatorMatch.Groups[1].Value, operatorMatch.Groups[2].Value));
            }

            // Recursively call this method to account for expressions that could be created after an expression has
            // been replaced
            expression = ApplyTwoArgOperator(operatorRegex, expression, operatorFunc);

			expression = expression.Replace("--","+");
            // Return the final expression
            return expression;
        }

        // Method returns sine of operand
        private static string Sin(string operand)
        {
            return Math.Sin(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns cosine of operand
        private static string Cos(string operand)
        {
            return Math.Cos(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns tan of operand
        private static string Tan(string operand)
        {
            return Math.Tan(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns inverse sine of operand
        private static string Asin(string operand)
        {
            return Math.Asin(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns inverse cosine of operand
        private static string Acos(string operand)
        {
            return Math.Acos(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns inverse tan of operand
        private static string Atan(string operand)
        {
            return Math.Atan(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns square root of operand
        private static string Sqrt(string operand)
        {
            return Math.Sqrt(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns the natural logarithm of operand
        private static string Ln(string operand)
        {
            return Math.Log(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns e to the power of operand
        private static string Exp(string operand)
        {
            return Math.Exp(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method returns absolute value of operand
        private static string Abs(string operand)
        {
            return Math.Abs(Convert.ToDouble(operand)).ToString("F8");
        }

        // Method raises op1 to the power of op2
        private static string Power(string op1, string op2)
        {
            return Math.Pow(Convert.ToDouble(op1), Convert.ToDouble(op2)).ToString("F8");
        }

        // Method multiplies two operands
        private static string Mult(string op1, string op2)
        {
            return (Convert.ToDouble(op1) * Convert.ToDouble(op2)).ToString("F8");
        }

        // Method divides two operands
        private static string Div(string op1, string op2)
        {
            return (Convert.ToDouble(op1) / Convert.ToDouble(op2)).ToString("F8");
        }

        // Method adds two operands
        private static string Add(string op1, string op2)
        {
            return (Convert.ToDouble(op1) + Convert.ToDouble(op2)).ToString("F8");
        }

        // Method subtracts two operands
        private static string Sub(string op1, string op2)
        {
            return (Convert.ToDouble(op1) - Convert.ToDouble(op2)).ToString("F8");
        }
    }
}