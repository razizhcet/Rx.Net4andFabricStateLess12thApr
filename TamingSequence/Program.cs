using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Rx.TamingSequence
{
    class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        static Employee emp;
        static List<Employee> list = new List<Employee>();
        static void Add()
        {
            Console.Write("No. of Record : ");
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                Employee e = new Employee();
                Console.Write("Enter Name : ");
                e.Name = Console.ReadLine();
                Console.Write("Enter Age : ");
                e.Age = Convert.ToInt32(Console.ReadLine());
                //list.Add(e);
                emp = e;
            }

        }

        static void ConvertIntoJson()
        {

            StreamWriter sw = new StreamWriter(new FileStream("File.json", FileMode.Create));
            var s = JsonConvert.SerializeObject(emp);
            sw.Write(s);
            sw.Close();
        }

        /*
        static void ConverIntoString()
        {
            string json = File.ReadAllText("File.json");
            Employee[] s1 = JsonConvert.DeserializeObject<Employee[]>(json);
            int i = 1;
            foreach (Employee s2 in s1)
            {
                Console.WriteLine("\n" + i + " Record");
                Console.WriteLine("Name : " + s2.Name);
                Console.WriteLine("Age  : " + s2.Age);
                i++;
            }
        }
        */

        static void ConvertIntoSequence()
        {
            string json = File.ReadAllText("File.json");
            //Employee[] s1 = JsonConvert.DeserializeObject<Employee[]>(json);
            Employee s1 = JsonConvert.DeserializeObject<Employee>(json);




            var subject = new Subject<Employee>();
            subject.Subscribe(
              x => Console.WriteLine( "Name : " + x.Name + "\nAge : " + x.Age),
              e => Console.WriteLine( e),
              () => Console.WriteLine("completed"));
            //foreach(var obj in s1)
            subject.OnNext(s1);


            //var subject = new Subject<Employee>();
            //subject
            //.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
           
            subject.OnCompleted();
        }
        static void Main(string[] args)
        {
            Add();
            ConvertIntoJson();
            //ConverIntoString();
            ConvertIntoSequence();
            Console.ReadKey();
        }
    }
}



