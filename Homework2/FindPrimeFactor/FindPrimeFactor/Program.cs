using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindPrimeFactor
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int mid, num, oriNum;
            Console.Write("Please enter a number: ");
            input = Console.ReadLine();
            check(input, out int flag);   //判断是否为合法输入
            if (flag == 1)
            {
                Console.WriteLine("Your input is invalid!");
                return;
            }

            num = Int32.Parse(input);
            oriNum = num;
            mid = (int)Math.Pow(num, 0.5);
            for (int i = 2; num > 1; i++)
            {
                if (!isPrime(i))
                    continue;
                while (num > 1)
                {
                    if (num % i != 0)
                        break;
                    num /= i;
                    Console.WriteLine($"{i}");
                }
            }
            if (oriNum == 1)
                Console.WriteLine("No factors for the number!");
            else
                Console.WriteLine("All the prime number factors have been found.");
        }
        static void check(string s, out int flag)
        {
            flag = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] <= '0' || s[i] >= '9')
                {
                    flag = 1;
                    break;
                }
            }
        }
        static bool isPrime(int num)
        {
            int cnt = 0;
            for (int i = 1; i * i <= num; i++)
            {
                if (num % i == 0)
                    cnt++;
            }
            if (num != 1 && cnt == 1)
                return true;
            else
                return false;
        }
    }
}
