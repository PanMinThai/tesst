using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Models
{
    public class FullTimeEmployee : Employee
    {
        public decimal MonthlySalary { get; private set; }
        public decimal Bonus { get; private set; }

        public FullTimeEmployee(int id, string fullName, string email,
                              DateTime joinDate, decimal monthlySalary, decimal bonus)
            : base(id, fullName, email, joinDate)
        {
            MonthlySalary = monthlySalary;
            Bonus = bonus;
        }

        public override decimal CalculateSalary()
        {
            return MonthlySalary + Bonus;
        }

        public override void DisplayBasicInfo()
        {
            base.DisplayBasicInfo();
            Console.WriteLine($"Loại NV: Toàn thời gian");
            Console.WriteLine($"Lương tháng: {MonthlySalary:N0}");
            Console.WriteLine($"Thưởng: {Bonus:N0}");
            Console.WriteLine($"Tổng lương: {CalculateSalary():N0}");
        }
    }
}
