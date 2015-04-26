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
            Console.WriteLine("Press key to close");
            Console.ReadKey();
        }

        private static void Compute()
        {
            var anagramSolver = new AnagramSolver("poultry outwits ants");
            anagramSolver.Start();
        }
    }
}
