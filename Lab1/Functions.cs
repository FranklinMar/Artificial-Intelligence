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
            return 1 / (1 + Math.Exp(-x)) * 10;
        }

        public double CalculateDerivative(double x)
        {
            return Math.Exp(x) / Math.Pow((Math.Exp(x) + 1), 2);
        }
    }
}
