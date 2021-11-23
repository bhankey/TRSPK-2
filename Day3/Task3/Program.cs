using System;
using System.Threading;

namespace Task3
{
    class Program
    {
        private static int _i = 0;

        static void IncrementVar()
        {
            for (int j = 0; j < 1000000; j++)
            {
                _i++;
            }
        }
        
        static void IncrementVarSafe()
        {
            for (int j = 0; j < 1000000; j++)
            {
                Interlocked.Increment(ref _i);
            }
        }

        static void Main(string[] args)
        {
            const int countOfThreads = 10;
            var threads = new Thread[countOfThreads];
            
            for (int i = 0; i < countOfThreads; i++)
            {
                threads[i] = new Thread(IncrementVar);
            }
            
            for (int i = 0; i < countOfThreads; i++)
            {
                threads[i].Start();
            }
            
            for (int i = 0; i < countOfThreads; i++)
            {
                threads[i].Join();
            }
            
            
            Console.WriteLine($"i - {_i}");
        }
    }
}