using System;

namespace Task5
{
    public interface IToolKit
    {
        string[] GetTools();
    }

    public interface IParts
    {
        string[] GetParts();
    }
    public abstract class IkeaKit<TContents> where TContents : IToolKit, IParts, new()
    {
        public abstract string Title
        {
            get;
        }

        public abstract string Colour
        {
            get;
        }
        public void GetInventory()
        {
            var contents = new TContents();
            foreach (string tool in contents.GetTools())
            {
                Console.WriteLine("Tool: {0}", tool);
            }
            foreach (string part in contents.GetParts())
            {
                Console.WriteLine("Part: {0}", part);
            }
        }
    }

    public class Bed : IToolKit, IParts
    {
        public string[] GetTools()
        {
            return new string[] { "screwdriver ", "hammer"};
        }
        public string[] GetParts()
        {
            return new string[] { "leg", "leg", "leg", "leg", "headboard", "footboard", "BedBase" };
        }

    }
     public class Table : IToolKit, IParts
    {
        public string[] GetTools()
        {
            return new string[] { "screwdriver "};
        }
        public string[] GetParts()
        {
            return new string[] { "leg", "leg", "leg", "leg", "tabletop" };
        }

    }
    public class Nightstand : IToolKit, IParts
    {
        public string[] GetTools()
        {
            return new string[] { "screwdriver", "pliers " };
        }
        public string[] GetParts()
        {
            return new string[] { "door", "back wall", "side wall", "side wall", "top wall" };
        }

    }
    public class ViсtoriaBed: IkeaKit<Bed>
    {
        public override string Title
        {
            get { return "VictoriaBed"; }
        }
        public override string Colour
        {
            get { return "pink"; }
        }
    }
    public class SegaTableKirill : IkeaKit<Table>
    {
        public override string Title
        {
            get { return "SegaTableKirill"; }
        }
        public override string Colour
        {
            get { return "black"; }
        }
    }
    public class NatashaNightstand  : IkeaKit<Nightstand>
    {
        public override string Title
        {
            get { return "NatashaNightstand"; }
        }
        public override string Colour
        {
            get { return "white"; }
        }
    }


    class Program
    {
        static void Main(String[] args)
        {
            var bed = new ViсtoriaBed();
            Console.WriteLine( bed.Title);
            Console.WriteLine("Colour: {0} ", bed.Colour);
            bed.GetInventory();

            var table = new SegaTableKirill();
            Console.WriteLine(table.Title);
            Console.WriteLine("Colour: {0} ", table.Colour);
            table.GetInventory();

            var nightstand = new NatashaNightstand();
            Console.WriteLine(nightstand.Title);
            Console.WriteLine("Colour: {0} ", nightstand.Colour);
            nightstand.GetInventory();
        }
    }
}
