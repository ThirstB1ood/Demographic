using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace Demographic
{
    public sealed class Distribution
    {
        private Distribution()
        {
            Distributions = new Dictionary<Event, IDistribution>
                                 {
                                     { Event.BirthEngageDisengage, new ContinuousUniform() },
                                     { Event.GetPregnant, new ContinuousUniform() },
                                     { Event.Die, new ContinuousUniform() },
                                 };
            min = ((ContinuousUniform)Distribution.Distributions[Event.GetPregnant]).Maximum;
        }
        static Distribution()
        {
            Instance = new Distribution();
        }

        public static Dictionary<Event, IDistribution> Distributions { get; private set; }

        public static Distribution Instance { get; }

        private double min;
    }
}
