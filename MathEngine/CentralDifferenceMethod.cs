using System;

namespace MathEngine
{
    // This class is used to estimate the dervivative of a TargetFunction at a target point.
    // The class uses the central divided difference method to estimate the derivative
    public class CentralDifferenceMethod
    {
        // Constructor for CentralDifferenceMethod class. The constructor takes in arguments essential for its
        // operation and assigns them to properties of the object
        // The constructor takes two arguments - 
        // 1. The TargetFunction object that defines the equation of the function to be differentiated, and 
        // allows for the evaluation of the function,
        // 
        // 2. The target point of the differentiation
        public CentralDifferenceMethod(Function function, double target)
        {
            TargetFunction = function;
            TargetPoint = target;
        }

        // Public property for the Target Point. Allows for the target to be changed without having to
        // create an entirely new instance of the class
        public double TargetPoint { get; set; }

        // Public property for the Target TargetFunction. Allows for the target TargetFunction object to be changed 
        // without having to create an entirely new instance of the class
        public Function TargetFunction { get; set; }

        // Return the first derivative of the TargetFunction at the TargetPoint
        public double DeriveFirst()
        {
            // Get a delegate for the TargetFunction to allow evaluation of the function
            Func<double, double> f = TargetFunction.GetDelegate();

            double x = TargetPoint;
            const double h = 0.001;

            // Use the first central divided difference formula to estimate the first derivative
            return (-f(x + 2 * h) + 8 * f(x + h) - 8 * f(x - h) + f(x - 2 * h)) / 12 / h;

            //return (f(x + h) - f(x - h)) / (2 * h);
        }

        // Return the second deriviative of the TargetFunction at the TargetPoint
        public double DeriveSecond()
        {
            // Get a delegate for the TargetFunction to allow evaluation of the function
            Func<double, double> f = TargetFunction.GetDelegate();

            double x = TargetPoint;
            const double h = 0.001;

            // Use the second central divided difference formula to estimate the second derivative
            return (-f(x + 2 * h) + 16 * f(x + h) - 30 * f(x) + 16 * f(x - h) - f(x - 2 * h)) / (12 * h * h);

            //return (f(x + h) - 2 * f(x) + f(x - h)) / (h * h);
        }
    }
}