using System;
using System.Text.RegularExpressions;

namespace MathEngine
{
    // Class provides an abstract base class to all operator subclasses
    // This base class contains a Regular Expression property common to all operator classes
    // Class is abstract and so cannot be instantiated
    internal abstract class Operator
    {
        // A Regular Expression property that will match the operator that the object represents
        public Regex RegularExpression { get; set; }
    }

    // Class representing an operator with one argument (e.g sine, cosine etc)
    // Class inherits from the abstract base class Operator
    // Class provides a constructor to initialise the properties and a delegate representing
    // the calculation that the operator defines
    internal class OneArgumentOperator : Operator
    {
        // Constructor initialises the properties of the class
        // regex - The Regular Expression that will match the operator the class represents
        // calculationFunction - A pointer to a method that returns the result of the operator the class represents
        public OneArgumentOperator(Regex regex, Func<string, string> calculationFunction)
        {
            RegularExpression = regex;
            Calculation = calculationFunction;
        }

        // Property representing a pointer to a method that returns the result of the operator with one argument
        public Func<string, string> Calculation { get; set; }
    }

    // Class representing an operator with two arguments (e.g +, * etc)
    // Class inherits from the abstract base class Operator
    // Class provides a constructor to initialise the properties and a delegate representing
    // the calculation that the operator defines
    internal class TwoArgumentOperator : Operator
    {
        // Constructor initialises the properties of the class
        // regex - The Regular Expression that will match the operator the class represents
        // calculationFunction - A pointer to a method that returns the result of the operator the class represents
        public TwoArgumentOperator(Regex regex, Func<string, string, string> calculationFunction)
        {
            RegularExpression = regex;
            Calculation = calculationFunction;
        }

        // Property representing a pointer to a method that returns the result of the operator with two arguments
        public Func<string, string, string> Calculation { get; set; }
    }
}
