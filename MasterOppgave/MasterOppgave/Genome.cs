using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
   public class Genome
    {
        //     VAUE         INDEX  
        // Self direcion  =   0
        // Stimulation    =   1
        // Hedonism       =   2
        // Achievement    =   3
        // Power          =   4      
        // Security       =   5
        // Conformity     =   6
        // Tradition      =   7
        // Benevolence    =   8
        // Univarsalism   =   9
        // equals: 0,1,2 - 2,3,4 - 5,6,7 - 8,9
        // opposites: 0,5 - 1,5 - 2,67 - 3,8 - 4,9
        private List<double> genomeValues;
        private List<double> genomeNormalised;

        public Genome(List<double> genome)
        {
            genomeValues = genome;
            mutate(EALoop.mutationProb);
            genomeNormalised = new List<double>(10);
            genomeNormalised = normalise(genome);
        }
        public Genome()
        {
            genomeNormalised = new List<double>();
            Random rng = new Random();
            genomeValues = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                genomeValues.Add(0.0);
            }
            int n = 20;

            genomeValues[0] = rng.Next(0,100); // Random value
            genomeValues[1] = (genomeValues[0] * (1 + (System.Convert.ToDouble(rng.Next(0, n)) /100))); // Close to value #0
            genomeValues[5] = (100 - genomeValues[0]) * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Opposite of #0 
            genomeValues[6] = genomeValues[5] * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Close to value #5
            genomeValues[7] = genomeValues[6] * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Close to value #6

            genomeValues[3] = rng.Next(0, 100); // Random value
            genomeValues[4] = genomeValues[3] * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Close to value #3
            genomeValues[8] = (100 - genomeValues[4]) * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Opposite of #4
            genomeValues[9] = genomeValues[8] * (1 + (System.Convert.ToDouble(rng.Next(0, n)) / 100)); // Close to value 8

            genomeValues[2] = ((genomeValues[1]+genomeValues[3]) / 2) * (1 + (System.Convert.ToDouble(rng.Next(0, n/2)) / 100)); // Close to #3 and #1
            genomeNormalised = normalise(genomeValues);
        }

        private List<double> normalise(List<double> genome)
        {
            List<double> temp = new List<double>();
            double sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += genome[i];
            }

            for (int i = 0; i < 10; i++)
            {
                temp.Add(genome[i] / sum);
            }
            return temp;
        }

        public void mutate(double p)
        {
            Random rng = new Random();
            if(rng.NextDouble() <= p)
            {
                double mutateRate = (rng.Next(4) - 2) / 10; // Blir et tall mellom -0.2 og 0.2
                int index = rng.Next(genomeValues.Count);
                genomeValues[index] *= mutateRate;
                genomeNormalised = normalise(genomeValues);
            }
        }

        //-- getters and setters --//
        public List<double> getNormalisedGenome(){return genomeNormalised;}
        public List<double> getValuesGenome(){return genomeValues;}
        
    }
}
