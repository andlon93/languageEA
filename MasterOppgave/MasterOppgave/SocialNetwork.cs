using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOppgave
{
    class SocialNetwork
    {
        private Dictionary<Agent, List<Tuple<Agent, float>>> socialNetwork;

        public SocialNetwork()
        {
            socialNetwork = new Dictionary<Agent, List<Tuple<Agent, float>>>();
        }

        //-- getters and setters --//
        public List<Tuple<Agent, float>> getAgentsConnections(Agent agent)
        {
            foreach(KeyValuePair<Agent, List<Tuple<Agent, float>>> i in socialNetwork)
            {
                if (i.Key.Equals(agent))
                {
                    return i.Value;
                }
            }
            return null;
        }

        public float getConnection(Agent a, Agent b)
        {
            foreach(KeyValuePair<Agent, List<Tuple<Agent, float>>> i in socialNetwork)
            {
                if (i.Key.Equals(a))
                {
                    foreach(Tuple<Agent, float> j in i.Value)
                    {
                        if (j.Item1.Equals(b))
                        {
                            return j.Item2;
                        }
                    }
                }
            }
            return 0;
        }

        public void setConnection(Agent a, Agent b, float connection)
        {
            foreach (KeyValuePair<Agent, List<Tuple<Agent, float>>> i in socialNetwork)
            {
                if (i.Key.Equals(a))
                {
                    //foreach (Tuple<Agent, float> j in i.Value)
                    for (int j = 0; j < i.Value.Count(); j++)
                    {
                        if (i.Value[j].Item1.Equals(b))
                        {
                            // a og b har connection og den kan oppdateres
                            socialNetwork[a][j] = new Tuple<Agent, float>(b, connection);
                        }
                        else
                        {
                            // a og b har ikke connection og den opprettes
                            socialNetwork[a].Add(new Tuple<Agent, float>(b, connection));
                        }
                    }
                }
                else
                {
                    // Legge inn agent a i nettverk
                    Tuple<Agent, float> tempTuple = new Tuple<Agent, float>(b, connection);
                    List<Tuple<Agent, float>> tempList = new List<Tuple<Agent, float>>();
                    tempList.Add(tempTuple);
                    socialNetwork.Add(a, tempList);
                }
            }
        }
    }
}
