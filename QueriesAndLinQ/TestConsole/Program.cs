using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new Queries();

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
                }

                if (Console.ReadKey(true).KeyChar == '.')
                    return;
            }

        }
    }
}
