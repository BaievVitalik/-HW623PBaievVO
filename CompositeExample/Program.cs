using System;
using System.Collections.Generic;


namespace CompositePatternDemo
{

    public abstract class FileSystemItem
    {
        public string Name { get; set; }

        public FileSystemItem(string name)
        {
            this.Name = name;
        }

        public abstract void Display(int depth);

        public virtual void Add(FileSystemItem item)
        {
            throw new NotSupportedException("Cannot add to a leaf item.");
        }

        public virtual void Remove(FileSystemItem item)
        {
            throw new NotSupportedException("Cannot remove from a leaf item.");
        }
    }

    public class File : FileSystemItem
    {
        public File(string name) : base(name)
        {
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + " " + Name);
        }
    }

    public class Directory : FileSystemItem
    {
        private List<FileSystemItem> _children = new List<FileSystemItem>();

        public Directory(string name) : base(name)
        {
        }

        public override void Add(FileSystemItem item)
        {
            _children.Add(item);
        }

        public override void Remove(FileSystemItem item)
        {
            _children.Remove(item);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + "+ " + Name);

            foreach (var item in _children)
            {
                item.Display(depth + 2);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Демонстрація шаблону Composite ---");

            var root = new Directory("root");

            var documents = new Directory("Documents");
            documents.Add(new File("report.docx"));
            documents.Add(new File("presentation.pptx"));

            var pictures = new Directory("Pictures");
            var vacation = new Directory("Vacation2024");
            vacation.Add(new File("photo1.jpg"));
            vacation.Add(new File("photo2.jpg"));
            pictures.Add(vacation);
            pictures.Add(new File("logo.png"));

            root.Add(documents);
            root.Add(pictures);
            root.Add(new File("readme.txt"));

            root.Display(0);

            Console.ReadKey();
        }
    }
}
