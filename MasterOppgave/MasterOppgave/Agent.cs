using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOppgave
{
    class Agent
    {
        private double fitness;
        private Genome genome;
        private List<Tuple<Agent, double>> connections;
        private int age;

        public Agent(List<double> genomeValues, List<Tuple<Agent, double>> connections)
        {
            this.genome = new Genome(genomeValues);
            this.connections = connections;
            age = 0;
        }

        public void calculateFitness()
        {
            double fitness = 0;

            double wMax = 1.0;
            double numConnections = connections.Count;
            double N = EALoop.populationSize;
            double sumOfAllWeights = 0;
            double sumOfStrongWeights = 0;
            foreach(Tuple<Agent, double> i in connections){
                sumOfAllWeights += i.Item2;
                if(i.Item2 > 0.1) { sumOfStrongWeights += i.Item2; }
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
