using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Venkat.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mark",
                    Department = Dept.IT,
                    Email = "mark123@hotmail.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Mary",
                    Department = Dept.HR,
                    Email = "mary123@hotmail.com"
                }
            );
        }
    }
}
