using Exercise1.Models;
using Exercise1.Services;
using System.Globalization;

namespace Exercise1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employeeService = new EmployeeService();

                while (true)
                {
                    Console.WriteLine("\nQUẢN LÝ NHÂN VIÊN");
                    Console.WriteLine("1. Thêm nhân viên toàn thời gian");
                    Console.WriteLine("2. Thêm nhân viên bán thời gian");
                    Console.WriteLine("3. Hiển thị danh sách");
                    Console.WriteLine("4. Tổng lương hàng tháng");
                    Console.WriteLine("5. Thoát");
                    Console.Write("Chọn: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddFullTimeEmployee(employeeService);
                            break;
                        case "2":
                            AddPartTimeEmployee(employeeService);
                            break;
                        case "3":
                            employeeService.DisplayAllEmployees();
                            break;
                        case "4":
                            Console.WriteLine($"Tổng lương tháng: {employeeService.CalculateTotalMonthlySalary():N0}");
                            break;
                        case "5":
                            return;
                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ!");
                            break;
                    }
                }
            }

            static void AddFullTimeEmployee(EmployeeService service)
            {
                Console.WriteLine("\nTHÊM NHÂN VIÊN TOÀN THỜI GIAN");

                var id = GetInputInt("Nhập ID: ");
                var fullName = GetInputString("Nhập họ tên: ");
                var email = GetInputString("Nhập email: ");
                var joinDate = GetInputDate("Nhập ngày vào (dd/MM/yyyy): ");
                var salary = GetInputDecimal("Nhập lương tháng: ");
                var bonus = GetInputDecimal("Nhập thưởng: ");

                var emp = new FullTimeEmployee(id, fullName, email, joinDate, salary, bonus);
                service.AddEmployee(emp);
            }

            static void AddPartTimeEmployee(EmployeeService service)
            {
                Console.WriteLine("\nTHÊM NHÂN VIÊN BÁN THỜI GIAN");

                var id = GetInputInt("Nhập ID: ");
                var fullName = GetInputString("Nhập họ tên: ");
                var email = GetInputString("Nhập email: ");
                var joinDate = GetInputDate("Nhập ngày vào (dd/MM/yyyy): ");
                var hourlyRate = GetInputDecimal("Nhập lương giờ: ");
                var hoursWorked = GetInputInt("Nhập số giờ làm: ");

                var emp = new PartTimeEmployee(id, fullName, email, joinDate, hourlyRate, hoursWorked);
                service.AddEmployee(emp);
            }

            static string GetInputString(string prompt)
            {
                Console.Write(prompt);
                return Console.ReadLine()?.Trim() ?? string.Empty;
            }

            static int GetInputInt(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    if (int.TryParse(Console.ReadLine(), out int result))
                        return result;
                    Console.WriteLine("Vui lòng nhập số nguyên!");
                }
            }

            static decimal GetInputDecimal(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    if (decimal.TryParse(Console.ReadLine(), out decimal result))
                        return result;
                    Console.WriteLine("Vui lòng nhập số!");
                }
            }

            static DateTime GetInputDate(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    var input = Console.ReadLine();
                    if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime result))
                        return result;
                    Console.WriteLine("Vui lòng nhập đúng định dạng dd/MM/yyyy!");
                }
            
        }

    }
}
