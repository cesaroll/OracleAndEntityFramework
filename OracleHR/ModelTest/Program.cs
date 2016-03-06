using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRLibrary;

namespace ModelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var hr = new Entities();

            var employees =
                hr.EMPLOYEES.Where(e => e.LAST_NAME.StartsWith("P")).OrderBy(e => e.LAST_NAME).ThenBy(e => e.FIRST_NAME);

            Console.WriteLine("Selected Products:");
            Console.WriteLine("------------------");
            Console.WriteLine("       Employee         Salary");
            Console.WriteLine("--------------------  -------------");

            foreach (var emp in employees)
            {
                Console.WriteLine("{0,-22} {1:C}", emp.LAST_NAME + ", " + emp.FIRST_NAME, emp.SALARY);
            }
        }
    }
}
