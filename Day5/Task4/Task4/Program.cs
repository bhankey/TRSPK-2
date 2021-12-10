using System;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var f = new ClassWithPerfectNaming("C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\Day5\\Task4\\l1.txt", "C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\Day5\\Task4\\l2.txt"))
            {
                f.Open();
                f.Work();
            }
            
            
        }
    }
}