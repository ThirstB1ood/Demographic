using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demographic.WinForms
{
    public class Model
    {
        private IEngine engine;

        public delegate void YearWent(List<Person> population, int currentTime);

        public event YearWent YearTick;

        private int fromAge;

        private int toAge;

        private int count;

        public Model()
        {
            engine = new Engine();
            engine.YearWent += Year;
        }

        public void SetValues(int from, int till, int count)
        {
            fromAge = from;
            toAge = till;
            this.count = count;
        }

        public void Year(List<Person> population, int currentTime)
        {
            YearTick(population, currentTime);
        }

        public  void LoadData(string fileName)
        {
            engine.SetDataFromFileToList(fileName);
        }
        public void DataFromStringToDouble()
        {
            engine.SetValues(fromAge, toAge, count);
            engine.SetDataFromFileToDouble();
           // engine.Create();
            engine.Execute();
        }

        public int[] GetMale()
        {
            List<int> count = new List<int>();
            count.Add(engine.Population.Count(p => p.Age >= 0 && p.Age <= 18 && p is Male && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 18 && p.Age <= 45 && p is Male && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 45 && p.Age <= 65 && p is Male && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 65 && p.Age <= 100 && p is Male && p.Status == Alive.alive));
            return count.ToArray();
        }

        public int[] GetFemale()
        {
            List<int> count = new List<int>();
            count.Add(engine.Population.Count(p => p.Age >= 0 && p.Age <= 18 && p is Female && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 18 && p.Age <= 45 && p is Female && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 45 && p.Age <= 65 && p is Female && p.Status == Alive.alive));
            count.Add(engine.Population.Count(p => p.Age > 65 && p.Age <= 100 && p is Female && p.Status == Alive.alive));
            return count.ToArray();
        }


    }
}