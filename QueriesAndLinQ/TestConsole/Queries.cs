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
                Console.WriteLine("{0} {1} - {2}", emp.FIRST_NAME, emp.LAST_NAME, emp.EMAIL + "@mycompany.com");
            }

        }


        public void NavigationReferencesQuery()
        {
            var dc = new HREntities();

            /*
            var employeeManagers = dc.EMPLOYEES
                .Where(e => e.DEPARTMENT_ID == 100)
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new
                {
                    Employee = e,
                    Manager = e.MANAGER
                });
            */

            var employeeManagers = dc.EMPLOYEES
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new
                {
                    Employee = e,
                    Manager = e.MANAGER
                });

            Console.WriteLine("         Employee            Manager");
            Console.WriteLine("==============================================");

            foreach (var item in employeeManagers)
            {
                var emp = string.Format("{0}, {1}", item.Employee.LAST_NAME, item.Employee.FIRST_NAME);
                var mgr = string.Empty;

                if (item.Manager != null)
                    mgr = string.Format("{0}, {1}", item.Manager.LAST_NAME, item.Manager.FIRST_NAME);


                Console.WriteLine("{0,-25} {1, -25}", emp, mgr);
            }
            Console.WriteLine();
        }

        public void NavigationReferenceQuery2()
        {
            var dc = new HREntities();

            var managerSubordinates = dc.EMPLOYEES
                .Where(e => e.SUBORDINATES.Count > 0)
                .Select(e => new
                {
                    Manager = e,
                    Subordinates = e.SUBORDINATES
                });

            foreach (var item in managerSubordinates)
            {
                Console.WriteLine("Manager: {0}, {1}", item.Manager.LAST_NAME, item.Manager.FIRST_NAME);
                Console.WriteLine("===============================================");

                foreach (var sub in item.Subordinates)
                {
                    Console.WriteLine("     {0}, {1}", sub.LAST_NAME, sub.FIRST_NAME);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This one does  work
        /// Just beware of the Syntax
        /// Cannot use String.Format or String.empty
        /// </summary>
        public void NavigationReferencesQuery3()
        {
            var dc = new HREntities();

            var employeeManagers = dc.EMPLOYEES
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new
                {
                    Employee = e.LAST_NAME + ", " + e.FIRST_NAME,
                    Manager = (e.MANAGER != null)? e.MANAGER.LAST_NAME + ", " + e.MANAGER.FIRST_NAME : ""
                });

            

            Console.WriteLine("         Employee            Manager");
            Console.WriteLine("==============================================");

            foreach (var item in employeeManagers)
            {
                Console.WriteLine("{0,-25} {1, -25}", item.Employee, item.Manager);
            }
            Console.WriteLine();
        }

    }
}