using System.Collections.Generic;

namespace Demographic
{
    public interface IEngine
    {
        List<Person> Population { get; set; }

        void SetDataFromFileToList(string fileName);

        void SetDataFromFileToDouble();

        void SetValues(int from, int till, int count);

        void Execute();

        event Engine.YearToo YearWent;

    }
}
