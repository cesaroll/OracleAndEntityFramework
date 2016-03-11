using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using HRLibrary;

namespace TestConsole
{
    public class Queries
    {
        #region Constructors

        public Queries(HREntities context)
        {
            dc = context;
        }

        #endregion
        
        #region Properties

        private HREntities dc { get; set; }

        #endregion


        #region Very Simple Query

        public void VerySimpleQuery()
        {
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

        #region Aggregates

        /// <summary>
        /// Get Number of employees by department
        /// </summary>
        public void AggregateQuery()
        {
            var depCount = dc.DEPARTMENTS
                .Where(d => d.EMPLOYEES.Any())
                .OrderBy(d => d.DEPARTMENT_NAME)
                .Select(d => new
                {
                    d.DEPARTMENT_NAME,
                    EmployeeCount = d.EMPLOYEES.Count
                });

            Console.WriteLine("Department Name     Employee Count");
            Console.WriteLine("==================================");

            foreach (var dep in depCount)
            {
                Console.WriteLine("{0,16}     {1}", dep.DEPARTMENT_NAME, dep.EmployeeCount);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Get most well paid employee by department
        /// </summary>
        public void AggregateQuery2()
        {
            var dc = new HREntities();

            var bestPaid = dc.DEPARTMENTS
                .Where(d => d.EMPLOYEES.Any() && !d.EMPLOYEE.Equals(null))
                .Select(d => new
                {
                    DepartmentName = d.DEPARTMENT_NAME,
                    Employee = d.EMPLOYEES
                        .OrderByDescending(e => e.SALARY)
                        .Select(e => new
                        {
                            Name = e.LAST_NAME + ", " + e.FIRST_NAME,
                            Salary = e.SALARY
                        })
                        .FirstOrDefault()
                        
                })
                .OrderByDescending(d => d.Employee.Salary);

            //Console.WriteLine(bestPaid.ToString());
            Console.WriteLine("Best paid employees by department:");
            Console.WriteLine("Department             Employee            Salary");
            Console.WriteLine("====================================================");

            foreach (var dep in bestPaid)
            {
                Console.WriteLine("{0,16}    {1, 18}    {2:C}", 
                    dep.DepartmentName,
                    dep.Employee.Name,
                    dep.Employee.Salary);
            }
            Console.WriteLine();
        }

        #endregion

        #region Grouping with Projection

        public void GroupinWithProjectionQuery()
        {
            try
            {
                var result = dc.EMPLOYEES
                    .Where(e => dc.EMPLOYEES.Any(x => x.EMPLOYEE_ID == e.MANAGER_ID))
                    .GroupBy(e => e.MANAGER_ID)
                    .Select(g => new
                    {
                        ManagerId = g.Key,
                        ManagerName = dc.EMPLOYEES.FirstOrDefault(x => x.EMPLOYEE_ID == g.Key).LAST_NAME,
                        EmployeeCount = g.Count(),
                        Employees = g.Select(x => new
                        {
                            Name = x.LAST_NAME + ", " + x.FIRST_NAME,
                            Salary = x.SALARY,
                            Email = x.EMAIL.ToLower()
                        })
                    });

//                Console.WriteLine(result);
//                Console.WriteLine();
                
                Console.WriteLine("Employees by Manager:\n");

                foreach (var item in result)
                {
                    //var manager = dc.EMPLOYEES.FirstOrDefault(e => e.EMPLOYEE_ID == item.ManagerId);

                    Console.WriteLine("Manager: {0}      Count: {1}", item.ManagerName, item.EmployeeCount);
                    Console.WriteLine("====================================================================");
                    
                    foreach (var emp in item.Employees)
                    {
                        Console.WriteLine("   {0,-20} {1:C} {2,16}@ces.io",
                            emp.Name, emp.Salary, emp.Email);
                    }
                    Console.WriteLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
            }

            Console.WriteLine();

        }

        #endregion

        #region Entity SQL

        /// <summary>
        /// Basic Entity SQL using Object Query Syntax
        /// Display Object Query command text
        /// Display final SQL using "ToTraceString()"
        /// </summary>
        public void BasicEntitySQL()
        {
            Console.WriteLine("Basic Entity SQL\n");

            string query = "SELECT VALUE d FROM HREntities.DEPARTMENTS AS d WHERE d.Department_id >= 100";

            ObjectQuery<DEPARTMENT> departments = ((IObjectContextAdapter) dc).ObjectContext.CreateQuery<DEPARTMENT>(query);

            Console.WriteLine(departments.CommandText);
            Console.WriteLine();
            Console.WriteLine(departments.ToTraceString());
            Console.WriteLine();

            foreach (var dep in departments)
            {
                Console.WriteLine(dep.DEPARTMENT_NAME);
            }

            Console.WriteLine("\n");
        }

        public void ParameterizedEntitySQL()
        {
            Console.WriteLine("Parameterized Entity SQL\n");
            string query = "SELECT VALUE d FROM HREntities.DEPARTMENTS AS d WHERE d.Department_id >= @DepartmentID";

            ObjectQuery<DEPARTMENT> departments = ((IObjectContextAdapter)dc).ObjectContext.CreateQuery<DEPARTMENT>(query);

            departments.Parameters.Add(new ObjectParameter("DepartmentID", 100));

            Console.WriteLine(departments.CommandText);
            Console.WriteLine();
            Console.WriteLine(departments.ToTraceString());
            Console.WriteLine();

            foreach (var dep in departments)
            {
                Console.WriteLine(dep.DEPARTMENT_NAME);
            }

            Console.WriteLine("\n");
        }

        #endregion

    }
}