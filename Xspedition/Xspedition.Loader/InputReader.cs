using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xspedition.Loader
{
    public class InputReader
    {
        public List<string> Lines { get; private set; } = new List<string>();
        public void ReadFromFile()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\", string.Empty);
            string path = baseDirectory + "Input.csv";

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    Lines.Add(sr.ReadLine());
                }
            }
        }
    }
}
