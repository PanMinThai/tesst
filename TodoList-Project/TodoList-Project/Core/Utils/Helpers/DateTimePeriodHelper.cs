using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.Utils.Helpers
{
    public static class DateTimePeriodHelper
    {
        public static (DateTime StartDate, DateTime EndDate) GetDateRange(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var today = DateTime.Today;

            return period switch
            {
                DateTimePeriod.Today => (today, today),
                DateTimePeriod.Yesterday => (today.AddDays(-1), today.AddDays(-1)),
                DateTimePeriod.ThisWeek => (GetStartOfWeek(today), GetEndOfWeek(today)),
                DateTimePeriod.ThisMonth => (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month))),
                DateTimePeriod.LastWeek => GetLastWeekRange(today),
                DateTimePeriod.LastMonth => GetLastMonthRange(today),
                DateTimePeriod.Custom when customStartDate.HasValue && customEndDate.HasValue
                    => (customStartDate.Value.Date, customEndDate.Value.Date),
                _ => throw new ArgumentException("Invalid period or missing custom dates")
            };
        }

        private static DateTime GetStartOfWeek(DateTime date)
        {
            return date.AddDays(-(int)date.DayOfWeek);
        }

        private static DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }

        private static (DateTime StartDate, DateTime EndDate) GetLastWeekRange(DateTime today)
        {
            var startOfThisWeek = GetStartOfWeek(today);
            var startOfLastWeek = startOfThisWeek.AddDays(-7);
            var endOfLastWeek = startOfThisWeek.AddDays(-1);
            return (startOfLastWeek, endOfLastWeek);
        }

        private static (DateTime StartDate, DateTime EndDate) GetLastMonthRange(DateTime date)
        {
            var firstDayOfLastMonth = new DateTime(date.Year, date.Month, 1).AddMonths(-1);
            var lastDayOfLastMonth = new DateTime(date.Year, date.Month, 1).AddDays(-1);
            return (firstDayOfLastMonth, lastDayOfLastMonth);
        }
    }
}
