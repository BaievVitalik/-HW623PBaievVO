using System;
using System.Collections.Generic;

namespace PrototypePatternDemo
{
    public abstract class Shape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }

        public Shape()
        {
        }

        public Shape(Shape source)
        {
            if (source != null)
            {
                this.X = source.X;
                this.Y = source.Y;
                this.Color = source.Color;
            }
        }

        public abstract Shape Clone();

        public override string ToString()
        {
            return $"Position: ({X}, {Y}), Color: {Color}";
        }
    }

    public class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle()
        {
        }

        public Rectangle(Rectangle source) : base(source)
        {
            if (source != null)
            {
                this.Width = source.Width;
                this.Height = source.Height;
            }
        }

        public override Shape Clone()
        {
            return new Rectangle(this);
        }

        public override string ToString()
        {
            return $"Rectangle - {base.ToString()}, Size: ({Width}x{Height})";
        }
    }

    public class Circle : Shape
    {
        public int Radius { get; set; }

        public Circle()
        {
        }

        public Circle(Circle source) : base(source)
        {
            if (source != null)
            {
                this.Radius = source.Radius;
            }
        }

        public override Shape Clone()
        {
            return new Circle(this);
        }

        public override string ToString()
        {
            return $"Circle - {base.ToString()}, Radius: {Radius}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Демонстрація шаблону Prototype ---");

            var originalRectangle = new Rectangle
            {
                X = 10,
                Y = 20,
                Color = "Blue",
                Width = 100,
                Height = 50
            };

            var originalCircle = new Circle
            {
                X = 30,
                Y = 40,
                Color = "Red",
                Radius = 25
            };

            var clonedRectangle = (Rectangle)originalRectangle.Clone();
            var clonedCircle = (Circle)originalCircle.Clone();

            clonedRectangle.X = 110;
            clonedRectangle.Color = "Green";

            Console.WriteLine("Original Rectangle: " + originalRectangle);
            Console.WriteLine("Cloned Rectangle:   " + clonedRectangle);
            Console.WriteLine("\nOriginal Circle:    " + originalCircle);
            Console.WriteLine("Cloned Circle:      " + clonedCircle);

            Console.ReadKey();
        }
    }
}
