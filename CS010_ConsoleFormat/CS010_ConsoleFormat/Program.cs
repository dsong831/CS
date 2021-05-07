using System;

namespace CS010_ConsoleFormat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            Console.WriteLine("Standard Numeric Format Specifiers");
            Console.WriteLine(
                "(C) Currency : . . . . . . . {0:C3}\n" +
                "(D) Decimal  : . . . . . . . {0:D4}\n" +
                "(E) Scientific : . . . . . . {1:E2}\n" +
                "(F) Fixed point :  . . . . . {1:F2}\n" +
                "(G) General   :  . . . . . . {1:G5}\n" +
                "(N) Number   : . . . . . . . {0:N2}\n" +
                "(P) Percent  : . . . . . . . {1:P2}\n" +
                "(R) Round-trip : . . . . . . {1:R}\n" +
                "(X) Hexadecimal :  . . . . . {2:X4}\n" +
                "(X) Hexadecimal :  . . . . . {2, 20:X4}\n",
                -12345678, -1234.5678f, 255
                );
        }
    }
}
