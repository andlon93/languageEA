using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
    public class Dialogue
    {
        public double C = 0.5;
        //private int maxExtroConv = 7;

        public Dialogue()
        {

        }

        public Agent selectSpeaker(List<Agent> pop)
        {
            double sum = 0;
            foreach (Agent a in pop)
            {
                sum += a.getFitness();
            }
            double rnd = EALoop.RandomDouble();
            double n = 0;
            foreach (Agent a in pop)
            {
                n += a.getFitness() / sum;
                if (rnd <= n)
                {
                    return a;
                }
            }
            return pop[EALoop.RandomInt(0,pop.Count)];
        }

        public Agent selectListener(Agent agent, SocialNetwork net, List<Agent> population)
        {
            var genome = agent.getGenome().getValuesGenome();
            double P_extrovert = ((genome[3] + (100 - genome[6]) / 200) * C)/100;
            Dictionary<Agent, double> connections = net.getAgentsConnections(agent);
            //if ((EALoop.RandomDouble() <= P_extrovert && agent.getZ() < maxExtroConv) || net.getAgentsConnections(agent) == null)
            if (EALoop.RandomDouble() <= P_extrovert) 
            {
                // Extrovert
                //System.Console.WriteLine("EXTROVERT");
                Agent listener = population[EALoop.RandomInt(0, population.Count)];
                while (listener == agent)
                {
                    listener = population[EALoop.RandomInt(0, population.Count)];
                }
                agent.incrementZ();
                return listener;
            }
            // Introvert
            double sum = 0;
            foreach(var friend in connections)
            {
                sum += friend.Value;
            }
            double random = EALoop.RandomDouble();
            double to = 0;
            foreach(var friend in connections)
            {
                to += friend.Value / sum;
                if(random <= to)
                {
                    return friend.Key;
                }
            }
            return null;
        }

        public string utterWord(Agent speaker)
        {
            Dictionary<string, double> vocabulary = speaker.getVocabulary().getVocabulary();
            if (EALoop.RandomInt(0, 100) <= 40)
            {
                return newWord();
            }
            if (vocabulary.Count == 0)
            {
                return newWord(); 
            }

            double sum = 0;
            var sortedDict = from entry in vocabulary orderby entry.Value descending select entry;
            foreach (var i in sortedDict)
            {
                sum += i.Value;
            }
            double rnd = EALoop.RandomDouble();
            double prob = 0;
            foreach (var i in sortedDict)
            {
                prob += i.Value / sum;
                if(rnd <= prob)
                {
                    return i.Key;
                }
            }
            return vocabulary.ElementAt(EALoop.RandomInt(0, vocabulary.Count)).Key;
        }
        private string newWord()
        {
            int length = EALoop.RandomInt(1,5);
            string word = "";
            for (int i = 0; i < length; i++)
            {
                int n = EALoop.RandomInt(0, 26);
                word += newLetter(n);
            }
            return word;
        }
        private string newLetter(int n)
        {
            if (n == 0) { return "a"; }
            else if (n == 1) { return "b"; }
            else if (n == 2) { return "b"; }
            else if (n == 3) { return "c"; }
            else if (n == 4) { return "d"; }
            else if (n == 5) { return "e"; }
            else if (n == 6) { return "f"; }
            else if (n == 7) { return "g"; }
            else if (n == 8) { return "h"; }
            else if (n == 9) { return "i"; }
            else if (n == 10) { return "j"; }
            else if (n == 11) { return "k"; }
            else if (n == 12) { return "l"; }
            else if (n == 13) { return "m"; }
            else if (n == 14) { return "n"; }
            else if (n == 15) { return "o"; }
            else if (n == 16) { return "p"; }
            else if (n == 17) { return "q"; }
            else if (n == 18) { return "r"; }
            else if (n == 19) { return "s"; }
            else if (n == 20) { return "t"; }
            else if (n == 21) { return "u"; }
            else if (n == 22) { return "v"; }
            else if (n == 23) { return "w"; }
            else if (n == 24) { return "x"; }
            else if (n == 25) { return "y"; }
            else { return "z"; }

        }
    }
}
