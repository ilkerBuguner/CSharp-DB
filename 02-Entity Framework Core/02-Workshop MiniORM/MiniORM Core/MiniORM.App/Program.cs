using MiniORM.App.Data;
using MiniORM.App.Data.Entities;
using System;
using System.Linq;

namespace MiniORM.App
{
    public class Program
    {
        private static string connectionString = @"Server=.\SQLEXPRESS;Database=MiniORM;Integrated Security=True;";
        static void Main(string[] args)
        {
            var db = new SoftUniDbContext(connectionString);

            db.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = db.Departments.First().Id,
                IsEmployed = true
            });

            var employee = db.Employees.Last();
            employee.FirstName = "Modified";

            db.SaveChanges();
        }
    }
}
