using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs.Services
{
    internal class EmailPreparer
    {
        public const int MorningEndHour = 12;
        public const int AfternoonEndHour = 18;

        public const string DateFormat = "dddd MMMM dd, yyyy";

        public const string SubjectTemplate =  "CVS Sheet for {0} (Week {1})";
        public const string BodyTemplate = 
            "Good {0},\n\n" +
            "See attached the CVS sheet for the week of {1} (Week {2}).\n\n" + 
            "Have a great day,\n" + 
            "{3}";

        private static string getDayTimeStr(DateTime sendDate)
        {
            int currentHour = sendDate.Hour;

            if (currentHour < MorningEndHour)
            {
                return "Morning";
            }
            else if (currentHour < AfternoonEndHour) {
                return "Afternoon";
            }
            else
            {
                return "Evening";
            }
        }

        public static string PrepareSubjectText(DateTime sheetDate, int sheetWeekNum)
        {
            return string.Format(SubjectTemplate, sheetDate, sheetWeekNum);
        }

        public static string PrepareBodyText(DateTime sheetDate, int sheetWeekNum, string inspectorName)
        {
            string timeStr = getDayTimeStr(DateTime.Now);
            string dateStr = sheetDate.ToString(DateFormat);

            return string.Format(BodyTemplate, timeStr, dateStr,
                sheetWeekNum.ToString(), inspectorName);
        }
    }
}
