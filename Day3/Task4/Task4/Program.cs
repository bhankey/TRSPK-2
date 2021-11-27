using System;
using System.Threading;

namespace Task4
{
    class Program
    {
        static void FirstFunction(AutoResetEvent first, AutoResetEvent second)
        {
            while (true)
            {
                first.WaitOne();

                Console.Write("1");

                second.Set();
            }
        }

        static void SecondFunction(AutoResetEvent second, AutoResetEvent third)
        {
            while (true)
            {
                second.WaitOne();

                Console.Write(" 2");

                third.Set();
            }
        }

        static void ThirdFunction(AutoResetEvent third, AutoResetEvent first)
        {

            while (true)
            {
                third.WaitOne();

                Console.WriteLine(" 3");

                first.Set();
            }
        }
            
        
        static void Main(string[] args)
        {
            var event1 = new AutoResetEvent(false);
            var event2 = new AutoResetEvent(false);
            var event3 = new AutoResetEvent(false);
            
            var f = new Thread(() => FirstFunction(event1, event2));
            var s = new Thread(() => SecondFunction(event2, event3));
            var t = new Thread(() => ThirdFunction(event3, event1));

            f.Start();
            s.Start();
            t.Start();

            event1.Set();

            f.Join();
            
        }
    }
}