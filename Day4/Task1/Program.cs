using System;
using System.Threading;

namespace Task1
{
    
    
    class Program
    {
        
        private const int needToStart = 3;
        
        private static void SomeWork(Semaphore autoEvent)
        {
            autoEvent.Release();
            
            Console.WriteLine("Working....");
        }
        
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(10, 10);
            var autoEvent = new Semaphore(0, needToStart);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < needToStart; i++)
            {
                Thread t = new Thread(state => SomeWork(autoEvent));
                t.Start();
            }
            
            var started = 0;
            while (autoEvent.WaitOne())
            {
                started++;
                if (started == needToStart)
                {
                    break;
                }
            }
            
            watch.Stop();
            Console.WriteLine($"Started for {watch.ElapsedTicks}");
            
            
            watch.Reset();
            
            Thread.Sleep(1000);
            
            watch.Start();
            for (int i = 0; i < needToStart; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((state => SomeWork(autoEvent))));
            }
            
            
            started = 0;
            while (autoEvent.WaitOne())
            {
                started++;
                if (started == needToStart)
                {
                    break;
                }
            }
            
            watch.Stop();
            Console.WriteLine($"Started for {watch.ElapsedTicks}");
        }
    }
}