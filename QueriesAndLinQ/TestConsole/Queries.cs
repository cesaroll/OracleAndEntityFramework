using System;
using System.Linq;
using HRLibrary;

namespace TestConsole
{
    public class Queries
    {
        public void VerySimpleQuery()
        {
            var dc = new HREntities();

            var employees = from emp in dc.EMPLOYEES
                where emp.DEPARTMENT_ID == 90
                select emp;

            foreach (var emp in employees)
            {
                Console.WriteLine("{0} {1}", emp.FIRST_NAME, emp.LAST_NAME);
            }

        }

        public void ProjectionQuery()
        {
            var dc = new HREntities();

            var employees = dc.EMPLOYEES
                .Where(e => e.DEPARTMENT_ID == 100)
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new{ e.FIRST_NAME, e.LAST_NAME, e.EMAIL, e.PHONE_NUMBER});

            foreach (var emp in employees)
            {
                Console.WriteLine("{0} {1} - {2}", emp.FIRST_NAME, emp.LAST_NAME, emp.EMAIL);
            }

        }
    }
}