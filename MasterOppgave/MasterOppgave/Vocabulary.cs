using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace LanguageEvolution
{
    public class Vocabulary
    {
        Mutex mut = new Mutex();
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
                vocabulary[word] = vocabulary[word] += weight;
            }
            else
            {
                vocabulary.Add(word, weight);
            }
            if(vocabulary.Count > 10)
            {
                var sortedDict = from entry in vocabulary orderby entry.Value ascending select entry;
                vocabulary.Remove(sortedDict.ElementAt(0).Key);
            }
        }
    }
}
