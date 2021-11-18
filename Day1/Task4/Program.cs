using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DefaultNamespace;

namespace Task4
{

    
    class Program
    {
        

        static public long TestingPerformance<TContainer, TArg, TYpeOfCheck>(TContainer c, TArg e,  TYpeOfCheck t) 
            where TContainer: IList, new()
            where TYpeOfCheck: IPerformance
        {
            var watch = Stopwatch.StartNew();

            t.Check(c, e);
            
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }

        static public TContainer PrepareList<TContainer, TArg>(int count, TArg example)
            where TContainer: IList, new()
        {
            TContainer list = new TContainer();

            for (int i = 0; i < count; i++)
            {
                list.Add(example);
            }

            return list;
        }
        
        static void Main(string[] args)
        {
            var add = new AddPerformance();
            var get = new GetPerformance();
            
            // add testing
            try
            {
                var element = 5;
                var list = PrepareList<ArrayList, int>(0, element);
                Console.WriteLine($"Add time of ArrayList With int in milliseconds {TestingPerformance(list,element, add)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            try
            {
                var element = 5;
                var list = PrepareList<List<int>, int>(0, element);
                Console.WriteLine($"Add time of List With int in milliseconds {TestingPerformance(list, element, add)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            try
            {
                var element = "111";
                var list = PrepareList<ArrayList, string>(0, element);
                Console.WriteLine($"Add time of ArrayList With string in milliseconds {TestingPerformance(list, element, add)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            try
            {
                var element = "111";
                var list = PrepareList<List<string>, string>(0, element);
                Console.WriteLine($"Add time of List With string in milliseconds {TestingPerformance(list, element, add)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            
            // Get Testing
            try
            {
                var element = 5;
                var list = PrepareList<ArrayList, int>(IPerformance.CountOfIteration, element);
                Console.WriteLine($"Get time of ArrayList With int in milliseconds {TestingPerformance(list, element, get)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            try
            {
                var element = 5;
                var list = PrepareList<List<int>, int>(IPerformance.CountOfIteration, element);
                Console.WriteLine($"Get time of List With int in milliseconds {TestingPerformance(list, element, get)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            try
            {
                var element = "111";
                var list = PrepareList<ArrayList, string>(IPerformance.CountOfIteration, element);
                Console.WriteLine($"Get time of ArrayList With string in milliseconds {TestingPerformance(list, element, add)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
            try
            {
                var element = "111";
                var list = PrepareList<List<string>, string>(IPerformance.CountOfIteration, element);
                Console.WriteLine($"Get time of List With string in milliseconds {TestingPerformance(list, element, get)}");
            }
            finally
            {
                GC.Collect();
                Thread.Sleep(100);
            }
            
        }
    }
}