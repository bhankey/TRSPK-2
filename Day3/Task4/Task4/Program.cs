using System;
using System.Threading;

namespace Task4
{
    class Program
    {
        class ThreadSafe
        {
            static readonly object _locker = new object();
            private static int i;

            public static int Get()
            {
                lock (_locker) return i;
            }
            public static void Increment() { lock (_locker) i++; }
            public static void Assign(int x)    { lock (_locker) i = x; }
        }

        static void FirstFunction()
        {
            while (true)
            {
                if (ThreadSafe.Get() == 1)
                {
                    Console.Write("1");
                    ThreadSafe.Increment();
                }
            }
        }
        
        static void SecondFunction()
        {
            while (true)
            {
                if (ThreadSafe.Get() == 2)
                {
                    Console.Write(" 2");
                    ThreadSafe.Increment();
                }
            }
        }
        
        static void ThirdFunction()
        {
            while (true)
            {
                if (ThreadSafe.Get() == 3)
                {
                    Console.WriteLine(" 3");
                    ThreadSafe.Assign(1);
                }
            }
        }
            
        
        static void Main(string[] args)
        {
            var f = new Thread(FirstFunction);
            var s = new Thread(SecondFunction);
            var t = new Thread(ThirdFunction);

            ThreadSafe.Assign(1);
            
            f.Start();
            s.Start();
            t.Start();

            f.Join();
            
        }
    }
}