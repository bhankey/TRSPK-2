using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task3_1
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
            for (int i = 0; i < 100000000; i++)
            {
                Console.WriteLine("Поток 3 выводит " + i);
            }
        }

        static void Main(string[] args)
        {
            Thread thread1 = new Thread(mythread1);
            Thread thread2 = new Thread(mythread2);
            Thread thread3 = new Thread(mythread3);

            String command = "";
            Console.WriteLine("1. Threads");
            Console.WriteLine("2. Background Threads");
            Console.WriteLine("3. Infinite Thread");
            command = Console.ReadLine();
            switch (command)
            {
                case "1":

                    thread1.Start();
                    thread2.Start();

                    break;

                case "2":

                    thread1.IsBackground = true;
                    thread2.IsBackground = true;
                    thread3.IsBackground = true;
                    thread1.Start();
                    thread2.Start(); 
                    thread3.Start();
                    Thread.Sleep(1000000000);

                    break;

                case "3":

                    while (true)
                    {
                        thread1.Start();
                    }

                    break;
            }

            Console.ReadLine();
        }
    }
}
