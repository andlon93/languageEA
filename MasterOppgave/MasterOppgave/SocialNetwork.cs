using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOppgave
{
    class SocialNetwork
    {
        private Dictionary<Agent, List<Tuple<Agent, double>>> socialNetwork;

        public SocialNetwork()
        {
            socialNetwork = new Dictionary<Agent, List<Tuple<Agent, double>>>();
        }

        //-- getters and setters --//
        public List<Tuple<Agent, double>> getAgentsConnections(Agent agent)
        {
            foreach(KeyValuePair<Agent, List<Tuple<Agent, double>>> i in socialNetwork)
            {
                if (i.Key.Equals(agent))
                {
                    return i.Value;
                }
            }
            return null;
        }

        public double getConnection(Agent a, Agent b)
        {
            foreach(KeyValuePair<Agent, List<Tuple<Agent, double>>> i in socialNetwork)
            {
                if (i.Key.Equals(a))
                {
                    foreach(Tuple<Agent, double> j in i.Value)
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

        public void setConnection(Agent a, Agent b, double connection)
        {
            foreach (KeyValuePair<Agent, List<Tuple<Agent, double>>> i in socialNetwork)
            {
                if (i.Key.Equals(a))
                {
                    //foreach (Tuple<Agent, double> j in i.Value)
                    for (int j = 0; j < i.Value.Count(); j++)
                    {
                        if (i.Value[j].Item1.Equals(b))
                        {
                            // a og b har connection og den kan oppdateres
                            socialNetwork[a][j] = new Tuple<Agent, double>(b, connection);
                        }
                        else
                        {
                            // a og b har ikke connection og den opprettes
                            socialNetwork[a].Add(new Tuple<Agent, double>(b, connection));
                        }
                    }
                }
                else
                {
                    // Legge inn agent a i nettverk
                    Tuple<Agent, double> tempTuple = new Tuple<Agent, double>(b, connection);
                    List<Tuple<Agent, double>> tempList = new List<Tuple<Agent, double>>();
                    tempList.Add(tempTuple);
                    socialNetwork.Add(a, tempList);
                }
            }
        }
    }
}
