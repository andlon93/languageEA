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
        public void genomeNormalisation()
        {
            Genome g = new Genome();
            double expectedSum = 1;
            double sum = 0;
 
            for (int i = 0; i < 10; i++)
            {
                sum += g.getNormalisedGenome()[i];
                //System.Diagnostics.Debug.WriteLine(g.getNormalisedGenome()[i] + "    " + g.getValuesGenome()[i]);
                //System.Diagnostics.Debug.WriteLine(sum);
            }

            Assert.AreEqual(expectedSum, sum);    

            

        }
    }
}
   
