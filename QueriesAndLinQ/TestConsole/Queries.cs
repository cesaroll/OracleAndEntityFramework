using System;
using System.Linq;
using HRLibrary;

namespace TestConsole
{
    public class Queries
    {
        #region Very Simple Query

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

        #endregion

        #region Projection Query

        public void ProjectionQuery()
        {
            var dc = new HREntities();

            var employees = dc.EMPLOYEES
                .Where(e => e.DEPARTMENT_ID == 100)
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new {e.FIRST_NAME, e.LAST_NAME, e.EMAIL, e.PHONE_NUMBER});

            foreach (var emp in employees)
            {
                Console.WriteLine("{0} {1} - {2}", emp.FIRST_NAME, emp.LAST_NAME, emp.EMAIL + "@mycompany.com");
            }

        }

        #endregion

        #region Navigaton References Query

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
                    Manager = (e.MANAGER != null) ? e.MANAGER.LAST_NAME + ", " + e.MANAGER.FIRST_NAME : ""
                });



            Console.WriteLine("         Employee            Manager");
            Console.WriteLine("==============================================");

            foreach (var item in employeeManagers)
            {
                Console.WriteLine("{0,-25} {1, -25}", item.Employee, item.Manager);
            }
            Console.WriteLine();
        }

        #endregion

        #region Navigation Collection

        public void NavigationCollection()
        {
            var dc = new HREntities();

            var empJobHist = dc.EMPLOYEES.Where(e => e.JOB_HISTORY.Count > 0)
                .Select(e => new
                {
                    Employee = e,
                    JobHistory = e.JOB_HISTORY
                });

            Console.WriteLine("Employee Job Histories 1:");
            Console.WriteLine("===============================");
            Console.WriteLine();

            foreach (var item in empJobHist)
            {
                Console.WriteLine("{0}, {1} ({2}):",
                    item.Employee.LAST_NAME,
                    item.Employee.FIRST_NAME,
                    item.Employee.EMAIL);
                Console.WriteLine("---------------------------");

                foreach (var hist in item.JobHistory.OrderByDescending(h => h.END_DATE))
                {
                    Console.WriteLine("   {0} - {1}",
                        hist.JOB.JOB_TITLE, hist.DEPARTMENT.DEPARTMENT_NAME);
                }

                Console.WriteLine();
            }

        }

        public void NavigationCollection2()
        {
            var dc = new HREntities();

            var empJobHist = dc.EMPLOYEES
                .Where(e => e.JOB_HISTORY.Count > 0)
                .Select(e => new
                {
                    Name =  e.LAST_NAME + ", " + e.FIRST_NAME,
                    Email = e.EMAIL,
                    JobHistory = e.JOB_HISTORY.OrderByDescending(h => h.END_DATE).Select(h => new
                    {
                        Job = h.JOB.JOB_TITLE,
                        Department = h.DEPARTMENT.DEPARTMENT_NAME
                    })
                });


            Console.WriteLine("Employee Job Histories 2:");
            Console.WriteLine("===============================");
            Console.WriteLine();

            foreach (var emp in empJobHist)
            {
                Console.WriteLine("{0}, ({1}):",
                    emp.Name, emp.Email);
                Console.WriteLine("---------------------------");

                foreach (var hist in emp.JobHistory)
                {
                    Console.WriteLine("   {0} - {1}",
                        hist.Job, hist.Department);
                }

                Console.WriteLine();
            }

        }

        #endregion

        #region Navigation Collection using "Any"

        /// <summary>
        /// Is better to use any than count
        /// Since the generated SQL will be using "EXISTS"
        /// instead of "Count(*)"
        /// Also Note DateTime.Compare
        /// </summary>
        public void NavigationCollectionAny()
        {
            var dc = new HREntities();

            var ldate = DateTime.Parse("2005-01-01").Date;

            var empJobHist = dc.EMPLOYEES
                .Where(e => e.JOB_HISTORY.Any(h => DateTime.Compare(h.END_DATE, ldate) >= 0))
                .OrderBy(e => e.LAST_NAME)
                .ThenBy(e => e.FIRST_NAME)
                .Select(e => new
                {
                    Name = e.LAST_NAME + ", " + e.FIRST_NAME,
                    Email = e.EMAIL +"@ces.edu",
                    JobHistory = e.JOB_HISTORY.Select(h => new
                    {
                        Job = h.JOB.JOB_TITLE,
                        Department = h.DEPARTMENT.DEPARTMENT_NAME,
                        EndDate = h.END_DATE
                    })
                });

            Console.WriteLine("Employee Job Histories 2:");
            Console.WriteLine("===============================");
            Console.WriteLine();

            foreach (var emp in empJobHist)
            {
                Console.WriteLine("{0}, ({1}):",
                    emp.Name, emp.Email);
                Console.WriteLine("---------------------------");

                foreach (var hist in emp.JobHistory)
                {
                    Console.WriteLine("   {0} - {1} (End Date: {2:D})",
                        hist.Job, hist.Department, hist.EndDate);
                }

                Console.WriteLine();
            }

        }

        #endregion
    }
}