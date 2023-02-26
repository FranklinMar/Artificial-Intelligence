using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    /*public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }*/

    public abstract class Singleton<T> where T : class, new()
    {
        private static readonly T instance = new ();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton()
        {
        }
        protected Singleton()
        {
        }
        public static T Instance
        {
            get
            {
                return instance;
            }
        }
    }

    interface IFunction
    {
        public double Calculate(double x);
        public double CalculateDerivative(double x);
    }

    class Sigmoid : Singleton<Sigmoid>, IFunction
    {
        public double Calculate(double x)
        {
            return 1.0 / (1 + Math.Exp(-x));
        }

        public double CalculateDerivative(double x)
        {
            return Math.Exp(-x) / Math.Pow((Math.Exp(-x) + 1), 2);
        }
    }
    
    class And : Singleton<And>, IFunction
    {
        public double Calculate(double x)
        {
            if (x < 1.5)
            {
                return 0;
            } 
            else
            {
                return 1;
            }
        }

        public double CalculateDerivative(double x)
        {
            return 0;
        }
    }

    class Or : Singleton<Or>, IFunction
    {
        public double Calculate(double x)
        {
            if (x < 0.5)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public double CalculateDerivative(double x)
        {
            return 0;
        }
    }

    class Not : Singleton<Not>, IFunction
    {
        public double Calculate(double x)
        {
            if (x < -1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public double CalculateDerivative(double x)
        {
            return 0;
        }
    }

    class Xor : Singleton<Xor>, IFunction
    {
        public double Calculate(double x)
        {
            if (x < 0.5)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public double CalculateDerivative(double x)
        {
            return 0;
        }
    }
}
