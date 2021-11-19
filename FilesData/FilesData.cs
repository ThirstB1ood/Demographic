using System;
using System.Collections.Generic;
using System.Linq;

namespace Demographic.FileOperations
{
    public class FilesData
    {
        public List<List<double>> InitialData { get; }
        public List<List<double>> DeathChance { get; set; }


        public FilesData(List<List<double>> pathInitialData, List<List<double>> pathDeathData)
        {
            InitialData = pathInitialData;
            DeathChance = pathDeathData;
        }

        public double GetInitialChance(int age)
        {
            int ageIndex = 0;
            int chanceIndex = 1;

            double chance = 0;
            if (age > 0 && age < InitialData[0][ageIndex])
                chance =  InitialData[0][chanceIndex];
            else
            {
                for(int i = 1; i < InitialData.Count; i++)
                {
                    if(age > InitialData[i - 1][ageIndex] && age < InitialData[i][ageIndex])
                    {
                        chance =  InitialData[i][chanceIndex];
                        break;
                    }
                }
            }
            return chance;
        }

    }
}
