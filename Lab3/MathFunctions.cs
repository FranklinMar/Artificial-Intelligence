using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class MathFunctions
    {
        double X_min;
        double X_max;

        public MathFunctions(double Min, double Max)
        {
            X_min = Min;
            X_max = Max;
        }
        // Y(x) = x * sin(5*x), x = [-2...5]
        public double MaxSineFunction(Genome<double> X)
        {
            if (X.Variable < X_min || X.Variable > X_max)
            {
                return 0;//Double.MinValue;
            }
            return X.Variable * Math.Sin(5 * X.Variable);
        }

        public double MinSineFunction(Genome<double> X)
        {
            if (X.Variable < X_min || X.Variable > X_max)
            {
                return 0;
            }
            return -MaxSineFunction(X);
        }
    }
}
