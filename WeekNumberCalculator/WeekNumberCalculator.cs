using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class WeekNumberCalculator
{
    public static DateTime CalcFirstWeekStart(int year)
    {
        DayOfWeek actualStartDay = DayOfWeek.Monday;

        DateTime firstWeekStart = new DateTime(year, 04, 01);

        if (firstWeekStart.DayOfWeek != actualStartDay)
        {
            int diff = actualStartDay - firstWeekStart.DayOfWeek;
            if (diff < 0) diff = 7 + diff;
            firstWeekStart = firstWeekStart.AddDays(diff);
        }

        return firstWeekStart;
    }

    public static int CalcWeekNumber(DateTime date)
    {
        DateTime firstWeekStart = CalcFirstWeekStart(date.Year);

        if (date < firstWeekStart) { return 0; }

        var dateDiff = date - firstWeekStart;

        return dateDiff.Days / 7 + 1;
    }

    public static DateTime CalcStartOfWeek(int weekNumber, int year)
    {
        DateTime firstWeekStart = CalcFirstWeekStart(year);
        int days = (weekNumber - 1) * 7;
        return firstWeekStart.AddDays(days);
    }
}
