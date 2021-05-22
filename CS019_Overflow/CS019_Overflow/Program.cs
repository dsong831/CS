using System;

namespace CS019_Overflow
{
    class Program
    {
        static void Main(string[] args)
        {
            // y = int.MaxValue + 10;
            int x = int.MaxValue, y = 0;

            checked
            {
                try
                {
                    y = x + 10;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("int.MaxValue + 10 = {0}", y);
        }
    }
}
