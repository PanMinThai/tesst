using Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Services
{
    public class EmployeeService
    {
        private readonly List<Employee> _employees = new();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
            Console.WriteLine("Thêm nhân viên thành công!");
        }

        public void DisplayAllEmployees()
        {
            if (_employees.Count == 0)
            {
                Console.WriteLine("Danh sách nhân viên trống!");
                return;
            }

            Console.WriteLine("\nDANH SÁCH NHÂN VIÊN");
            foreach (var emp in _employees)
            {
                emp.DisplayBasicInfo();
                Console.WriteLine("********************");
            }
        }

        public decimal CalculateTotalMonthlySalary()
        {
            decimal total = 0;
            foreach (var emp in _employees)
            {
                total += emp.CalculateSalary();
            }
            return total;
        }
    }
}
