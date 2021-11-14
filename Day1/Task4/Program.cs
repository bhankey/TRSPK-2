using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Task4
{

    
    class Program
    {
        private static int _countOfIteration = 10000000;
        
        static long ArrayListIntTime()
        {
            ArrayList list = new ArrayList();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            for (var i = 0; i < _countOfIteration; ++i)
            {
                list.Add(i);
            }

            for (var i = 0; i < _countOfIteration; ++i)
            {
                _ = list.GetRange(i, 1);
            }
            
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        
        static long ListIntTime()
        { 
            var list = new List<int>();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            for (var i = 0; i < _countOfIteration; ++i)
            {
                list.Add(i);
            }

            for (var i = 0; i < _countOfIteration; ++i)
            {
                _ = list.GetRange(i, 1);
            }
            
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        
        static long ArrayListStringTime()
        {
            ArrayList list = new ArrayList();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            string someString = "123455";
            for (var i = 0; i < _countOfIteration; ++i)
            { ;
                list.Add(someString);
            }

            for (var i = 0; i < _countOfIteration; ++i)
            {
                _ = list.GetRange(i, 1);
            }
            
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        
        static long ListStringTime()
        { 
            var list = new List<string>();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            string someString = "123455";
            for (var i = 0; i < _countOfIteration; ++i)
            {
                list.Add(someString);
            }

            for (var i = 0; i < _countOfIteration; ++i)
            {
                _ = list.GetRange(i, 1);
            }
            
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        
        static void Main(string[] args)
        {
            System.GC.Collect();
            Console.WriteLine($"Time of ArrayList With int in milliseconds {ArrayListIntTime()}");
            System.GC.Collect();
            Console.WriteLine($"Time of List With int in milliseconds {ListIntTime()}");
            System.GC.Collect();
            Console.WriteLine($"Time of ArrayList With string in milliseconds {ArrayListStringTime()}");
            System.GC.Collect();
            Console.WriteLine($"Time of List With int in milliseconds {ListStringTime()}");
        }
    }
}