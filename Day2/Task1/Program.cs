using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2_1
{
    public class GeometryException : Exception
    {
        public Int32[] Parameters { get; private set; } = { 0, 0, 0, 0 };

        public GeometryException(string message) : base(message)
        {

        }
    }

    public class TriangleException : GeometryException
    {
        public TriangleException(string message) : base(message)
        {
            Console.WriteLine("Ошибка в построении треугольника.");
        }
    }

    public class QuadrangleException : GeometryException
    {
        public QuadrangleException(string message) : base(message)
        {
            Console.WriteLine("Ошибка в построении четырёхугольника.");
        }
    }

    public class CircleException : GeometryException
    {
        public CircleException(string message) : base(message)
        {
            Console.WriteLine("Ошибка в построении круга.");
        }
    }

    public class Triangle
    {
        public Triangle(Int32 a, Int32 b, Int32 c)
        {
            try
            {
                if (a <= 0 || b <= 0 || c <= 0)
                {
                    throw new Exception();
                }
                else if (a + b <= c || b + c <= a || a + c <= b)
                {
                    throw new Exception();
                }
                else Console.WriteLine("Треугольник создан");
            }
            catch (Exception TriangleException)
            {
                Console.WriteLine("Нельзя создать треугольник");
            }
            finally
            {

            }
        }
    }

    public class Quadrangle
    {
        public Quadrangle(Int32 a, Int32 b, Int32 c, Int32 d)
        {
            try
            {
                if (a <= 0 || b <= 0 || c <= 0 || d <= 0)
                {
                    throw new Exception();
                }
                else if (a + b + c <= d || a + b + d <= c || a + c + d <= b || b + c + d <= a)
                {
                    throw new Exception();
                }
                else Console.WriteLine("Четырехугольник создан");
            }
            catch (Exception QuadrangleException)
            {
                Console.WriteLine("Нельзя создать четырехугольник");
            }
            finally
            {

            }
        }
    }

    public class Circle
    {
        public Circle(Int32 r)
        {
            try
            {
                if (r <= 0)
                {
                    throw new Exception();
                }
                else Console.WriteLine("Круг создан");
            }
            catch (Exception CircleException)
            {
                Console.WriteLine("Нельзя оздать круг");
            }
            finally
            {

            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            String a, b, c, d, r;
            String command = "";
            GeometryException geoException = new GeometryException("Ошибка в построении фигуры.");

            Console.WriteLine("Выберите фигуру: ");
            Console.WriteLine("1. Треугольник");
            Console.WriteLine("2. Четырёхугольник");
            Console.WriteLine("3. Круг");
            command = Console.ReadLine();
            switch (command)
            {
                case "1":

                    Console.Write("Введите длину первой стороны: ");
                    a = Console.ReadLine();
                    Int32 aTriangle = Int32.Parse(a);
                    geoException.Parameters[0] = aTriangle;

                    Console.Write("Введите длину второй стороны: ");
                    b = Console.ReadLine();
                    Int32 bTriangle = Int32.Parse(b);
                    geoException.Parameters[1] = bTriangle;

                    Console.Write("Введите длину третьей стороны: ");
                    c = Console.ReadLine();
                    Int32 cTriangle = Int32.Parse(c);
                    geoException.Parameters[2] = cTriangle;

                    Triangle triangle = new Triangle(aTriangle, bTriangle, cTriangle);

                    Console.WriteLine("Список переданных параметров:");
                    for (int i = 0; i <= 2; i++)
                    {
                        Console.WriteLine(geoException.Parameters[i]);
                    }

                    break;

                case "2":

                    Console.Write("Введите длину первой стороны: ");
                    a = Console.ReadLine();
                    Int32 aQuadrangle = Int32.Parse(a);
                    geoException.Parameters[0] = aQuadrangle;

                    Console.Write("Введите длину второй стороны: ");
                    b = Console.ReadLine();
                    Int32 bQuadrangle = Int32.Parse(b);
                    geoException.Parameters[1] = bQuadrangle;

                    Console.Write("Введите длину третьей стороны: ");
                    c = Console.ReadLine();
                    Int32 cQuadrangle = Int32.Parse(c);
                    geoException.Parameters[2] = cQuadrangle;

                    Console.Write("Введите длину четвёртой стороны: ");
                    d = Console.ReadLine();
                    Int32 dQuadrangle = Int32.Parse(d);
                    geoException.Parameters[3] = dQuadrangle;

                    Quadrangle quadrangle = new Quadrangle(aQuadrangle, bQuadrangle, cQuadrangle, dQuadrangle);

                    Console.WriteLine("Список переданных параметров:");
                    for (int i = 0; i <= 3; i++)
                    {
                        Console.WriteLine(geoException.Parameters[i]);
                    }

                    break;

                case "3":

                    Console.Write("Введите длину радиуса: ");
                    r = Console.ReadLine();
                    Int32 rCircle = Int32.Parse(r);
                    geoException.Parameters[0] = rCircle;

                    Circle circle = new Circle(rCircle);

                    Console.WriteLine("Список переданных параметров:");
                    Console.WriteLine(geoException.Parameters[0]);

                    break;

                default:

                    break;
            }

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}


