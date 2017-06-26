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
        private int z = 0;
        public double succesfullDialogs = 0;
        public double totDialogs = 0;
        public int ID;
        public Agent(List<double> genomeValues, int ID)
        {
            genome = new Genome(genomeValues);
            vocabulary = new Vocabulary();
            age = 1;
            this.ID = ID;
        }

        public Agent(int ID)
        {
            genome = new Genome();
            vocabulary = new Vocabulary();
            age = 1;
            this.ID = ID;
        }

        public double calculateFitness(Dictionary<Agent, double> connections)
        {
            if(connections == null || connections.Count == 0) { return 0; }

            double wMax = 0.0;
            double numConnections = connections.Count;
            double numStrongConnections = 0;
            double N = EALoop.populationSize;
            double sumOfAllWeights = 0;
            double NStrongWeights = 0;
            foreach (var i in connections)
            {
                double weight = i.Value;
                sumOfAllWeights += weight;
                if (wMax < weight) { wMax = weight; }
                if(weight > 0.0) { NStrongWeights++; numStrongConnections++; }
            }
            //Console.WriteLine("sum of all weights: " + sumOfAllWeights + "\nWmax: " + wMax + "\nNumber of connections: " + numConnections + "\n number strong weights: " + NStrongWeights + "\n N: " + N + "\nAge factor: " + Math.Exp(-0.05 * getAge()));
            if (numConnections == 0) { return 0; }
            double weightFitness = Math.Exp( (EALoop.alpha * sumOfAllWeights)) -1;
            double degreeFitness = Math.Exp(EALoop.beta * (NStrongWeights/numStrongConnections)) - 1;
            double ageFitness = Math.Exp(EALoop.gamma * getAge()) ;
            return Math.Min(weightFitness * degreeFitness * ageFitness, 1);
        }

        public void updatepersonality(Agent partner, bool isSuccess)
        {
            if(isSuccess)
            {
                List<double> values = getGenome().getValuesGenome();
                List<double> partnerValues = partner.getGenome().getValuesGenome();
                List<double> newValues = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < values.Count; i++)
                {
                    newValues[i] += (( (partnerValues[i] - values[i]) / getAge() ) * 0.5);
                    newValues[i] = Math.Max(newValues[i], 100);
                }
                double newLR = newValues[0] + newValues[8] + newValues[9] -
                               newValues[4] - newValues[1] - newValues[7];
                double oldLR = values[0] + values[8] + values[9] -
                               values[4] - values[1] - values[7];
                if(newLR > oldLR)
                {
                    //values[0] = newValues[0];
                    //values[8] = newValues[8];
                    //values[9] = newValues[9];
                    //values[4] = newValues[4];
                    //values[1] = newValues[1];
                    //values[7] = newValues[7];
                    if (newValues[0] > values[0]) { values[0] = newValues[0]; }
                    if (newValues[5] > values[5]) { values[5] = newValues[5]; }
                    if (newValues[3] > values[3]) { values[3] = newValues[3]; }
                    if (newValues[8] > values[8]) { values[8] = newValues[8]; }
                    if (newValues[9] > values[9]) { values[9] = newValues[9]; }
                    if (newValues[6] < values[6]) { values[6] = newValues[6]; }
                    if (newValues[4] < values[4]) { values[4] = newValues[4]; }
                    if (newValues[1] < values[1]) { values[1] = newValues[1]; }
                    if (newValues[7] < values[7]) { values[7] = newValues[7]; }
                }
                //if (newValues[0] > values[0]) { values[0] = newValues[0]; }
                //if (newValues[8] > values[8]) { values[8] = newValues[8]; }
                //if (newValues[9] > values[9]) { values[9] = newValues[9]; }
                //if (newValues[4] < values[4]) { values[4] = newValues[4]; }
                //if (newValues[1] < values[1]) { values[1] = newValues[1]; }
                //if (newValues[7] < values[7]) { values[7] = newValues[7]; }
            }
            
        }

        //-- getters and setters --//
        public void updateDialogs(bool isSuccess)
        {
            if (isSuccess) { succesfullDialogs++; }
            totDialogs++;
        }
        public double getSuccDia() { return succesfullDialogs; }
        public double getTotDialogs() { return totDialogs; }
        public void nulstillDialogs() { succesfullDialogs = 0; totDialogs = 0; }
        public void incrementZ() { z++; }
        public int getZ() { return z; }
        public void setFitness(double fitness){this.fitness = fitness;}
        public double getFitness(){return fitness;}
        public int getAge() { return age;}
        public void incrementAge() { age++; }
        public Vocabulary getVocabulary(){ return vocabulary; }
        public Genome getGenome() { return genome; }
        public int getID() { return ID; }
        //public List<Tuple<Agent, double>> getConnections() { return connections; }
        //public void setConnections(List<Tuple<Agent, double>> connections) { this.connections = connections; }
    }
}
