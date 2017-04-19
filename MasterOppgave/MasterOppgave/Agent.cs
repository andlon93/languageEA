using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEvolution
{
    public class Agent
    {
        // Genome har genotype og phenotype
        //private double fitness;
        //private Genome genome;
        //private List<Tuple<Agent, double>> connections;
        //private int age;
        public double fitness;
        public Genome genome;
        public List<Tuple<Agent, double>> connections;
        public int age;

        public Agent(List<double> genomeValues, List<Tuple<Agent, double>> connections)
        {
            genome = new Genome(genomeValues);
            this.connections = connections;
            age = 0;
        }

        public void calculateFitness()
        {
            double fitness = 0;

            double wMax = 0.0;
            double numConnections = connections.Count;
            double N = EALoop.populationSize;
            double sumOfAllWeights = 0;
            double sumOfStrongWeights = 0;
            foreach(Tuple<Agent, double> i in connections){
                double weight = i.Item2;
                sumOfAllWeights += weight;
                if (wMax < weight) { wMax = weight; }
                if(weight > 0.1) { sumOfStrongWeights += weight; }
            }

            fitness = (sumOfAllWeights/(wMax*numConnections)) * (sumOfStrongWeights/(N-1)) * Math.Exp(-0.05*getAge());
            setFitness(fitness);
        }

        //-- getters and setters --//
        public void setFitness(double fitness){this.fitness = fitness;}
        public double getFitness(){return fitness;}
        public int getAge() { return age;}
        public void incrementAge() { age++; }
    }
}
