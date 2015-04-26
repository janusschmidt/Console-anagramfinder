using System;

namespace anagramfinderConsole
{
    class Program
    {
        static void Main()
        {
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("ITERATION {0}", i);
                Compute();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void Compute()
        {
            var anagramSolver = new AnagramSolver("poultry outwits ants");
            anagramSolver.Start();
        }
    }
}
