using System;
using System.Collections.Generic;
using System.Linq;

namespace Demographic.FileOperations
{
    public sealed class FilesData
    {
        public static FilesData instance;

        public static List<List<double>> InitialData;
        public static List<List<double>> DeathChance;

        private FilesData(List<List<double>> pathInitialData, List<List<double>> pathDeathData)
        {
            InitialData = pathInitialData;
            DeathChance = pathDeathData;
        }

        public static void GetInstance(List<List<double>> pathInitialData, List<List<double>> pathDeathData)
        {
            if (instance == null)
            {
                instance = new FilesData(pathInitialData, pathDeathData);
            }
        }

        public static FilesData GetInstance()
        {
            return instance;
        }

        public static double GetInitialChance(int age)
        {
            int ageIndex = 0;
            int chanceIndex = 1;

            double chance = 0;
            if (age > 0 && age < InitialData[0][ageIndex])
                chance = InitialData[0][chanceIndex];
            else
            {
                for (int i = 1; i < InitialData.Count; i++)
                {
                    if (age > InitialData[i - 1][ageIndex] && age < InitialData[i][ageIndex])
                    {
                        chance = InitialData[i][chanceIndex];
                        break;
                    }
                }
            }
            return chance;
        }

        public static double GetDeathChance(int age, Gender gender)
        {
            int ageIndexFrom = 0, ageIndexTill = 1;
            int chanceIndexMale = 2, chanceIndexFemale = 3;

            double chance = 0;
                for (int i = 0; i < InitialData.Count; i++)
                {
                    if (age > DeathChance[i][ageIndexFrom] && age < DeathChance[i][ageIndexTill])
                    {
                        if(gender == Gender.male)
                        {
                            chance = DeathChance[i][chanceIndexMale];
                            break;
                        } 
                        else if (gender == Gender.female)
                        {
                            chance = DeathChance[i][chanceIndexFemale];
                            break;
                        }
                    }
                }
            
            return chance;
        }
    }
}
