using System.Collections.Generic;
using System;
using System.Linq;

namespace Demographic.WinForms
{
    class Controller
    {

        public delegate void YearWent(List<Person> population, int currentTime);

        public event YearWent YearTick;

        private readonly Model model;
        public Controller()
        {
            model = new Model();
            model.YearTick += Year;
        }

        public void Year(List<Person> population, int currentTime)
        {
            YearTick(population, currentTime);
        }

        public void SetValues(string from, string till, string count)
        {
            model.SetValues(Convert.ToInt32(from), Convert.ToInt32(till), Convert.ToInt32(count));
        }

        public void LoadData(string fileName)
        {
            model.LoadData(fileName);
        }

        public void DataToDouble()
        {
            model.DataFromStringToDouble();
        }

        public int[] GetMale()
        {
            return model.GetMale();
        }

        public int[] GetFemale()
        {
            return model.GetFemale();
        }

    }
}
