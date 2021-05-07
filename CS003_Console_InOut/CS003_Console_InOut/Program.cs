using System;

namespace CS003_Console_InOut
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.Write("Insert your name : ");
            string name = Console.ReadLine();

            Console.WriteLine();
            Console.Write(name);
            Console.WriteLine(", nice to meet you.");
        }
    }
}
