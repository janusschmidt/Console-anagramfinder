using System;

namespace anagramfinderConsole
{
    class Program
    {
        static long totalComputeAllMilliSeconds;
        static long totalTimeToFindTheRightAnagram;

        static void Main()
        {
            const int itterations = 10;

            //First iteration is slow. So spinup.
            ComputeOnce();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            totalComputeAllMilliSeconds = 0;
            totalTimeToFindTheRightAnagram = 0;
            for (var i = 1; i <= itterations; i++)
            {
                Console.WriteLine("ITERATION {0}", i);
                ComputeOnce();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Time to compute {0} iterations: {1} ms", itterations, totalComputeAllMilliSeconds);
            Console.WriteLine("Average time to find THE RIGHT anagrams: {0} ms", totalTimeToFindTheRightAnagram * 1m / itterations);
            Console.WriteLine("Average time to compute all anagrams: {0} ms", totalComputeAllMilliSeconds * 1m / itterations);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press key to close");
            Console.ReadKey();
        }

        private static void ComputeOnce()
        {
            var anagramSolver = new AnagramSolver("poultry outwits ants");
            anagramSolver.Start();
            totalComputeAllMilliSeconds += anagramSolver.TotalTimeToComputeAllAnagrams;
            totalTimeToFindTheRightAnagram += anagramSolver.TotalTimeToFindTheRightAnagram;
        }
    }
}
