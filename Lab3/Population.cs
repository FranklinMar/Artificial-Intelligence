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

        public Population() { }
        public Population(List<Genome<T>> genomes) => Individuals = genomes;

        public Population<T> Selection(Func<Genome<T>, double> FitnessFunction, int TopNumber)
        {
            if (TopNumber < 0 || TopNumber > Individuals.Count)
            {
                throw new ArgumentException("Limit number out of bounds");
            }
            Dictionary<Genome<T>, double> Dict = new();
            Individuals.ForEach(Individual => Dict.Add(Individual, FitnessFunction.Invoke(Individual)));
            var SortedList = from entry in Dict orderby entry.Value descending select entry.Key;
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
