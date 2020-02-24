using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] input;
            int sum = 0, max = Int32.MinValue, min = Int32.MaxValue;
            string s;
            Console.Write("Please enter the figure number of your array: ");
            s = Console.ReadLine();
            int num = Int32.Parse(s);
            input = new int[num];
            for(int i=0;i<num;i++)
            {
                s = Console.ReadLine();
                input[i] = Int32.Parse(s);
                sum += input[i];
                if (input[i] > max)
                    max = input[i];
                if (input[i] < min)
                    min = input[i];
            }
            double mean = sum / (double)num;
            Console.WriteLine($"The max value is {max}.");
            Console.WriteLine($"The min value is {min}.");
            Console.WriteLine($"The sum value of the array is {sum}.");
            Console.WriteLine($"The mean value of the array is {mean}");
        }
    }
}
