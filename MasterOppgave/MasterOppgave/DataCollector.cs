using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEvolution
{
    public class DataCollector
    {
        private List<double> averageFitness;

        public DataCollector()
        {
            averageFitness = new List<double>();
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
        public void writeFitnessToFile()
        {
            string data = "";
            foreach(double f in averageFitness)
            {
                data += f.ToString() + ",";
            }
            System.IO.File.WriteAllText(@"C:\Users\andrl\Desktop\masterStuff\MasterData\test.txt", data);
        }
    }
}
