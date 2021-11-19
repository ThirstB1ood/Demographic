using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Demographic.FileOperations
{
    public sealed class FileReader
    {

        private readonly string fileName;
        /// <summary>
        /// size in bytes
        /// </summary>
        private readonly int sizeFile = 10000;
        private readonly int countValues = 4;

        public FileReader(string filename)
        {
            this.fileName = filename;
        }

        public List<List<double>> DivideIrises()
        {
            CheckFile();
            string[] arrayStrings = File.ReadAllLines(fileName);
            var dataFile = new List<List<double>>();

            CheckStrings(arrayStrings);

            string[][] data;
            data = arrayStrings.Select(x => x.Split(',')).ToArray();

            foreach (string[] str in data.Skip(1))
            {
                List<double> temp = new List<double>();
                temp = CheckValue(str, data[0].Length);

                dataFile.Add(temp);
            }

            //CheckArray(data);

            return dataFile;
        }

        private List<double> CheckValue(string[] str, int length)
        {
            var temp = new List<double>();
            var number = 0.0;
            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(str[i]))
                {
                    throw new Exception("Empty number in file");
                }
                if (!Double.TryParse(str[i].Replace('.', ','), out number))
                {
                    throw new Exception("Wrong number");
                }
                temp.Add(number);
            }
            return temp;
        }

        private void CheckArray(string[][] data)
        {
            HashSet<string> set = new HashSet<string>();
            foreach (string[] str in data.Skip(1))
            {
                set.Add(str[countValues]);
            }
            if (set.Count != 3)
            {
                if (set.Count == 0)
                {
                    throw new Exception("Not irises");
                }
                else
                {
                    throw new Exception("Count irises not 3");
                }
            }
        }

        private void CheckStrings(string[] arrayString)
        {
            foreach (string str in arrayString)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("Empty string in file");
                }
                else if (!str.Contains(','))
                {
                    throw new Exception("Error file");
                }
            }
        }

        private void CheckFile()
        {
            FileInfo file = new FileInfo(fileName);
            if (fileName == "")
            {
                throw new FileNotFoundException();
            }
            else if (file.Length == 0)
            {
                throw new Exception("Empty file");
            }
            else if (!file.Exists)
            {
                throw new Exception("File not exists");
            }
            else if (file.Length > sizeFile)
            {
                throw new Exception("Size too large");
            }
        }
    }
}
