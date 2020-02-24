using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numgroup = new int[101];
            for(int i=2;i<=100;i++)
            {
                for (int j = 2; i * j <= 100; j++)
                    numgroup[i * j] = 1;
            }
            for (int i = 2; i <= 100; i++)
                if (numgroup[i] == 0)
                    Console.WriteLine(i.ToString());
        }
    }
}
