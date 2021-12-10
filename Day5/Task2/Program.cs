using System;

namespace Task2
{
    class Program
    {

        static private byte[] AllocateMemory(int elements)
        {
            return new byte[elements];
        }

        static private void F()
        {
            var first = AllocateMemory(100);

            Decimal bytesAllocated = 0;
            var toAllocateByIter = 10000;
            var toPrint = true;
            var random = new Random(123);
            while(true)
            {
                var toAlloc = random.Next(10, 1000);
                var _ = AllocateMemory(toAlloc);
                bytesAllocated += toAlloc;

                if (System.GC.GetGeneration(first) == 1 && toPrint)
                {
                    toPrint = false;
                    Console.WriteLine($"first generation after allocated {bytesAllocated}");
                }
                
                if (System.GC.GetGeneration(first) == 2)
                {
                    Console.WriteLine($"second generation after allocated {bytesAllocated}");
                    
                    break;
                }
            }
        }

        static private void S()
        {
            var first = AllocateMemory(851000); // LOH - 85000 > bytes
            var second = AllocateMemory(851000);

            var fGeneration = System.GC.GetGeneration(first);
            var sGeneration = System.GC.GetGeneration(second);
            Console.WriteLine($"first generation {fGeneration}"); // Always 2
            Console.WriteLine($"second generation {sGeneration}");
            
            System.GC.Collect();
            
           fGeneration = System.GC.GetGeneration(first);
            sGeneration = System.GC.GetGeneration(second);
            Console.WriteLine($"first generation {fGeneration}");
            Console.WriteLine($"second generation {sGeneration}");
        }

        static void Main(string[] args)
        {
            S();
        }
    }
}