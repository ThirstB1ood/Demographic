using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using Demographic.FileOperations;

namespace Demographic
{
    public class Person
    {
        public int Age { get; set; }
        public int BirthDate { get; set; }
        public int DeadDate { get; set; }
        public Alive Status { get; set; }
        public Person Couple { get; set; }

        static readonly int RelationAge = 18;

        private readonly double couple = 0.5;

        private readonly int ageBetweenPersons = 10;



        protected Person(int date, int age)
        {
            Age = age;
            BirthDate = date - age;
            Couple = null;
        }

        private void FindCouple(IEnumerable<Person> population)
        {
            foreach (var candidate in population)
                if (SuitablePartner(candidate) &&
                    candidate.SuitableRelation() &&
                    ((ContinuousUniform)Distribution.Distributions[Event.BirthEngageDisengage]).Sample() <= couple)
                {
                    // Relate them
                    candidate.Couple = this;
                    Couple = candidate;
                    break;
                }
        }

        public bool SuitableRelation()
        {
            return Age >= RelationAge && Couple == null;
        }

        public bool SuitablePartner(Person individual)
        {
            return ((individual is Male && this is Female) ||
                    (individual is Female && this is Male)) &&
                    individual.SuitableRelation() &&
                    this.SuitableRelation() &&
                    Math.Abs(individual.Age - Age) <= ageBetweenPersons;
        }

        public void YearNext(List<Person> population)
        {
            if (Status != Alive.dead)
            {
                Age += 1;

                var gender = this is Female ? Gender.female : Gender.male;

                // Event -> Birth
                if (gender == Gender.female && (this as Female).IsPregnant)
                {
                    // Population.Add((individual as Female).GiveBirth(_distributions, _currentTime));
                    (this as Female).GiveBirth();
                }

                // Event -> Check whether someone starts a relation this year
                if (SuitableRelation())
                    FindCouple(population);

                // Events where having an engaged individual represents a prerequisite
                if (Engaged)
                {
                    // Event -> Check whether some relation ends this year
                    if (EndRelation())
                        Disengage();

                    // Event -> Check whether some couple can have a child now
                    if (this is Female &&
                    (this as Female).SuitablePregnancy())
                        (this as Female).IsPregnant = true;
                }

                //double b = ((ContinuousUniform)_distributions[Event.Die]).Sample();
                //double c = ((ContinuousUniform)_distributions[Event.BirthEngageDisengage]).Sample();
                // double c = filesData.GetDeathChance(Age, gender);

                if (((ContinuousUniform)Distribution.Distributions[Event.Die]).Sample() < FilesData.GetDeathChance(Age, gender))
                {
                    this.Status = Alive.dead;
                }
            }
        }

        public bool IsNotDead()
        {
            bool res;
            if (DeadDate == 0)
                res = true;
            else
                res = false;
            return res;
        }

        public int AgeGet
        {
            get { return Age; }
        }

        public bool Engaged
        {
            get { return Couple != null; }
        }

        public void Disengage()
        {
            Couple.Couple = null;
            Couple = null;
        }

        public bool EndRelation()
        {
            var sample = ((ContinuousUniform)Distribution.Distributions[Event.BirthEngageDisengage]).Sample();

            if (Age >= 18 && Age <= 20 && sample <= 0.7)
                return true;
            if (Age >= 21 && Age <= 28 && sample <= 0.5)
                return true;
            if (Age >= 29 && sample <= 0.2)
                return true;

            return false;
        }
    }
}
