using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Models
{
    public class PartTimeEmployee : Employee
    {
        public decimal HourlyRate { get; private set; }
        public int HoursWorked { get; private set; }

        public PartTimeEmployee(int id, string fullName, string email,
                              DateTime joinDate, decimal hourlyRate, int hoursWorked)
            : base(id, fullName, email, joinDate)
        {
            HourlyRate = hourlyRate;
            HoursWorked = hoursWorked;
        }

        public override decimal CalculateSalary()
        {
            return HourlyRate * HoursWorked;
        }

        public override void DisplayBasicInfo()
        {
            base.DisplayBasicInfo();
            Console.WriteLine($"Loại NV: Bán thời gian");
            Console.WriteLine($"Lương giờ: {HourlyRate:N0}");
            Console.WriteLine($"Số giờ làm: {HoursWorked}");
            Console.WriteLine($"Tổng lương: {CalculateSalary():N0}");
        }
    }
}
