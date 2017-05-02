﻿using System;
using System.Collections.Generic;

namespace LanguageEvolution
{
    public class SocialNetwork
    {
        public Dictionary<Agent, List<Tuple<Agent, double>>> socialNetwork;

        public SocialNetwork()
        {
            socialNetwork = new Dictionary<Agent, List<Tuple<Agent, double>>>();
        }
        public SocialNetwork(Dictionary<Agent, List<Tuple<Agent, double>>> network)
        {
            socialNetwork = network;
        }

        //-- getters and setters --//
        public List<Tuple<Agent, double>> getAgentsConnections(Agent agent)
        {
            foreach(var i in socialNetwork)
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
            if(socialNetwork.ContainsKey(a))
            {
                foreach (var i in socialNetwork[a].ToArray())
                {
                    int counter = 0;
                    if (i.Item1.Equals(b))
                    {
                        // Update existing connection
                        //connection += socialNetwork[a][counter].Item2;
                        socialNetwork[a][counter] = new Tuple<Agent, double>(b, connection);
                    }
                    counter++;
                }
                // Add new connection
                socialNetwork[a].Add(new Tuple<Agent, double>(b, connection));
            }
            else
            {
                // Add new agent to the network
                Tuple<Agent, double> tempTuple = new Tuple<Agent, double>(b, connection);
                List<Tuple<Agent, double>> tempList = new List<Tuple<Agent, double>>();
                tempList.Add(tempTuple);
                socialNetwork.Add(a, tempList);
            }
        }
    }
}
