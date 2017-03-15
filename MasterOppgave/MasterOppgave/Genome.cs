using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOppgave
{
    class Genome
    {
        private List<double> genomeValues;
        private List<double> genomeNormalised;

        public Genome(List<double> genome)
        {
            genomeValues = genome;
            genomeNormalised = normalise(genomeValues);
        }
        public Genome()
        {
            Random rng = new Random();
            genomeValues = new List<double>();
            for(int i=0; i<10; i++)
            {
                genomeValues.Add(rng.Next(0, 100));
            }

            genomeNormalised = normalise(genomeValues);
        }

        public List<double> normalise(List<double> genome)
        {
            List<double> normalised = genome.Select(i => i * (100.0/genome.Max()) ).ToList();
            return normalised;
            // return genome.Select(i => i * (100.0/genome.Max()) ).ToList();
        }

        public void mutate(double p)
        {
            Random rng = new Random();
            if(rng.NextDouble() <= p)
            {
                double mutateRate = (rng.Next(4) - 2) / 10; // Blir et tall mellom -0.2 og 0.2
                int index = rng.Next(genomeValues.Count);
                genomeValues[index] *= mutateRate;
            }
        }

        //-- getters and setters --//
        public List<double> getNormalisedGenome(){return genomeNormalised;}
        public List<double> getValuesGenome(){return genomeValues;}
        
    }
}
