using System;

namespace DefaultNamespace
{
    public class Animal: IComparable<Animal>
    {
        private int _age;
        private string _name;

        public Animal(int age, string name)
        {
            _age = age;
            _name = name;
        }

        public int Age
        {
            get => _age;
            set => _age = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public override string ToString()
        {
            return $"animal with name {_name} and age {_age}";
        }

        // Old CompareTo
        //
        // public int CompareTo(object obj)
        // {
        //     if (obj == null)
        //     {
        //         return 1;
        //     }
        //
        //     var animal = obj as Animal;
        //     if (animal != null)
        //     {
        //         return this._age.CompareTo(animal.Age);
        //     }
        //     
        //     throw new ArgumentException("Object is not Animal");
        // }
        
        public int CompareTo(Animal? animal)
        {
            if (animal == null)
            {
                return 1;
            }
            
            return this._age.CompareTo(animal.Age);
        }
    }
}