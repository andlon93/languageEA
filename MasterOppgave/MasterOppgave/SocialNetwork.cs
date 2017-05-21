using System.Collections.Generic;

namespace LanguageEvolution
{
    public class SocialNetwork
    {
        public Dictionary<Agent, Dictionary<Agent, double>> socialNetwork;

        public SocialNetwork()
        {
            socialNetwork = new Dictionary<Agent, Dictionary<Agent, double>> ();
        }
        public SocialNetwork(Dictionary<Agent, Dictionary<Agent, double>> network)
        {
            socialNetwork = network;
        }

        //-- getters and setters --//
        public Dictionary<Agent, double> getAgentsConnections(Agent a)
        {
            if (socialNetwork.ContainsKey(a))
                return socialNetwork[a];
            return null;
        }

        public double getConnection(Agent a, Agent b)
        {
            if(socialNetwork.ContainsKey(a) && socialNetwork[a].ContainsKey(b))
                return socialNetwork[a][b];
            return 0;
        }

        public void setConnection(Agent a, Agent b, double connection)
        {
            
            if(socialNetwork.ContainsKey(a))
            {
                if (socialNetwork[a].ContainsKey(b))
                {
                    socialNetwork[a][b] += connection;    
                }
                else
                {
                    socialNetwork[a].Add(b, connection);
                }
            }
            else
            {
                socialNetwork.Add(a, new Dictionary<Agent, double>());
                socialNetwork[a].Add(b, connection);
                   
            }
        }
    }
}
