using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Models
{
    public abstract class Employee
    {
        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public DateTime JoinDate { get; private set; }

        protected Employee(int id, string fullName, string email, DateTime joinDate)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            JoinDate = joinDate;
        }

        public abstract decimal CalculateSalary();

        public virtual void DisplayBasicInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Họ tên: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Ngày vào: {JoinDate:dd/MM/yyyy}");
        }
    }
}
