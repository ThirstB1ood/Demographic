using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using Demographic.FileOperations;

namespace Demographic
{
    public class Engine : IEngine
    {
        private FileReader readerDeath;
        private FileReader readerInitial;

        public delegate void Year(List<Person> population);

        public event Year YearTick;

        public delegate void YearToo(List<Person> population, int currentTime);

        public event YearToo YearWent;
        public List<Person> Population { get; set; }

        private  int time;

        private int currentTime ;

        private int count;

        public Engine()
        {
            Population = new List<Person>();
            _ = Distribution.Instance;
        }

        public void SetValues(int from, int till, int count)
        {
            this.count = count;
            time = till;
            currentTime = from;
        }

        private void CreateInitialData(int countPopulation)
        {
            Population.AddRange(CreateInitialPersons(countPopulation / 2, Gender.male));

            Population.AddRange(CreateInitialPersons(countPopulation / 2, Gender.female));
            foreach (Person person in Population)
                Subscribe(person);
        }

        private List<Person> CreateInitialPersons(int countFemalePerson, Gender gender)
        {
            List<Person> persons = new List<Person>(countFemalePerson);
            for (int i = 0; i < FilesData.InitialData.Count - 1; i++)
            {
                int percent = (int)Math.Round(Convert.ToDouble(countFemalePerson) / 1000 * FilesData.InitialData[i][1]);
                for (int j = 0; j < percent; j++)
                {
                    int age = 0;
                    if (i != FilesData.InitialData.Count - 1)
                        age = CreateAge(Convert.ToInt32(FilesData.InitialData[i][0]), Convert.ToInt32(FilesData.InitialData[i + 1][0]), false);
                    else if (i != FilesData.InitialData.Count - 1)
                        age = CreateAge(Convert.ToInt32(FilesData.InitialData[i][0]), 0, true);
                    switch (gender)
                    {
                        case Gender.male:
                            persons.Add(new Male(time, age));
                            break;
                        case Gender.female:
                            persons.Add(new Female(time, age));
                            break;
                    }
                }
            }
            return persons;
        }

        private int CreateAge(int from, int till, bool end)
        {
            ContinuousUniform random;
            if (end)
            {
                random = new ContinuousUniform(from, 100);
            }
            else
            {
                random = new ContinuousUniform(from, till);
            }
            return (int)random.Sample();
        }

        public void SetDataFromFileToList(string fileName)
        {
            if (fileName.Contains("Age"))
            {
                readerInitial = new FileReader(fileName);
            }
            else if (fileName.Contains("Rule"))
            {
                readerDeath = new FileReader(fileName);
            }
        }

        public void SetDataFromFileToDouble()
        {
            if(readerInitial == null || readerDeath == null)
            {
                throw new Exception("Ошибка выбора");
            }
            FilesData.GetInstance(readerInitial.DivideIrises(), readerDeath.DivideIrises());
        }

        private void Subscribe(Person person)
        {
            YearTick += person.YearNext;
            if (person is Female female)
            {
                female.OnBirth += BirthPerson;
            }
        }

        private void UnSubcribe()
        {
            foreach (Person person in Population)
            {
                if (person.Status == Alive.dead)
                {
                    YearTick -= person.YearNext;
                    if (person is Female female)
                    {
                        female.OnBirth -= BirthPerson;
                    }
                }
            }
        }

        private void BirthPerson(Gender gender)
        {
            if (gender == Gender.female)
            {
                Population.Add(new Female(time, 0));
            }
            else
            {
                Population.Add(new Male(time, 0));
            }
            Subscribe(Population[Population.Count - 1]);
        }



        public void Execute()
        {
            CreateInitialData(count);
            while (currentTime < time)
            {
                currentTime++;
                YearTick(Population);
                YearWent(Population, currentTime);
                UnSubcribe();
            }
        }
    }
}
