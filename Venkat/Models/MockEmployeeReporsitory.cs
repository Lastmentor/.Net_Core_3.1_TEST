using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Venkat.Models
{
    public class MockEmployeeReporsitory : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeReporsitory()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Alex", Department = Dept.HR, Email = "alex@hotmail.com"},
                new Employee() { Id = 2, Name = "David", Department = Dept.IT, Email = "david@hotmail.com"},
                new Employee() { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@hotmail.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(x => x.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int employeeDelete)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == employeeDelete);
            _employeeList.Remove(employee);
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(x => x.Id == Id);
        }

        public Employee Update(Employee employeeUpdate)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == employeeUpdate.Id);
            employee.Name = employeeUpdate.Name;
            employee.Email = employeeUpdate.Email;
            employee.Department = employeeUpdate.Department;
            return employee;
        }
    }
}
