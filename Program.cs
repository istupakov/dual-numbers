using System;
using System.Threading.Tasks;

namespace DualNumbers
{
    class Program
    {
        static async Task Main()
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
                        await Func<DualNumber>(code);
                    else if (code.StartsWith("p =>"))
                        await Func<DualVectorGrad>(code);
                    else
                        await Expr(code);
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

        static async Task Expr(string code)
        {
            var expr = await Parser.Parse<object>(code);
            Console.WriteLine(expr);
        }

        static async Task<(bool, T)> TryRead<T>()
        {
            Console.Write($"Enter expr or 'q': ");
            var line = Console.ReadLine();
            if (line == "q")
                return (true, default);
            return (false, await Parser.Parse<T>($"Variable({line})"));
        }

        static async Task Func<T>(string code)
        {
            var f = await Parser.Parse<Func<T, object>>(code);
            while (true)
            {
                try
                {
                    var (exit, value) = await TryRead<T>();
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
