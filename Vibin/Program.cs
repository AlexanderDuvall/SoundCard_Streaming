using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Vibin
{
    internal class Program : extends
    {
        public static void loop()
        {
            Console.WriteLine("mkay");
            for (int i = 0; i <= 10; i++)
            {
                if (i > 5)
                {
                    Console.WriteLine(i);
                    continue;
                }
                else
                {
                    Console.WriteLine("just doing our thing");
                }

                Console.WriteLine(114);
            }
        }

        public static void array()
        {
            int[] arg = new int[10];
            for (int i = 0; i < arg.Length; i++)
            {
                arg[i] = i * 20;
            }
        }

        public static void practice()
        {
           
            //can be declared null too.. without it cant be 
            double? num = 12.3;
            double? num2 = null;
            double? num4;
            // num 4 = one of the two that isnt null
            num4 = num2 ?? num;
            Console.WriteLine("number is {0} ", num4);
        }

       
    }
}

class extends
{
    public static void extension()
    {
        Console.WriteLine("C# version of java extends");
    }
}