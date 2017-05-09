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
            System.Console.WriteLine("Unique top rated words: "+ unique.Count);
            uniqueWords.Add(unique.Count);
        }
        public List<int> getUniqueWords()
        {
            return uniqueWords;
        }
        public void setDegree(SocialNetwork n)
        {
            double degreeSum = 0;
            double pop = 0;
            foreach(var a in n.socialNetwork)
            {
                pop++;
                degreeSum += a.Value.Count;
            }
            //System.Console.WriteLine("average degree: " + (degreeSum / pop) + " " + degreeSum + " " + pop);
            degree.Add(degreeSum / pop);
        }
        public List<double> getDegree()
        {
            return degree;
        }
        public void setDialogues(double n)
        {
            dialogues.Add(n);
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
            foreach(double f in averageFitness)
            {
                data += f.ToString() + ",";
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\FitnessData.txt", data);

            data = "";
            foreach(double d in dialogues)
            {
                data += d.ToString() + ",";
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\DialogueData.txt", data);

            data = "";
            foreach (double d in degree)
            {
                data += d.ToString() + ",";
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\DegreeData.txt", data);

            data = "";
            foreach (int d in uniqueWords)
            {
                data += d.ToString() + ",";
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\UniqueWordsData.txt", data);
        }
    }
}
