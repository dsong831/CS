using System;

namespace CS004_Variable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hi, please introduce yourself");
            Console.Write(">> Name : ");
            string Name = Console.ReadLine();
            Console.Write(">> Age : ");
            int Age = int.Parse(Console.ReadLine());
            Console.Write(">> Height : ");
            float Height = float.Parse(Console.ReadLine());

            Console.WriteLine("");
            Console.Write("Hi Mr.");
            Console.Write(Name);
            Console.WriteLine(", nice to meet you.");
            Console.Write("You are ");
            Console.Write(Age);
            Console.WriteLine(" years old.");
            Console.Write("Your height is ");
            Console.Write(Height);
            Console.WriteLine("cm.");
        }
    }
}
