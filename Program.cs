using System;

namespace DualNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dual Numbers 1.0\n1D function must starts from 't =>'\n3D function must starts from 'p =>'");
            Console.WriteLine("Examples:\nSqrt(2*2)\nt => Sin(t)\nt => Vec(1, 2, t)\np => p^Vec(0, 0, 1)");
            ReadLine.AddHistory("Sqrt(2*2)", "t => Sin(t)", "t => Vec(1, 2, t)", "p => p^Vec(0, 0, 1)");

            while (true)
            {
                try
                {
                    var code = ReadLine.Read("(prompt)> ");
                    if (code.StartsWith("t =>"))
                        Func<DualNumber>(code);
                    else if (code.StartsWith("p =>"))
                        Func<DualVectorGrad>(code);
                    else
                        Expr(code);
                }
                catch (Exception ex)
                {
                    PrintError(ex);
                }
            }
        }

        static void PrintError(Exception ex)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = oldColor;
        }

        static void Expr(string code)
        {
            var expr = Parser.Parse<object>(code);
            Console.WriteLine(expr);
        }

        static (bool, T) TryRead<T>()
        {
            Console.Write($"Enter expr or 'q': ");
            var line = Console.ReadLine();
            if (line == "q")
                return (true, default(T));
            return (false, Parser.Parse<T>($"Variable({line})"));
        }

        static void Func<T>(string code)
        {
            var f = Parser.Parse<Func<T, object>>(code);
            while (true)
            {
                try
                {
                    var (exit, value) = TryRead<T>();
                    if(exit)
                        return;
                    Console.WriteLine(f(value));
                }
                catch (Exception ex)
                {
                    PrintError(ex);
                }                
            }
        }
    }
}
