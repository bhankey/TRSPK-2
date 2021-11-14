using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;

namespace Task1
{
    class Program
    {
        static void ArrayListTest()
        {
            ArrayList animalList = new ArrayList();

            animalList.Add(new Animal(5, "coshka"));
            animalList.Add(new Animal(10, "sobashka"));
            animalList.Add(new Animal(1, "kriska"));

            foreach (var animal in animalList)
            {
                Console.WriteLine(animal);
            }
            
            animalList.Sort(); // Failed to compare elements
            Console.WriteLine("sorted");
            foreach (var animal in animalList)
            {
                Console.WriteLine(animal);
            }
        }
        
        static void ListTest()
        {
            List<Animal> animalList = new List<Animal>();

            animalList.Add(new Animal(5, "coshka"));
            animalList.Add(new Animal(10, "sobashka"));
            animalList.Add(new Animal(1, "kriska"));

            foreach (var animal in animalList)
            {
                Console.WriteLine(animal);
            }
            
            animalList.Sort(); // Failed to compare elements
            Console.WriteLine("sorted");
            foreach (var animal in animalList)
            {
                Console.WriteLine(animal);
            }
        }
        
        static void Main(string[] args)
        {
            ArrayListTest(); // With IComparable<> didn't work
            
            Console.WriteLine();
            ListTest();
        }
    }
}