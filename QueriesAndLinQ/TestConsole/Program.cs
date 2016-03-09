using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }

                if (Console.ReadKey(true).KeyChar == '.')
                    return;
            }

        }
    }
}
