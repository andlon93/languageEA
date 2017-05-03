using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageEvolution
{
    public class Vocabulary
    {
        //List<Tuple<string, double>> vocabulary;
        Dictionary<string, double> vocabulary;
        public Vocabulary(Dictionary<string, double> vocabulary)
        {
            this.vocabulary = vocabulary;
        }

        public Vocabulary()
        {
            vocabulary = new Dictionary<string, double>();
        }

        public Dictionary<string, double> getVocabulary()
        {
            return vocabulary;
        }

        public void updateVocabulary(string word, double weight)
        {
            if (vocabulary.ContainsKey(word))
            {
                vocabulary[word] = Math.Max(0, vocabulary[word] += weight);
            }
            else
            {
                vocabulary.Add(word, weight);
            }
            //var sortedDict = from entry in vocabulary orderby entry.Value ascending select entry;
        }
    }
}
