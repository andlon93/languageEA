using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LanguageEvolution
{
    public class EALoop
    {
        Mutex breedMut = new Mutex();
        Mutex diaMut = new Mutex();
        public static int populationSize = 1000;
        public static int numThreads = populationSize;
        public static Double mutationProb = 0.01;
        public int k = 20;
        public double eps = 0.2;
        public static int d = 1;
        public static int Totalgenerations = 100;
        public static readonly Random random = new Random();
        public static readonly object syncLock = new object();
        public double succcessfullDialogues = 0;

        static void Main(string[] args)
        {
            EALoop ea = new EALoop();

            Console.WriteLine("STARTING");
            SocialNetwork socialNetwork = new SocialNetwork();
            DataCollector data = new DataCollector();
            List<Agent> population = new List<Agent>();
            Dialogue dialogue = new Dialogue();
            

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Agent());
            }
            //Console.WriteLine("size of population: " + population.Count);
            ea.dialogueThreads(socialNetwork, population);
            data.setDialogues(ea.succcessfullDialogues / (numThreads * d));
            data.setDegree(socialNetwork);
            data.setUniqueWords(population);
            ea.fitnessOfPopulation(population, socialNetwork);
            data.addFitnessData(population);
            ea.updateAges(population);
            int generations = 1;
            while (generations < Totalgenerations)
            {
                Console.WriteLine("\nGeneration number: " + generations);
                //Console.WriteLine("Nodes in the socialnetwork: " + socialNetwork.socialNetwork.Count);

                //--   MAKE CHILDREN   --//
                population.AddRange(ea.makeChildren(population, socialNetwork));
                //Console.WriteLine("Size of population after children was made: " + population.Count);

                //--    PERFORM DIALOGUES   --//
                ea.dialogueThreads(socialNetwork, population);
                data.setDialogues(ea.succcessfullDialogues / (population.Count * d));
                data.setDegree(socialNetwork);
                data.setUniqueWords(population);
                ea.succcessfullDialogues = 0;
                //Console.WriteLine("Dialogues performed");

                //--    CALCULATE FITNESS   --//
                ea.fitnessOfPopulation(population, socialNetwork);
                //Console.WriteLine("Fitness calculated");
                
                //--    SURVIVAL SELECTION   --//
                population = ea.survivalSelection(populationSize, population, socialNetwork);
                ea.updateAges(population);
                data.addFitnessData(population);
                //Console.WriteLine("Size of population after survival selection: " + population.Count);

                generations++;
            }
            Console.WriteLine("fitness data: " + data.getFitnessdata().Count + "\ndialogue data: " + data.getDialogues().Count + "\nUnique words data: " + data.getUniqueWords().Count);
            data.writeToFiles();
            Console.Write("Press any button to end");
        }

        public void updateAges(List<Agent> p)
        {
            foreach(var a in p)
            {
                a.incrementAge();
            }
        }

        public List<Agent> makeChildren(List<Agent> population, SocialNetwork socialNetwork)
        {
            EALoop ea = new EALoop();
            List<Agent> Children = new List<Agent>();
            List<Thread> ts = new List<Thread>();
            
            for(int i = 0; i < numThreads; i++)
            {
                Thread t = new Thread(new ThreadStart(() => Children.AddRange(ea.breed(population, socialNetwork))));
                t.Name = String.Format("t{0}", i + 1);
                t.Start();
                ts.Add(t);
            }
            foreach(var t in ts)
            {
                t.Join();
            }
            return Children;
        }

        public List<Agent> breed(List<Agent> pop, SocialNetwork socialNetwork)
        {
            List<Agent> children = new List<Agent>();
            for (int i = 0; i < pop.Count/numThreads; i++)
            {
                Agent parent1 = tournamentSelection(pop);
                Agent parent2 = tournamentSelection(pop);
                Agent child = crossover(parent1, parent2);
                breedMut.WaitOne(); // MUTEX start
                performDialogue(parent1, child, socialNetwork);
                performDialogue(parent2, child, socialNetwork);
                performDialogue(parent1, child, socialNetwork);
                performDialogue(parent2, child, socialNetwork);
                breedMut.ReleaseMutex(); // MUTEX end
                children.Add(child);
            }
            return children;
        }

        private void fitnessOfPopulation(List<Agent> population, SocialNetwork socialNetwork)
        {
            foreach (Agent a in population)
            {
                double fitness = a.calculateFitness(socialNetwork.getAgentsConnections(a));
                // Console.WriteLine("Vocabulary size: "+ a.getVocabulary().getVocabulary().Count + "\nFitness: "+ fitness+"\n");
                a.setFitness(fitness);
            }
            population = population.OrderBy(agent => agent.getFitness()).ToList();
            population.Reverse();
        }

        public void dialogueThreads(SocialNetwork socialNetwork, List<Agent> population)
        {
            List<Thread> ts = new List<Thread>();
            for (int i = 0; i < population.Count*d; i++)
            {
                Thread t = new Thread(new ThreadStart(() => performDialogues(socialNetwork, population)));
                t.Name = String.Format("t{0}", i + 1);
                t.Start();
                ts.Add(t);
            }
            foreach (var t in ts)
            {
                t.Join();
            }
        }

        public void performDialogues(SocialNetwork socialNetwork, List<Agent> population)
        {
            Dialogue dialogue = new Dialogue();
            Agent speaker = dialogue.selectSpeaker(population);
            Agent listener = dialogue.selectListener(speaker, socialNetwork, population);
            if (listener == null)
            {
                while (listener == null || listener == speaker)
                {
                    listener = population[RandomInt(0, population.Count)];
                }
            }
            performDialogue(speaker, listener, socialNetwork);
        }

        private void performDialogue(Agent speaker, Agent listener, SocialNetwork socialNetwork)
        {
            Dialogue dialogue = new Dialogue();
            string utterance = dialogue.utterWord(speaker);
            bool isSuccess = false;

            diaMut.WaitOne(); 
            if (!(listener.getVocabulary().getVocabulary() == null) && listener.getVocabulary().getVocabulary().ContainsKey(utterance))
            {
                isSuccess = true;
                speaker.getVocabulary().updateVocabulary(utterance, 1);
                listener.getVocabulary().updateVocabulary(utterance, 1);
                succcessfullDialogues++;
            }
            else
            {
                speaker.getVocabulary().updateVocabulary(utterance, -0.5);
                listener.getVocabulary().updateVocabulary(utterance, -0.5);
            }

            speaker.updatepersonality(listener, isSuccess);
            listener.updatepersonality(speaker, isSuccess);

            socialNetwork.setConnection(speaker, listener, getWeight(speaker, isSuccess));
            socialNetwork.setConnection(listener, speaker, getWeight(listener, isSuccess));
            diaMut.ReleaseMutex();
        }

        public double getWeight(Agent a, bool isSuccess)
        {
            if (isSuccess)
            {
                //return 1;
                return a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] + 1;
            }
            //return -0.5;
            double weight = a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] - 0.5;
            if (weight < 0) { return 0; }
            return weight;
        }

        public List<Agent> survivalSelection(int populationSize, List<Agent> population, SocialNetwork socialNetwork)
        {
            List<Agent> deadAgents = new List<Agent>();
            List<Agent> sortedList = population.OrderBy(agent => agent.getFitness()).ToList();
            //Console.WriteLine("\n\npopSize: " + populationSize + "population: " + population.Count);
            sortedList.Reverse();
            Console.WriteLine("fitnesses: " + sortedList[0].getFitness() + "  " + sortedList[sortedList.Count - 1].getFitness());
            deadAgents = sortedList.GetRange(populationSize, populationSize);
            removeDeadAgents(socialNetwork, deadAgents, populationSize);

            return sortedList.GetRange(0, populationSize);
        }

        private void removeDeadAgents(SocialNetwork socialNetwork, List<Agent> deadAgents, int populationSize)
        {
            
            foreach (var a in socialNetwork.socialNetwork.ToList())
            {
                if (deadAgents.Contains(a.Key))
                {
                    socialNetwork.socialNetwork.Remove(a.Key);
                }
                else
                {
                    foreach (var b in socialNetwork.socialNetwork[a.Key].ToList())
                    {
                        if (deadAgents.Contains(b.Key))
                        {
                            socialNetwork.socialNetwork[a.Key].Remove(b.Key);
                        }
                    }
                }
            }
        }

        public Agent tournamentSelection(List<Agent> allAgents)
        {
            List<Agent> pool = new List<Agent>(k);
            int numberOfAgents = allAgents.Count;
            while (pool.Count < k)
            {
                int i = RandomInt(0, numberOfAgents);
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
            double rnd = RandomDouble();
            double p = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                p += pList[i];
                if (rnd <= p)
                    return pool[i];
            }

            return pool[0];
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

        public static int RandomInt(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public static double RandomDouble()
        {
            lock (syncLock)
            { // synchronize
                return random.NextDouble();
            }
        }
        //-- getters and setters --//
        public int getPopulationSize()
        {
            return populationSize;
        }
    }
}
