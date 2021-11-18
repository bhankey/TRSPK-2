using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2_1
{

    struct TaskV
    {
        public int X;
        public int Y;

        public TaskV(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    
    struct TaskG
    {
        public int X;
        public double Y;

        public TaskG(int x, double y)
        {
            X = x;
            Y = y;
        }
    }

    struct TaskG2
    {
        public double X;
        public int Y;

        public TaskG2(double x, int y)
        {
            X = x;
            Y = y;
        }
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
            // a
            var arr = new int[]{1, 2, 3, 5, 10, 1, 5};

            // usual
            var max = 0;
            foreach (var item in arr)
            {
                if (item > max)
                {
                    max = item;
                }
            }
            
            // LINQ
            Console.WriteLine(arr.Max());
            
            // b
            
            // usual
            var maxIndex = 0;
            max = 0;
            for (var i = 0; i < arr.Length; i++)
            {
                var item = arr[i];
                if (item > max)
                {
                    max = item;
                    maxIndex = i;
                }
            }

            // LINQ
            Console.WriteLine(arr.ToList().IndexOf(arr.Max()));
            
            // b
            
            // usual
            var arrStruct = new TaskV[]
            {
               new TaskV(1, 2),
               new TaskV(2, 1),
               new TaskV(3, 5),
            };

            TaskV structItem;
            max = 0;
            for (var i = 0; i < arrStruct.Length; i++)
            {
                var item = arrStruct[i];
                if (item.Y > max)
                {
                    max = item.Y;
                    structItem = item;
                }
            }

            // LINQ
            structItem = arrStruct.ToList().OrderByDescending(item => item.Y).First();
            Console.WriteLine($"X - {structItem.X}, Y {structItem.Y}");
            
            // g
            
            // usual
            var arrStructNew = new TaskG[]
            {
                new TaskG(1, 2.1),
                new TaskG(2, 1.1),
                new TaskG(3, 5.23),
                new TaskG(3, 1.23),
            };

            TaskG structItemNew;
            max = 0;
            Array.Sort(arrStructNew, (g1, g2) =>
            {
                if (g1.Y > g2.Y)
                    return 1;
                else if (g1.Y < g2.Y)
                    return -1;
                else
                    return 0;
            });

            var arrStructNewNew = new TaskG2[arrStructNew.Length];
            for (var i = 0; i < arrStruct.Length; i++)
            {
                arrStructNewNew[i] = new TaskG2(arrStructNew[i].X, (int)arrStructNew[i].Y);
            }

            // LINQ
            arrStructNewNew = arrStructNew.ToList().
                OrderBy(item => item.Y).
                Select(s => new TaskG2(s.X, (int)s.Y)).
                ToArray();

            Console.WriteLine("ArrNewNew");
            arrStructNewNew
                .ToList()
                .ForEach(s => Console.Write($"X - {s.X} Y - {s.Y}, "));
            
        }
    }
}