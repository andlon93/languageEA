using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
    public class DataCollector
    {
        private List<int> uniqueWords;
        private List<double> averageFitness;
        private List<double> dialogues;
        private List<double> degree;

        public DataCollector()
        {
            uniqueWords = new List<int>();
            degree = new List<double>();
            averageFitness = new List<double>();
            dialogues = new List<double>();
        }

        public void addDiscreteGraph(List<Agent> pop, SocialNetwork n, int generation)
        {
            string s = "";
            string sep = ",";
            List<string> addedEdges = new List<string>();
            foreach (var a in pop)
            {
                s += a.getID().ToString() + sep;
                if (n.getAgentsConnections(a) != null)
                {
                    foreach (var b in n.getAgentsConnections(a))
                    {
                        if ( b.Value > 0.0 && !(addedEdges.Contains(a.getID().ToString() + b.Key.getID().ToString())) )
                        {
                            s += b.Key.getID().ToString() + sep;
                            addedEdges.Add(a.getID().ToString() + b.Key.getID().ToString());
                        }
                    }
                    s += "\n";
                }
            }
            string filename = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph"+ generation.ToString()+".txt";
            System.IO.File.WriteAllText(@filename, s);
        }

        public void setUniqueWords(List<Agent> p)
        {
            List<string> unique = new List<string>();
            foreach (Agent a in p)
            {
                if(a.getVocabulary().getVocabulary().Count > 0)
                {
                    var sortedDict = from entry in a.getVocabulary().getVocabulary() orderby entry.Value descending select entry;
                    if (!(unique.Contains(sortedDict.ToArray()[0].Key)))
                    {
                        unique.Add(sortedDict.ToArray()[0].Key);
                    }
                }
                
            }
            uniqueWords.Add(unique.Count);
        }
        public List<int> getUniqueWords()
        {
            return uniqueWords;
        }
        public void setDegree(double n)
        {
            degree.Add(n);
        }
        public List<double> getDegree()
        {
            return degree;
        }
        public void setDialogues(List<Agent> p)
        {
            double succesfull = 0;
            double tot = 0;
            foreach(Agent a in p)
            {
                succesfull += a.getSuccDia();
                tot += a.getTotDialogs();
                a.nulstillDialogs();
            }
            dialogues.Add(succesfull / tot);
        }
        public List<double> getDialogues()
        {
            return dialogues;
        }        
        public void addFitnessData(List<Agent> p)
        {
            double sum = 0;
            foreach(Agent a in p)
            {
                sum += a.getFitness();
            }
            averageFitness.Add(sum / p.Count);
        }
        public List<double> getFitnessdata()
        {
            return averageFitness;
        }

        public void writeToFiles()
        {
            string data = "";
            string separator = "\n";
            foreach(double f in averageFitness)
            {
                data += f.ToString() + separator;
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\FitnessData.txt", data);

            data = "";
            foreach(double d in dialogues)
            {
                data += d.ToString() + separator;
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\DialogueData.txt", data);

            data = "";
            foreach (double d in degree)
            {
                data += d.ToString() + separator;
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\DegreeData.txt", data);

            data = "";
            foreach (int d in uniqueWords)
            {
                data += d.ToString() + separator;
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\UniqueWordsData.txt", data);
        }
    }
}
