using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEvolution

{
    public class Connection
    {
        private Agent speaker;
        private Agent listener;
        private double speakerWeight;
        private double listenerWeight;

        public Connection(Agent speaker, Agent listener, double speakerWeight, double listenerWeight)
        {
            this.speaker = speaker;
            this.listener = listener;
            this.speakerWeight = speakerWeight;
            this.listenerWeight = listenerWeight;
        }

        public Agent getSpeaker() { return speaker; }
        public Agent getListener() { return listener; }
        public double getSpWeight() { return speakerWeight; }
        public double getLiWeight() { return listenerWeight; }
    }
}
