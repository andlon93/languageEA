using System.Collections.Generic;

namespace LanguageEvolution
{
    public class DataCollector
    {
        private List<double> averageFitness;
        private List<double> dialogues;

        public DataCollector()
        {
            averageFitness = new List<double>();
            dialogues = new List<double>();
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
        }
    }
}
