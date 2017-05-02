using System;
using System.Collections.Generic;

namespace LanguageEvolution
{
    public class Vocabulary
    {
        List<Tuple<string, double>> vocabulary;

        public Vocabulary(List<Tuple<string, double>> vocabulary)
        {
            this.vocabulary = vocabulary;
        }

        public Vocabulary()
        {
            vocabulary = new List<Tuple<string, double>>();
        }


        public List<Tuple<string, double>> getVocabulary()
        {
            return vocabulary;
        }

        public void updateVocabulary(string word, double weight)
        {
            Tuple<string, double> t = new Tuple<string, double>(word, weight);
            bool exists = false;
            foreach (var i in vocabulary.ToArray())
            {
                int counter = 0;
                if (i.Item1.Equals(word))
                {
                    vocabulary[counter] = new Tuple<string, double>(word, weight);
                    exists = true;
                }
                counter++;
            }   
            if (!exists)
            {
                vocabulary.Add(t);
            }

            vocabulary.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        }
    }
}
