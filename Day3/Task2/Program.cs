using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task3_2
{
    class Program
    {
        static void mythread1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 1 выводит " + i);
            }
        }

        static void mythread2()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 2 выводит " + i);
            }
        }

        static void mythread3()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 3 выводит " + i);
            }
        }

        static void Main(string[] args)
        {
            Thread thread1 = new Thread(mythread1);
            Thread thread2 = new Thread(mythread2);
            Thread thread3 = new Thread(mythread3);
            thread1.Priority = ThreadPriority.Highest;
            thread2.Priority = ThreadPriority.Highest;
            thread3.Priority = ThreadPriority.Highest;

            thread1.Start();
            thread2.Start();
            thread3.Start();

            Console.ReadLine();
        }
    }
}
