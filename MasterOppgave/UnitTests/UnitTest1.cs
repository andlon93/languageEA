using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using LanguageEvolution;

namespace UnitTests
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void genomeRandomgeneration()
        {
            Genome g = new Genome();
            Assert.AreEqual(10, g.getValuesGenome().Count);
            Assert.AreEqual(10, g.getNormalisedGenome().Count);

            List<double> genome = new List<double>() {10, 45, 65, 97, 31, 56, 97, 14, 85, 53 };
            Genome g2 = new Genome(genome);

            Assert.AreEqual(10, g2.getValuesGenome().Count);
            Assert.AreEqual(10, g2.getNormalisedGenome().Count);
        }

        [TestMethod]
        public void genomeNormalisation()
        {
            Genome g = new Genome();
            double sum = 0;
 
            for (int i = 0; i < 10; i++)
            {
                sum += g.getNormalisedGenome()[i];
                //System.Diagnostics.Debug.WriteLine(g.getNormalisedGenome()[i] + "    " + g.getValuesGenome()[i]);
                //System.Diagnostics.Debug.WriteLine(sum);
            }
            Assert.IsTrue(sum < 1.001);
            Assert.IsTrue(sum > 0.999);
        }

        [TestMethod]
        public void mutationTest()
        {
            Genome g = new Genome();
            double sum = 0;
            foreach(double i in g.getValuesGenome())
            {
                sum += i;
            }
            g.mutate(1.0);
            double sum2 = 0;
            foreach (double i in g.getValuesGenome())
            {
                sum2 += i;
            }
            Assert.AreNotEqual(sum, sum2);
        }

        [TestMethod]
        public void fitnessCalculationTest()
        {
            EALoop ea = new EALoop();

            Tuple<Agent, double> a1 = new Tuple<Agent, double>(new Agent(), 10);
            Tuple<Agent, double> a2 = new Tuple<Agent, double>(new Agent(), 1);
            Tuple<Agent, double> a3 = new Tuple<Agent, double>(new Agent(), 1);
            Tuple<Agent, double> a4 = new Tuple<Agent, double>(new Agent(), 1);
            Tuple<Agent, double> a5 = new Tuple<Agent, double>(new Agent(), 2);

            List<double> genome = new List<double>() { 10, 45, 65, 97, 31, 56, 97, 14, 85, 53 };
            List<Tuple<Agent, double>> connections = new List<Tuple<Agent, double>>() { a1, a2, a3, a4, a5 };
            Agent agent = new Agent(genome, connections);
            while (agent.getAge() < 2) { agent.incrementAge(); }
            Assert.AreEqual(0.1233869206412672, agent.calculateFitness());

        }


    }
}
   
