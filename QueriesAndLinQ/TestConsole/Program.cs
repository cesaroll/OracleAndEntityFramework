using System;
using System.ComponentModel.Design;
using HRLibrary;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hrContext = new HREntities())
            {
                var q = new Queries(hrContext); //injecting Context

                Menu(q);

            }

        }


        private static void Menu(Queries q)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("A: Very Simple Query");
                Console.WriteLine("B: Projection Query");
                Console.WriteLine("C: Navigaton References Query");
                Console.WriteLine("D: Navigaton References Query 3");
                Console.WriteLine("E: Navigaton Collection");
                Console.WriteLine("F: Navigaton Collection 2");
                Console.WriteLine("G: Navigaton Collection Any");
                Console.WriteLine("H: Aggregates");
                Console.WriteLine("I: Grouping with Projection");
                Console.WriteLine("J: Basic Entity SQL");
                

                Console.WriteLine("\nSelect an option ('.' to exit...)");

                var key = Char.ToUpper(Console.ReadKey().KeyChar);

                if (key == '.')
                    return;

                Console.Clear();

                switch (key)
                {
                    case 'A':
                        q.VerySimpleQuery();
                        break;
                    case 'B':
                        q.ProjectionQuery();
                        break;
                    case 'C':
                        q.NavigationReferencesQuery();
                        q.NavigationReferenceQuery2();
                        break;
                    case 'D':
                        q.NavigationReferencesQuery3();
                        break;
                    case 'E':
                        q.NavigationCollection();
                        break;
                    case 'F':
                        q.NavigationCollection2();
                        break;
                    case 'G':
                        q.NavigationCollectionAny();
                        break;
                    case 'H':
                        q.AggregateQuery();
                        q.AggregateQuery2();
                        break;
                    case 'I':
                        q.GroupinWithProjectionQuery();
                        break;
                    case 'J':
                        q.BasicEntitySQL();
                        break;
                }

                if (Console.ReadKey(true).KeyChar == '.')
                    return;
            }
        }

    }
}
