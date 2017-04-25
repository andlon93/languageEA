using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Agent selectListener(Agent agent, SocialNetwork net)
        {
            var genome = agent.getGenome().getValuesGenome();
            double P_extrovert = (genome[3] + genome[4] + (200 - genome[5] - genome[6]) / 400) * C;

            Random rng = new Random();
            List<Tuple<Agent, double>> connections = net.getAgentsConnections(agent);
            if(rng.NextDouble() <= P_extrovert)
            {
                // Extrovert
                Agent a1;
                Agent a2;
                if (connections.Count == 1) { a1 = connections[0].Item1; }
                else { a1 = connections[rng.Next(0, connections.Count - 1)].Item1; }
                if (net.getAgentsConnections(a1).Count == 1) { a2 = net.getAgentsConnections(a1)[0].Item1; }
                else { a2 = net.getAgentsConnections(a1)[rng.Next(0, net.getAgentsConnections(a1).Count - 1)].Item1; }
                if (net.getAgentsConnections(a2).Count == 1) { return net.getAgentsConnections(a2)[0].Item1; }
                else { return net.getAgentsConnections(a2)[rng.Next(0, net.getAgentsConnections(a2).Count - 1)].Item1; }
            }
            // Introvert
            double sum = 0;
            foreach(var friend in connections)
            {
                sum += friend.Item2;
            }
            double random = rng.NextDouble();
            double to = 0;
            foreach(var friend in connections)
            {
                to += friend.Item2 / sum;
                if(random <= to)
                {
                    return friend.Item1;
                }
            }
            return connections[connections.Count-1].Item1;
        }
    }
}
