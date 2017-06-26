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
        public int AgentIdCounter = 0;
        public static int populationSize = 1000;
        public static int numThreads = populationSize;
        public static Double mutationProb = 0.02;
        public double k = 0.9;
        public double n = 0.5;
        public double eps = 0.2;
        public static int d = 1;
        public static int Totalgenerations = 100;
        public static readonly Random random = new Random();
        public static readonly object syncLock = new object();
        public double succcessfullDialogues = 0;
        public static double alpha = 0.2; public static double beta = 0.12; public static double gamma = -0.05;
        public static double speakToParentsConstant = 3;
        static void Main(string[] args)
        {
            EALoop ea = new EALoop();

            Console.WriteLine("STARTING");
            SocialNetwork socialNetwork = new SocialNetwork();
            DataCollector data = new DataCollector();
            List<Agent> population = new List<Agent>();
            Dialogue dialogue = new Dialogue();
            int generations = 1;

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Agent(ea.AgentIdCounter));
                ea.AgentIdCounter++;
            }
            List<Agent> speakerPool = new List<Agent>();
            foreach (Agent a in population)
            {
                if (socialNetwork.getAgentsConnections(a) != null)
                {
                    speakerPool.Add(a);
                }
            }
            Console.WriteLine("Speaker pool: " + speakerPool.Count);
            ea.dialogueThreads(socialNetwork, speakerPool, population);

            ea.fitnessOfPopulation(population, socialNetwork);
            population = ea.survivalSelection(population, socialNetwork);
            data.addDiscreteGraph(population, socialNetwork, generations);
            data.addFitnessData(population);
            data.setDialogues(population);
            ea.addFitnessDegreeData(population, socialNetwork, data);
            data.setUniqueWords(population);

            ea.updateAges(population);

            
            while (generations < Totalgenerations)
            {
                generations++;
                Console.WriteLine("\nGeneration number: " + generations);
                //Console.WriteLine("Nodes in the socialnetwork: " + socialNetwork.socialNetwork.Count);

                //--   MAKE CHILDREN   --//
                //Console.WriteLine("Size of population before children is made: " + population.Count);
                population.AddRange(ea.makeChildren(population, socialNetwork));
                //Console.WriteLine("Size of population after children was made: " + population.Count);

                //--    PERFORM DIALOGUES   --//
                //Console.WriteLine("socialnetwork count  "+socialNetwork.socialNetwork.Count);
                speakerPool = new List<Agent>();
                for (int q = 0; q < population.Count; q++)
                {
                    //Console.WriteLine("index: " + q);
                    if(population[q] == null) { population.RemoveAt(q); }
                    else if(socialNetwork.getAgentsConnections(population[q]) != null && socialNetwork.getAgentsConnections(population[q]).Count > 0)
                    {
                        speakerPool.Add(population[q]);
                    }
                }
                Console.WriteLine("Speaker pool: " + speakerPool.Count);
                ea.dialogueThreads(socialNetwork, speakerPool, population);
                
                //ea.succcessfullDialogues = 0;
                //Console.WriteLine("Dialogues performed");

                //--    CALCULATE FITNESS   --//
                ea.fitnessOfPopulation(population, socialNetwork);


                //--    SURVIVAL SELECTION   --//
                population = ea.survivalSelection(population, socialNetwork);

                //--   DATA GATHERING   --//
                data.setDialogues(population);
                data.setUniqueWords(population);
                data.addFitnessData(population);
                if (generations % 5 == 0) { data.addDiscreteGraph(population, socialNetwork, generations); }
                ea.addFitnessDegreeData(population, socialNetwork, data);

                ea.updateAges(population);
                
                //Console.WriteLine("Size of population after survival selection: " + population.Count);
            }
            Console.WriteLine("fitness data: " + data.getFitnessdata().Count + "\ndialogue data: " + data.getDialogues().Count + "\nUnique words data: " + data.getUniqueWords().Count);
            data.writeToFiles();
            data.addDiscreteGraph(population, socialNetwork, generations);
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
            //Console.WriteLine("making " + (populationSize - n )+ " children.");
            for(int i = population.Count; i < populationSize; i++)
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
            //Console.WriteLine("Number of children: " + Children.Count);
            return Children;
        }

        public List<Agent> breed(List<Agent> pop, SocialNetwork socialNetwork)
        {
            List<Agent> children = new List<Agent>();
            for (int i = 0; i < 1; i++)
            {
                Agent parent1 = tournamentSelection(pop);
                Agent parent2 = tournamentSelection(pop);
                Agent child = crossover(parent1, parent2);
                if (RandomDouble() <= (child.getGenome().getNormalisedGenome()[5]*speakToParentsConstant))
                {
                    breedMut.WaitOne(); // MUTEX start
                    performDialogue(parent1, child, socialNetwork);
                    performDialogue(parent2, child, socialNetwork);
                    performDialogue(parent1, child, socialNetwork);
                    performDialogue(parent2, child, socialNetwork);
                    performDialogue(parent1, child, socialNetwork);
                    performDialogue(parent2, child, socialNetwork);
                    performDialogue(parent1, child, socialNetwork);
                    performDialogue(parent2, child, socialNetwork);
                    breedMut.ReleaseMutex(); // MUTEX end
                }
                children.Add(child);
            }
            return children;
        }

        private void addFitnessDegreeData(List<Agent> population, SocialNetwork socialNetwork, DataCollector d)
        {
            double C = 0.5;
            double allConnections = 0;
            double AvgWeight = 0;
            double learnRateSum = 0;
            double totalVocLen = 0;
            double speakToParentsGene = 0;
            double extrovertProb = 0;
            foreach (Agent a in population)
            {
                double agentsAvgWeight = 0;
                double agentsConnections = 0;
                speakToParentsGene += a.getGenome().getNormalisedGenome()[5]* speakToParentsConstant;
                List<double> genome = a.getGenome().getValuesGenome();
                extrovertProb += ((genome[3] + (100 - genome[6]) / 200) * C)/100;
                totalVocLen += a.getVocabulary().getVocabulary().Count;
                List<double> normGenome = a.getGenome().getNormalisedGenome();
                learnRateSum += (normGenome[0] + normGenome[8] + normGenome[9] - normGenome[4] - normGenome[1] - normGenome[7]);
                if (socialNetwork.getAgentsConnections(a) != null && socialNetwork.getAgentsConnections(a).Count > 0)
                {
                    foreach (var i in socialNetwork.getAgentsConnections(a))
                    {
                        if (i.Value > 0.0)
                        {
                            agentsConnections++;
                            agentsAvgWeight += i.Value;
                        }
                    }
                    if (agentsConnections > 0)
                    {
                        AvgWeight += (agentsAvgWeight / agentsConnections);
                        allConnections += agentsConnections;
                    }
                }
            }
            Console.WriteLine("Speak to parents prob: " + (speakToParentsGene / population.Count));
            Console.WriteLine("Extrovert probability: " + (extrovertProb / population.Count));
            Console.WriteLine("average vocabulary length: " + (totalVocLen / population.Count));
            Console.WriteLine("Fittest agent: " + population[0].getFitness());
            Console.WriteLine("Average connections: " + (allConnections / population.Count));
            Console.WriteLine("Agents average weight: " + (AvgWeight / population.Count));
            Console.WriteLine("Average learn rate: " + (learnRateSum / population.Count) + "\n\n");
            d.addExtrovertData((extrovertProb / population.Count));
            d.addSpeakToParentsGenome((speakToParentsGene / population.Count));
            d.addAvgVocLen((totalVocLen / population.Count));
            d.addFittest(population[0].getFitness());
            d.setDegree(allConnections / population.Count);
            d.addLearnRate((learnRateSum / population.Count));
        }

        private void fitnessOfPopulation(List<Agent> population, SocialNetwork socialNetwork)
        {

            foreach (Agent a in population)
            {
                if(a != null && socialNetwork.getAgentsConnections(a) != null)
                {
                    double fitness = a.calculateFitness(socialNetwork.getAgentsConnections(a));
                    a.setFitness(fitness);
                }
                else
                {
                    a.setFitness(0);
                }
                //Console.WriteLine("Vocabulary size: "+ a.getVocabulary().getVocabulary().Count + "\nFitness: "+ fitness+"\n");
                
            }
            population = population.OrderBy(agent => agent.getFitness()).ToList();
            population.Reverse();
        }

        public void dialogueThreads(SocialNetwork socialNetwork, List<Agent> speakerPool, List<Agent> population)
        {
            List<Thread> ts = new List<Thread>();
            for (int i = 0; i < population.Count*d; i++)
            {
                Thread t = new Thread(new ThreadStart(() => performDialogues(socialNetwork, speakerPool, population)));
                t.Name = String.Format("t{0}", i + 1);
                t.Start();
                ts.Add(t);
            }
            foreach (var t in ts)
            {
                t.Join();
            }
        }

        public void performDialogues(SocialNetwork socialNetwork, List<Agent> speakerPool, List<Agent> population)
        {
            diaMut.WaitOne();
            Dialogue dialogue = new Dialogue();
            Agent speaker; Agent listener;
            if (speakerPool.Count == 0)
            {
                //Console.WriteLine("forced extrovert");
                speaker = population[RandomInt(0, population.Count)];
                listener = population[RandomInt(0, population.Count)];
            }
            else
            {
                //Console.WriteLine("Introvert");
                speaker = dialogue.selectSpeaker(speakerPool);
                listener = dialogue.selectListener(speaker, socialNetwork, population);
            }

            if (listener == null)
            {
                while (listener == null || listener == speaker)
                {
                    Console.WriteLine("new listener");
                    listener = population[RandomInt(0, population.Count)];
                }
            }
            performDialogue(speaker, listener, socialNetwork);
            diaMut.ReleaseMutex();
        }

        private void performDialogue(Agent speaker, Agent listener, SocialNetwork socialNetwork)
        {
            Dialogue dialogue = new Dialogue();
            string utterance = dialogue.utterWord(speaker);
            bool isSuccess = false;

            if (!(listener.getVocabulary().getVocabulary() == null) && listener.getVocabulary().getVocabulary().ContainsKey(utterance))
            {
                isSuccess = true;
                speaker.updateDialogs(true);
                double i = speaker.getGenome().getNormalisedGenome()[0] + speaker.getGenome().getNormalisedGenome()[8] + speaker.getGenome().getNormalisedGenome()[9] - speaker.getGenome().getNormalisedGenome()[4] - speaker.getGenome().getNormalisedGenome()[1] - speaker.getGenome().getNormalisedGenome()[7];
                speaker.getVocabulary().updateVocabulary(utterance, Math.Max(0, i));
                double i2 = listener.getGenome().getNormalisedGenome()[0] + listener.getGenome().getNormalisedGenome()[8] + listener.getGenome().getNormalisedGenome()[9] - listener.getGenome().getNormalisedGenome()[4] - listener.getGenome().getNormalisedGenome()[1] - listener.getGenome().getNormalisedGenome()[7];
                listener.getVocabulary().updateVocabulary(utterance, Math.Max(0,i2));
                succcessfullDialogues++;
            }
            else
            {
                
                double i = speaker.getGenome().getNormalisedGenome()[0] + speaker.getGenome().getNormalisedGenome()[8] + speaker.getGenome().getNormalisedGenome()[9] - speaker.getGenome().getNormalisedGenome()[4] - speaker.getGenome().getNormalisedGenome()[1] - speaker.getGenome().getNormalisedGenome()[7];
                double i2 = listener.getGenome().getNormalisedGenome()[0] + listener.getGenome().getNormalisedGenome()[8] + listener.getGenome().getNormalisedGenome()[9] - listener.getGenome().getNormalisedGenome()[4] - listener.getGenome().getNormalisedGenome()[1] - listener.getGenome().getNormalisedGenome()[7];
                speaker.updateDialogs(false);
                speaker.getVocabulary().updateVocabulary(utterance, Math.Min(0, -1*i));
                listener.getVocabulary().updateVocabulary(utterance, Math.Max(0, i2));
            }

            speaker.updatepersonality(listener, isSuccess);
            listener.updatepersonality(speaker, isSuccess);

            socialNetwork.setConnection(speaker, listener, getWeight(speaker, isSuccess));
            socialNetwork.setConnection(listener, speaker, getWeight(listener, isSuccess));
        }

        public double getWeight(Agent a, bool isSuccess)
        {
            if (isSuccess)
            {
                return 1;
                //return a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] + 1;
            }
            return -0.5;
            //double weight = a.getGenome().getNormalisedGenome()[0] + a.getGenome().getNormalisedGenome()[8] + a.getGenome().getNormalisedGenome()[9] - a.getGenome().getNormalisedGenome()[4] - a.getGenome().getNormalisedGenome()[1] - a.getGenome().getNormalisedGenome()[7] - 0.5;
            //if (weight < 0) { return 0; }
            //return weight;
        }

        public List<Agent> survivalSelection(List<Agent> population, SocialNetwork socialNetwork)
        {

            List<Agent> deadAgents = new List<Agent>();
            List<Agent> survivingAgents = new List<Agent>();
            while(survivingAgents.Count < populationSize*k)
            {
                int i = RandomInt(0, population.Count);
                survivingAgents.Add(population[i]);
                population.RemoveAt(i);
            }
            
            List<Agent> sortedList = survivingAgents.OrderBy(agent => agent.getFitness()).ToList();
            //Console.WriteLine("\n\npopSize: " + populationSize + "population: " + sortedList.Count);
            sortedList.Reverse();
            //Console.WriteLine("fitnesses: " + sortedList[0].getFitness() + "  " + sortedList[sortedList.Count - 1].getFitness());
            removeDeadAgents(socialNetwork, population, populationSize);

            List<Agent> survivors = new List<Agent>();
            int counter = 0;
            while(counter < sortedList.Count * n)
            {
                survivors.Add(sortedList[counter]);
                counter++;
            }
            //Console.WriteLine("Number of survivors: "+ survivors.Count);
            return survivors;
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
            int poolsize = 20;
            List<Agent> pool = new List<Agent>(poolsize);
            int numberOfAgents = allAgents.Count;
            while (pool.Count < poolsize)
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
            Agent child = new Agent(childGenome, AgentIdCounter);
            AgentIdCounter++;
            return child;
            
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
