using System;
using System.Collections.Generic;

namespace LanguageEvolution
{
    public class Dialogue
    {
        public double C = 0.5;

        public Dialogue()
        {

        }

        public Agent selectSpeaker(List<Agent> agents)
        {
            Random rng = new Random();
            return agents[rng.Next(0, agents.Count - 1)];
        }

        public Agent selectListener(Agent agent, SocialNetwork net, List<Agent> population)
        {
            var genome = agent.getGenome().getValuesGenome();
            double P_extrovert = (genome[3] + genome[4] + (200 - genome[5] - genome[6]) / 400) * C;
            Dictionary<Agent, double> connections = net.getAgentsConnections(agent);
            Random r = new Random();
            if (r.NextDouble() <= P_extrovert || net.getAgentsConnections(agent) == null)
            {
                // Extrovert
                Agent listener = population[r.Next(0, population.Count)];
                while (listener == agent)
                {
                    listener = population[r.Next(0, population.Count)];
                }
                return listener;
            }
            // Introvert
            double sum = 0;
            foreach(var friend in connections)
            {
                sum += friend.Value;
            }
            double random = r.NextDouble();
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
            List<Tuple<string, double>> vocabulary = speaker.getVocabulary().getVocabulary();
            
            if (vocabulary.Count == 0)
            {
                return newWord(); 
            }

            double sum = 0;
            for (int i = 0; i < vocabulary.Count; i++)
            {
                sum += vocabulary[i].Item2;
            }
            Random rng = new Random();
            double rnd = rng.NextDouble();
            double prob = 0;
            for (int i = 0; i < vocabulary.Count; i++)
            {
                prob += vocabulary[i].Item2 / sum;
                if (rnd <= prob)
                    return vocabulary[i].Item1;
            }
            return null;
        }
        private string newWord()
        {
            Random rng = new Random();
            int length = rng.Next(1, 10);
            string word = "";
            for (int i = 0; i < length; i++)
            {
                int n = rng.Next(0, 26);
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
