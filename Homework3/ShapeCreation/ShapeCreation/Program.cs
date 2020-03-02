using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inst= new string[]{"Rectangle","Square","Triangle","Circle"};
            Shape[] shapes = new Shape[10];
            double area = 0;
            Random rd = new Random();
            for (int i=0;i<10;i++)
            {
                int num = rd.Next(0, 3);
                Console.WriteLine($"Your {i + 1}th shape: {inst[num]}");
                shapes[i] = ShapeConstructor.Constructor(inst[num]);
                area += shapes[i].GetArea();
            }
            Console.WriteLine($"Total area is {area}.");
        }
    }
    class ShapeConstructor
    {
        public static Shape Constructor(string inst)
        {
            if(inst=="Rectangle")
            {
                string a, b;
                Console.Write("Your height: ");
                a = Console.ReadLine();
                Console.Write("Your width: ");
                b = Console.ReadLine();
                Shape shape = new Rectangle(Double.Parse(a), Double.Parse(a));
                return shape;
            }
            else if (inst == "Square")
            {
                string a;
                Console.Write("Your sidelength: ");
                a = Console.ReadLine();
                Shape shape = new Square(Double.Parse(a));
                return shape;
            }
            else if (inst == "Triangle")
            {
                string a,b,c;
                Console.Write("Your side A length: ");
                a = Console.ReadLine();
                Console.Write("Your side B length: ");
                b = Console.ReadLine();
                Console.Write("Your side C length: ");
                c = Console.ReadLine();
                Shape shape = new Triangle(Double.Parse(a), Double.Parse(b), Double.Parse(c));
                return shape;
            }
            else if(inst=="Circle")
            {
                string a;
                Console.Write("Your radius: ");
                a = Console.ReadLine();
                Shape shape = new Circle(Double.Parse(a));
                return shape;
            }
            else
            {
                Console.WriteLine("Invalid argument!\nConstruct a default shape Circle(radius = 1).");
                Shape shape = new Circle(1);
                return shape;
            }
        }
    }
    abstract class Shape
    {
        public abstract double GetArea();
        public abstract bool IsLegal();
    }
    class Rectangle:Shape
    {
        protected double height, width;
        public Rectangle(double height, double width)
        {
            this.height = height;
            this.width = width;
            if(!IsLegal())
            {
                this.height = 1;
                this.width = 1;
                Console.WriteLine("Invalid class argument!");
                Console.WriteLine("Initialize all arguments in 1.");
            }
        }
        public Rectangle() { height = 0; width = 0; } //default constructor
        public override double GetArea()
        {
            if(this.IsLegal())
                return height * width;
            Console.WriteLine("Illegal shape! Can't get its Area.");
            return 0;
        }
        public override bool IsLegal()
        {
            return height > 0 && width > 0;
        }
        public double Height
        {
            set => height = value;
            get => height;
        }
        public double Width
        {
            set => width = value;
            get => width;
        }
    }
    class Square : Shape
    {
        protected double sidelength;
        public Square(double sidelength)
        {
            this.sidelength = sidelength;
            if(!IsLegal())
            {
                sidelength = 1;
                Console.WriteLine("Invalid class argument!");
                Console.WriteLine("Initialize all arguments in 1.");
            }
        }
        public Square() { sidelength = 1; }
        public override double GetArea()
        {
            if (this.IsLegal())
                return sidelength * sidelength;
            Console.WriteLine("Illegal shape! Can't get its Area.");
            return 0;
        }
        public override bool IsLegal()
        {
            return sidelength > 0;
        }
        public double SideLen
        {
            set => sidelength = value;
            get => sidelength;
        }
    }
    class Triangle : Shape
    {
        protected double lenA, lenB, lenC;
        public Triangle(double A,double B, double C)
        {
            lenA = A;
            lenB = B;
            lenC = C;
            if(!IsLegal())
            {
                lenA = 3;lenB = 4;lenC = 5;
                Console.WriteLine("Invalid class argument!");
                Console.WriteLine("Initialize arguments in 3,4,5.");
            }
        }
        public Triangle() { lenA = 3; lenB = 4; lenC = 5; }
        public override double GetArea()
        {
            if(IsLegal())
            {
                double p = (lenA + lenB + lenC) / 2;
                return Math.Pow(p * (p - lenA) * (p - lenB) * (p - lenC), 0.5);
            }
            Console.WriteLine("Illegal shape! Can't get its Area.");
            return 0;
        }
        public override bool IsLegal()
        {
            if (lenA <= 0 || lenB <= 0 || lenC <= 0)
                return false;
            if (lenA + lenB <= lenC || lenA + lenC <= lenB || lenB + lenC <= lenA)
                return false;
            return true;
        }
        public double LenA
        {
            set => lenA = value;
            get => lenA;
        }
        public double LenB
        {
            set => lenB = value;
            get => lenB;
        }
        public double LenC
        {
            set => lenB = value;
            get => lenB;
        }
    }
    class Circle : Shape
    {
        protected double radius;
        public Circle(double r)
        {
            radius = r;
            if(!IsLegal())
            {
                radius = 1;
                Console.WriteLine("Invalid class argument!");
                Console.WriteLine("Initialize all arguments in 1.");
            }
        }
        public Circle() { radius = 1; }
        public override double GetArea()
        {
            if(IsLegal())
                return Math.PI * radius * radius;
            Console.WriteLine("Illegal shape! Can't get its Area.");
            return 0;
        }
        public override bool IsLegal()
        {
            return radius > 0;
        }
        public double R
        {
            set => radius = value;
            get => radius;
        }
    }
}
