using System;

namespace Lab3
{
    class MathFunctions
    {
        private double Xmin;
        private double Xmax;
        public double X_min { 
            get 
            { 
                return Xmin;
            }
            set
            {
                if (value >= Xmax)
                {
                    throw new ArgumentException("Min cannot be bigger than max");
                }
                Xmin = value;
            }
        }
        public double X_max {
            get
            {
                return Xmax;
            }
            set
            {
                if (value <= Xmin)
                {
                    throw new ArgumentException("Min cannot be bigger than max");
                }
                Xmax = value;
            } 
        }

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
                return 0;
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
