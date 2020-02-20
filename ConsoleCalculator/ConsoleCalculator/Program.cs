using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            char c;
            double a, b, ans;
            Console.Write("Please enter your first number: ");
            s = Console.ReadLine();
            a = Double.Parse(s);
            Console.Write("Please enter your second number: ");
            s = Console.ReadLine();
            b = Double.Parse(s);
            Console.Write("Please enter your operator: ");
            c = (char)Console.Read();
            if (c == '+')
                ans = a + b;
            else if (c == '-')
                ans = a - b;
            else if (c == '*')
                ans = a * b;
            else if (c == '/')
            {
                if (b == 0)
                {
                    Console.WriteLine("The second number can't be 0!");
                    return;
                }
                else
                    ans = a / b;
            }
            else
            {
                Console.WriteLine("Error input operator!");
                return;
            }
            Console.WriteLine($"The answer is {ans}");
        }
    }
}
