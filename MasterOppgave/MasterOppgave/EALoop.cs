using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
    public class EALoop
    {
        public SocialNetwork socialNetwork;
        public static List<Agent> population;
        public static int populationSize = 12;
        public static Double mutationProb = 0.05;
        public int k = 5;
        public double eps = 0.2;
        public static int conversationsPerGeneration = 1;

        public EALoop()
        {
            socialNetwork = new SocialNetwork();
            population = new List<Agent>();
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("STARTING");
            SocialNetwork socialNetwork = new SocialNetwork();
            Dialogue dialogue = new Dialogue();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Agent());
            }
            Console.WriteLine("size of population: " + population.Count);

            

            Console.Write("");
        }

        public void performDialogues(SocialNetwork socialnetwork, List<Agent> population)
        {
            Dialogue dialogue = new Dialogue();
            for (int i = 0; i < conversationsPerGeneration; i++)
            {
                Agent speaker = dialogue.selectSpeaker(population);
                Agent Listener = dialogue.selectListener(speaker, socialNetwork);
                string utterance = dialogue.utterWord(speaker);
                bool isSuccess = false;
                foreach (var word in Listener.getVocabulary().getVocabulary())
                {
                    if (word.Item1 == utterance)
                    {
                        isSuccess = true;
                        break;
                    }
                }
                speaker.updatepersonality(Listener, isSuccess);
                Listener.updatepersonality(speaker, isSuccess);
                socialNetwork.setConnection(speaker, Listener, getWeight(speaker, isSuccess, socialNetwork.getConnection(speaker, Listener)));
                socialNetwork.setConnection(Listener, speaker, getWeight(Listener, isSuccess, socialNetwork.getConnection(Listener, speaker)));
            }
        }

        public double getWeight(Agent a, bool isSuccess, double connection)
        {
            if (isSuccess)
                return connection + a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] + 1;
            return connection + a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] - 1;
        }

        public List<Agent> survivalSelection(int populationSize, List<Agent> population)
        {
            List<Agent> sortedList = population.OrderBy(agent => agent.getFitness()).ToList();
            sortedList.RemoveRange(0, populationSize);
            return sortedList;
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

            Agent b = new Agent(); b.fitness = 100;
            return b;
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
        public List<Agent> getPopulation()
        {
            return population;
        }
    }
}
