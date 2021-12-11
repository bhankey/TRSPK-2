using System;

namespace task5_1
{
    class Number
    {
        public int Num { set; get; }

        public Number(int Num)
        {
            this.Num = Num;
        }
    }

    class Program
    {
        static void Main()
        {
            Number zero = new Number(0);
            Console.WriteLine("Поколение объекта zero: " + GC.GetGeneration(zero));

            // сборка мусора
            GC.Collect(0, GCCollectionMode.Forced); //вызывает немедленное выполнение сборки мусора
            GC.WaitForPendingFinalizers(); //приостанавливает работу текущего потока до освобождения всех объектов, для которых производится сборка мусора
            Console.WriteLine("Собрали мусор. Поколение объекта zero: " + GC.GetGeneration(zero));

            GC.Collect(0, GCCollectionMode.Forced); 
            GC.WaitForPendingFinalizers(); 
            Console.WriteLine("Собрали мусор. Поколение объекта zero: " + GC.GetGeneration(zero));

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Собрали мусор. Поколение объекта zero: " + GC.GetGeneration(zero));

            Console.WriteLine();
            Console.WriteLine("Максимальное количество поддерживаемых поколений объектов " + GC.MaxGeneration); //Возвращает информацию о том, сколько максимум поколений поддерживается в целевой системе.(поддерживается всего три поколения: 0, 1 и 2)



            Number first = new Number(1);
            Console.WriteLine();
            Console.WriteLine("Поколение объекта first: " + GC.GetGeneration(first));
            for (int i = 0; i < 10000; i++)
            {
                Number second = new Number(2);
            }

            GC.Collect(0, GCCollectionMode.Forced); 
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Собрали мусор. Поколение объекта first: " + GC.GetGeneration(first));



            Number third = new Number(3);
            Console.WriteLine();
            Console.WriteLine("Поколение объекта third: " + GC.GetGeneration(third));
            for (int i = 0; i < 10000; i++)
            {
                Number forth = new Number(4);
                GC.Collect(0, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();
            }

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Собрали мусор. Поколение объекта third: " + GC.GetGeneration(third));

            Console.ReadLine();
        }
    }
}
