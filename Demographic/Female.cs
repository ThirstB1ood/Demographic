using Demographic.FileOperations;
using MathNet.Numerics.Distributions;

namespace Demographic
{
    public sealed class Female : Person
    {
        public bool IsPregnant { get; set; }

        private static readonly int PregnancyAge = 18;

        public event ChildBirth OnBirth;

        public delegate void ChildBirth(Gender gender);

        private readonly double BirthPercent = 0.151;

        private readonly double GenderPercent = 0.55;


        public Female(int date, int age) : base(date, age)
        {
        }

        public bool SuitablePregnancy()
        {
            return Age >= PregnancyAge && ((ContinuousUniform)Distribution.Distributions[Event.GetPregnant]).Sample() < BirthPercent;
        }

        public void GiveBirth()
        { 

            if (Engaged)
            {
                if (((ContinuousUniform)Distribution.Distributions[Event.GetPregnant]).Sample() < GenderPercent)
                {
                    OnBirth(Gender.female);
                }
                else
                {
                    OnBirth(Gender.male);
                }
            }
            IsPregnant = false;
        }
    }
}
