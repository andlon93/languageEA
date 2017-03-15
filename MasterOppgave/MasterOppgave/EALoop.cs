using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOppgave
{
    class EALoop
    {
        public SocialNetwork socialNetwork;
        public static int populationSize = 100;

        public EALoop()
        {
            socialNetwork = new SocialNetwork();
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            
        }

        private List<Agent> survivalSelection(int populationSize, List<Agent> population)
        {
            List<Agent> sortedList = population.OrderBy(agent => agent.getFitness()).ToList();
            sortedList.RemoveRange(populationSize, sortedList.Count-populationSize);
            return sortedList;
        }

        //-- getters and setters --//
        public int getPopulationSize()
        {
            return populationSize;
        }
    }
}
