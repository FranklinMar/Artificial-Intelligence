using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class MathFunctions
    {
        // Y(x) = x * sin(5*x), x = [-2...5]
        public double SineFunction(Genome<double> X)
        {
            return X.Variable * Math.Sin(5 * X.Variable);
        }
    }
}
