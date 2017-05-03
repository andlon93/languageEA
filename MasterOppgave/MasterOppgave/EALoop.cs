using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
    public class EALoop
    {
        public static int populationSize = 250;
        public static Double mutationProb = 0.05;
        public int k = 5;
        public double eps = 0.2;
        public static int conversationsPerGeneration = 2500;

        static void Main(string[] args)
        {
            EALoop ea = new EALoop();

            Console.WriteLine("STARTING");
            SocialNetwork socialNetwork = new SocialNetwork();
            List<Agent> population = new List<Agent>();
            Dialogue dialogue = new Dialogue();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Agent());
            }
            Console.WriteLine("size of population: " + population.Count);
            int generations = 0;
            while (generations < 2)
            {
                ea.performDialogues(socialNetwork, population);
                Console.WriteLine("nodes in the socialnetwork: " + socialNetwork.socialNetwork.Count);

                ea.breed(population, socialNetwork);

                ea.fitnessOfPopulation(population, socialNetwork);
                population = population.OrderBy(agent => agent.getFitness()).ToList();
                population.Reverse();
                Console.WriteLine("Size of population: " + population.Count);
                foreach (Agent a in population)
                {
                    Console.WriteLine("Fitness: " + a.getFitness());
                }

                population = ea.survivalSelection(populationSize, population, socialNetwork);
                Console.WriteLine("\nSize of population: " + population.Count);



                generations++;
            }
            Console.Write("");
        }

        public void breed(List<Agent> pop, SocialNetwork socialNetwork)
        {
            List<Agent> children = new List<Agent>();
            for(int i = 0; i < pop.Count; i++)
            {
                Agent parent1 = tournamentSelection(pop);
                Agent parent2 = tournamentSelection(pop);
                Agent child = crossover(parent1, parent2);
                children.Add(child);
                performDialogue(parent1, child, socialNetwork);
                performDialogue(parent2, child, socialNetwork);
                performDialogue(parent1, child, socialNetwork);
                performDialogue(parent2, child, socialNetwork);
            }
            pop.AddRange(children);
            
        }

        private void fitnessOfPopulation(List<Agent> population, SocialNetwork socialNetwork)
        {
            foreach (Agent a in population)
            {
                double fitness = a.calculateFitness(socialNetwork.getAgentsConnections(a));
                Console.WriteLine("Vocabulary size: "+ a.getVocabulary().getVocabulary().Count + "\nFitness: "+ fitness+"\n");
                a.setFitness(fitness);
            }
            
        }

        public void performDialogues(SocialNetwork socialNetwork, List<Agent> population)
        {
            Dialogue dialogue = new Dialogue();
            for (int i = 0; i < conversationsPerGeneration; i++)
            {
                Agent speaker = dialogue.selectSpeaker(population);
                Agent listener = dialogue.selectListener(speaker, socialNetwork, population);
                if (listener == null)
                {
                    Random r = new Random();
                    while (listener == null || listener == speaker)
                    {
                        listener = population[r.Next(0, population.Count)];
                    }
                }
                performDialogue(speaker, listener, socialNetwork);
                
            }
        }

        private void performDialogue(Agent speaker, Agent listener, SocialNetwork socialNetwork)
        {
            Dialogue dialogue = new Dialogue();
            string utterance = dialogue.utterWord(speaker);
            bool isSuccess = false;
            if (!(listener.getVocabulary().getVocabulary() == null) && listener.getVocabulary().getVocabulary().ContainsKey(utterance))
            {
                isSuccess = true;
                speaker.getVocabulary().updateVocabulary(utterance, 1);
                listener.getVocabulary().updateVocabulary(utterance, 1);
            }
            else
            {
                speaker.getVocabulary().updateVocabulary(utterance, -0.2);
                listener.getVocabulary().updateVocabulary(utterance, -0.2);
            }

            speaker.updatepersonality(listener, isSuccess);
            listener.updatepersonality(speaker, isSuccess);

            socialNetwork.setConnection(speaker, listener, getWeight(speaker, isSuccess));
            socialNetwork.setConnection(listener, speaker, getWeight(listener, isSuccess));
            System.Threading.Thread.Sleep(1);
        }

        public double getWeight(Agent a, bool isSuccess)
        {
            if (isSuccess)
            {
                return 1;
                //return connection + a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] + 1;
            }
            return -0.5;
        //    double weight = connection + a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] - 1;
        //    if (weight < 0) { return 0; }
        //    return weight;
        }

        public List<Agent> survivalSelection(int populationSize, List<Agent> population, SocialNetwork socialNetwork)
        {
            List<Agent> deadAgents = new List<Agent>();
            List<Agent> sortedList = population.OrderBy(agent => agent.getFitness()).ToList();
            Console.WriteLine("\n\npopSize: " + populationSize + "population: " + population.Count);
            sortedList.Reverse();

            deadAgents = sortedList.GetRange(populationSize, populationSize);
            foreach(var a in socialNetwork.socialNetwork.ToList())
            {
                if (deadAgents.Contains(a.Key))
                {
                    socialNetwork.socialNetwork.Remove(a.Key);
                }
                else
                {
                    foreach(var b in socialNetwork.socialNetwork[a.Key].ToList())
                    {
                        if (deadAgents.Contains(b.Key))
                        {
                            socialNetwork.socialNetwork[a.Key].Remove(b.Key);
                        }
                    }
                }
            }

            return sortedList.GetRange(0, populationSize);
        }

        public Agent tournamentSelection(List<Agent> allAgents)
        {
            List<Agent> pool = new List<Agent>(k);
            Random rng = new Random();
            int numberOfAgents = allAgents.Count;
            while (pool.Count < k)
            {
                int i = rng.Next(0, numberOfAgents);
                if (!pool.Contains(allAgents[i]))
                {
                    pool.Add(allAgents[i]);
                }
            }
            pool.Sort((x, y) => x.fitness.CompareTo(y.fitness));
            pool.Reverse();

            List<double> pList = new List<double>();
            double sum = 0;
            for(int i = 1; i < pool.Count+1; i++)
            {
                double prob = ((1 - eps) * Math.Pow(eps, i - 1));
                pList.Add(prob);
                sum += prob;
            }
            double rnd = rng.NextDouble();
            double p = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                p += pList[i];
                if (rnd <= p)
                    return pool[i];
            }

            Console.WriteLine("SOMETHING WENT WRONG");
            return null;
        }

        public Agent crossover(Agent a, Agent b)
        {
            List<double> childGenome = new List<double>();
            List<double> aGenome = a.getGenome().getValuesGenome();
            List<double> bGenome = b.getGenome().getValuesGenome();
            childGenome.Add(aGenome[0]);
            childGenome.Add(aGenome[1]);
            childGenome.Add(bGenome[2]);
            childGenome.Add(bGenome[3]);
            childGenome.Add(bGenome[4]);
            childGenome.Add(aGenome[5]);
            childGenome.Add(aGenome[6]);
            childGenome.Add(aGenome[7]);
            childGenome.Add(bGenome[8]);
            childGenome.Add(bGenome[9]);

            return new Agent(childGenome);
        }
        
        //-- getters and setters --//
        public int getPopulationSize()
        {
            return populationSize;
        }
    }
}
