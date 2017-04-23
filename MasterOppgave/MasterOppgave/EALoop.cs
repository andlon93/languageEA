using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEvolution
{
    public class EALoop
    {
        public SocialNetwork socialNetwork;
        public static int populationSize = 12;
        public static Double mutationProb = 0.05;

        public EALoop()
        {
            socialNetwork = new SocialNetwork();
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            
        }

        public List<Agent> survivalSelection(int populationSize, List<Agent> population)
        {
            List<Agent> sortedList = population.OrderBy(agent => agent.getFitness()).ToList();
            sortedList.RemoveRange(0, populationSize);
            return sortedList;
        }

        //-- getters and setters --//
        public int getPopulationSize()
        {
            return populationSize;
        }
    }
}
