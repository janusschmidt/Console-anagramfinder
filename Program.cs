using System;

namespace anagramfinderConsole
{
    class Program
    {
        static void Main()
        {
            long totalMilliSeconds = 0;
            const int itterations = 10;

            //First iteration is slow. So spinup.
            Compute();

            for (var i = 1; i <= itterations; i++)
            {
                Console.WriteLine("ITERATION {0}", i);
                totalMilliSeconds += Compute();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Time to compute {0} iterations: {1}ms", itterations, totalMilliSeconds);
            Console.WriteLine("Average time to compute all anagrams: {0} ms", totalMilliSeconds * 1m / itterations);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press key to close");
            Console.ReadKey();
        }

        private static long Compute()
        {
            var anagramSolver = new AnagramSolver("poultry outwits ants");
            return anagramSolver.Start();
        }
    }
}
