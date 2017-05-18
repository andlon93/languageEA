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
            g.mutate(0.0);
            double sum3 = 0;
            foreach (double i in g.getValuesGenome())
            {
                sum3 += i;
            }
            Assert.AreEqual(sum2, sum3);
        }

        [TestMethod]
        public void fitnessCalculationTest()
        {
            EALoop ea = new EALoop();
            EALoop.populationSize = 5;
            SocialNetwork socialNetwork = new SocialNetwork();
            Agent agent = new Agent(0);
            socialNetwork.setConnection(agent, new Agent(0), 10);
            socialNetwork.setConnection(agent, new Agent(1), 2);
            socialNetwork.setConnection(agent, new Agent(2), 1);
            socialNetwork.setConnection(agent, new Agent(3), 1);
            socialNetwork.setConnection(agent, new Agent(4), 1);
            while (agent.getAge() < 2) { agent.incrementAge(); }

            //Assert.AreEqual(0.27, agent.calculateFitness(socialNetwork.getAgentsConnections(agent), 5));

        }

        [TestMethod]
        public void survivalSelectionTest()
        {
            //EALoop ea = new EALoop();
            //Agent a1 = new Agent(); a1.setFitness(10);
            //Agent a2 = new Agent(); a2.setFitness(11);
            //Agent a3 = new Agent(); a3.setFitness(2);
            //Agent a4 = new Agent(); a4.setFitness(12);
            //List<Agent> pop = new List<Agent>() { a1, a2, a3, a4 };
            //List<Agent> survived = new List<Agent>();
            //survived = ea.survivalSelection(2, pop, new SocialNetwork());

            //System.Diagnostics.Debug.WriteLine(survived[0].getFitness());
            //System.Diagnostics.Debug.WriteLine(survived[1].getFitness());
            //Assert.AreEqual(2, survived.Count);
            //Assert.AreEqual(12, survived[0].getFitness());
            //Assert.AreEqual(11, survived[1].getFitness());
        }

        [TestMethod]
        public void SetsocialNetworkTest()
        {
            SocialNetwork net = new SocialNetwork();
            Agent a = new Agent(1);
            Agent b = new Agent(2);
            Agent a1 = new Agent(3);
            Agent b2 = new Agent(4);

            net.setConnection(a, b, 2); // legge til ny agent.
            Assert.AreEqual(1, net.socialNetwork.Count);

            net.setConnection(a1, b2, 3); // ny agent #2.
            Assert.AreEqual(2, net.socialNetwork.Count);

            net.setConnection(a, a1, 4); // ny connection for eksisterende agent.
            Assert.AreEqual(2, net.socialNetwork[a].Count);

            net.setConnection(a, b, 8);
            Assert.AreEqual(10, net.socialNetwork[a][b]);
        }

        [TestMethod]
        public void getAgentsConnectionsSocialNetworkTest()
        {
            SocialNetwork net = new SocialNetwork();
            Agent a = new Agent(0);
            Agent b = new Agent(1);
            Agent a1 = new Agent(2);
            Agent b2 = new Agent(3);
            net.setConnection(a, b, 2);
            net.setConnection(a1, b2, 3);
            net.setConnection(a, a1, 4);

            Assert.AreEqual(net.socialNetwork[a], net.getAgentsConnections(a));
            Assert.AreEqual(2, net.getAgentsConnections(a).Count);
            net.setConnection(a, b, 3);
            Assert.AreEqual(2, net.getAgentsConnections(a).Count);
        }

        [TestMethod]
        public void getConnectionSocialNetworkTest()
        {
            SocialNetwork net = new SocialNetwork();
            Agent a = new Agent(0);
            Agent b = new Agent(1);
            Agent a1 = new Agent(2);
            Agent b2 = new Agent(3);
            net.setConnection(a, b, 2);
            net.setConnection(a1, b2, 3);
            net.setConnection(a, a1, 4);

            Assert.AreEqual(2, net.getConnection(a, b));
            Assert.AreEqual(3, net.getConnection(a1, b2));
            Assert.AreEqual(4, net.getConnection(a, a1));
        }

        [TestMethod]
        public void vocabularyTest()
        {
            Agent a = new Agent(0);

            a.getVocabulary().updateVocabulary("s", 1);
            Assert.AreEqual(1, a.getVocabulary().getVocabulary().Count);
            Assert.AreEqual(true, a.getVocabulary().getVocabulary().ContainsKey("s"));

            a.getVocabulary().updateVocabulary("t", 2);
            a.getVocabulary().updateVocabulary("u", 0);
            Assert.AreEqual(3, a.getVocabulary().getVocabulary().Count);
            Assert.AreEqual(true, a.getVocabulary().getVocabulary().ContainsKey("u"));
            Assert.AreEqual(0, a.getVocabulary().getVocabulary()["u"]);

            a.getVocabulary().updateVocabulary("u", 3);            
            Assert.AreEqual(true, a.getVocabulary().getVocabulary().ContainsKey("u"));
            Assert.AreEqual(3, a.getVocabulary().getVocabulary()["u"]);
            Assert.AreEqual(true, a.getVocabulary().getVocabulary().ContainsKey("s"));
            Assert.AreEqual(3, a.getVocabulary().getVocabulary().Count);
        }

        [TestMethod]
        public void testSelectSpeaker()
        {
            Dialogue d = new Dialogue();
            Agent a = new Agent(0);                     
            Agent b = new Agent(1);
            Agent c = new Agent(2);
            List<Agent> agent = new List<Agent>() { a };
            List<Agent> agents = new List<Agent>() { a, b, c };

            Assert.AreEqual(a, d.selectSpeaker(agent));

            if (agents.Contains(d.selectSpeaker(agents)))
                Assert.AreEqual(1, 1);
            else
                Assert.AreNotEqual(1, 1);
        }

        [TestMethod]
        public void testSelectListener()
        {
            //List<double> genomeExtro = new List<double>() { 0, 0, 0, 100, 100, 0, 0, 0, 0, 0 };
            Agent a = new Agent(0);
            Agent b = new Agent(1);
            Agent c = new Agent(2);
            List<Agent> pop = new List<Agent>() { c };
            SocialNetwork net = new SocialNetwork();
            net.setConnection(a, b, 1);
            //a.setConnections(net.getAgentsConnections(a));

            Dialogue dia = new Dialogue();
            dia.C = 0.0;
            Agent listenerIntrovert = dia.selectListener(a, net, pop);
            dia.C = 1.0;
            Agent listenerExtrovert = dia.selectListener(a, net, pop);

            Assert.AreEqual(b, listenerIntrovert);
            Assert.AreEqual(c, listenerExtrovert);
        }

        [TestMethod]
        public void testUpdatePersonality()
        {
            List<double> genome = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            List<double> genome2 = new List<double>() { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            Agent a = new Agent(genome, 0);
            Agent b = new Agent(genome2, 1);

            a.updatepersonality(b, false);
            Assert.AreEqual(1, a.getGenome().getValuesGenome()[0]);

            a.updatepersonality(b, true);
            b.updatepersonality(a, true);

            Assert.AreEqual(2, b.getGenome().getValuesGenome()[0]);
            Assert.AreEqual(1, a.getGenome().getValuesGenome()[0]);
        }

        [TestMethod]
        public void testTournamentSelection()
        {
            int k = 5;
            double eps = 0.2;
            EALoop ea = new EALoop();
            Agent a = new Agent(1); a.fitness = 5;
            Agent b = new Agent(2); b.fitness = 4;
            Agent c = new Agent(3); c.fitness = 3;
            Agent d = new Agent(4); d.fitness = 2;
            Agent e = new Agent(5); e.fitness = 1;
            List<Agent> allAgents = new List<Agent>() { a, e, b, d, c};

            ea.k = k;
            ea.eps = eps;

            double a_count = 0; double b_count = 0; double c_count = 0; double d_count = 0; double e_count = 0; double error = 0;
            double N = 100;
            
            for (int i = 0; i < N; i++)
            {
                double fitness = ea.tournamentSelection(allAgents).getFitness();
                if (fitness == 5)
                    a_count++;
                else if (fitness == 4)
                    b_count++;
                else if (fitness == 3)
                    c_count++;
                else if (fitness == 2)
                    d_count++;
                else if (fitness == 1)
                    e_count++;
                else if (fitness == 100)
                    error++;
            }
            System.Diagnostics.Debug.WriteLine("\n\n"+N+ " tournaments run with p as "+ (1-eps));
            System.Diagnostics.Debug.WriteLine("A: "+ (a_count/N)*100);
            System.Diagnostics.Debug.WriteLine("B: "+ (b_count / N) * 100);
            System.Diagnostics.Debug.WriteLine("C: "+ (c_count / N) * 100);
            System.Diagnostics.Debug.WriteLine("D: " + (d_count / N) * 100);
            System.Diagnostics.Debug.WriteLine("E: " + (e_count / N) * 100);
            System.Diagnostics.Debug.WriteLine("errors: "+ (error / N) * 100);
            Assert.AreEqual(0.0, error);
        }

        [TestMethod]
        public void testCrossover()
        {
            EALoop ea = new EALoop();
            Agent parent1 = new Agent(new List<double>() { 0, 1, 10, 10, 10, 5, 6, 7, 10, 10 }, 0);
            Agent parent2 = new Agent(new List<double>() { 10, 10, 2, 3, 4, 10, 10, 10, 8, 9 }, 0);
            Agent child = ea.crossover(parent1, parent2);
            Agent child2 = ea.crossover(parent2, parent1);
            for(int i = 0; i < 10; i++)
            {
                Assert.AreEqual(10, child2.getGenome().getValuesGenome()[i]);
                Assert.AreEqual(i, child.getGenome().getValuesGenome()[i]);
            }
        }

        [TestMethod]
        public void testutterWord()
        {
            Agent speaker = new Agent(0);
            Dialogue d = new Dialogue();
            string word = d.utterWord(speaker);
            speaker.getVocabulary().updateVocabulary(word, 1);
            Assert.AreEqual(word, d.utterWord(speaker));
            Assert.AreNotEqual(null, d.utterWord(speaker));
        }

        [TestMethod]
        public void testGetWeight()
        {
            Agent a = new Agent(new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 0);
            EALoop e = new EALoop();
            Assert.AreEqual(1, e.getWeight(a, true));
            Assert.AreEqual(0, e.getWeight(a, false));
        }
    }
}
   
