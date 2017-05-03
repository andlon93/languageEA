using System;
using System.Collections.Generic;

namespace LanguageEvolution
{
    public class Agent
    {     
        public double fitness;
        // Genome har genotype og phenotype
        public Genome genome;
        public int age;
        public Vocabulary vocabulary;
        public Agent(List<double> genomeValues)
        {
            genome = new Genome(genomeValues);
            vocabulary = new Vocabulary();
            age = 1;
        }

        public Agent()
        {
            genome = new Genome();
            vocabulary = new Vocabulary();
            age = 1;
        }

        public double calculateFitness(Dictionary<Agent, double> connections)
        {
            if(connections == null || connections.Count == 0) { return 0; }

            double wMax = 0.0;
            double numConnections = connections.Count;
            double N = EALoop.populationSize;
            double sumOfAllWeights = 0;
            double NStrongWeights = 0;

            foreach (var i in connections){
                double weight = i.Value;
                sumOfAllWeights += weight;
                if (wMax < weight) { wMax = weight; }
                if(weight > 0.5) { NStrongWeights += 1; }
            }
            //Console.WriteLine("sum of all weights: " + sumOfAllWeights + "\nWmax: " + wMax + "\nNumber of connections: " + numConnections + "\n number strong weights: " + NStrongWeights + "\n N: " + N + "\nAge factor: " + Math.Exp(-0.05 * getAge()));
            if (wMax == 0 || numConnections == 0) { return 0; }
            return (sumOfAllWeights/(wMax*numConnections)) * (NStrongWeights / (N-1)) * Math.Exp(-0.05*getAge());
        }

        public void updatepersonality(Agent partner, bool isSuccess)
        {
            if(isSuccess)
            {
                List<double> values = this.getGenome().getValuesGenome();
                List<double> partnerValues = partner.getGenome().getValuesGenome();
                for (int i = 0; i < values.Count; i++)
                {
                    values[i] += (( (partnerValues[i] - values[i]) / this.getAge() ) * 0.5);
                }
            }
        }

        //-- getters and setters --//
        public void setFitness(double fitness){this.fitness = fitness;}
        public double getFitness(){return fitness;}
        public int getAge() { return age;}
        public void incrementAge() { age++; }
        public Vocabulary getVocabulary(){ return vocabulary; }
        public Genome getGenome() { return genome; }
        //public List<Tuple<Agent, double>> getConnections() { return connections; }
        //public void setConnections(List<Tuple<Agent, double>> connections) { this.connections = connections; }
    }
}
