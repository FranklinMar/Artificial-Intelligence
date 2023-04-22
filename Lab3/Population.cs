using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;

namespace Lab3
{
    public class Population <T>
    {
        public List<Genome<T>> Individuals { get; private set; } = new();

        public int Count
        {
            get
            {
                return Individuals.Count;
            }
        }

        public Population() { }

        public void Add(Genome<T> genome) => Individuals.Add(genome);

        public void Remove(Genome<T> genome) => Individuals.Remove(genome);

        public void RemoveAt(int Index) => Individuals.RemoveAt(Index);

        public Population(List<Genome<T>> genomes) => Individuals = genomes;

        public Genome<T> this[int i]
        {
            get
            {
                return Individuals[i];
            }
            set
            {
                Individuals[i] = value;
            }
        }

        public Population<T> Selection(Func<Genome<T>, double> FitnessFunction, int LimitNumber)
        {
            if (LimitNumber < 0 || LimitNumber > Individuals.Count)
            {
                //throw new ArgumentException("Limit number out of bounds");
                throw new ArgumentOutOfRangeException(nameof(LimitNumber));
            }
            Dictionary<Genome<T>, double> Dict = new();
            Individuals.ForEach(Individual => Dict.Add(Individual, FitnessFunction.Invoke(Individual)));
            var SortedList = (from entry in Dict orderby entry.Value descending select entry.Key).Take(LimitNumber);
            return new(new (SortedList));
        }

        public Population<T> NewGeneration(int IndividualsNumber)
        {
            SecureRandom Generator = new();
            List<Genome<T>> Generation = new();
            Genome<T> First;
            Genome<T> Second;

            for (int i = 0; i < IndividualsNumber; i++)
            {
                First = Individuals[Generator.Next(Individuals.Count)];
                Second = Individuals[Generator.Next(Individuals.Count)];
                Generation.Add(First.Cross(Second, Generator.Next(First.BitCount)));
                if (Generator.NextDouble() < 0.1)
                {
                    Generation[i].InverseGene(Generator.Next(First.BitCount));
                }
            }
            return new(Generation);
        }
        //public 
    }
}
