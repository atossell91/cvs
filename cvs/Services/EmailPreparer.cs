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

        public readonly string DateFormat = "dddd MMMM dd, yyyy";

        public readonly string SubjectTemplate =  "CVS Sheet for {0} (Week {1})";
        public readonly string BodyTemplate = 
            "Good {0},\n\n" +
            "See attached the CVS sheet for the week of {1} (Week {2}).\n\n" + 
            "Have a great day,\n" + 
            "{3}";

        private string getDayTimeStr()
        {
            int currentHour = DateTime.Now.Hour;

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

        private string prepareSubjectText(DateTime sheetDate, int sheetWeekNum)
        {
            return string.Format(SubjectTemplate, sheetDate, sheetWeekNum);
        }

        private string prepareBodyText(DateTime sheetDate, int sheetWeekNum, string inspectorName)
        {
            string timeStr = getDayTimeStr();
            string dateStr = sheetDate.ToString(DateFormat);

            return string.Format(BodyTemplate, timeStr, dateStr,
                sheetWeekNum.ToString(), inspectorName);
        }

        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public int WeekNumber { get; set; }
        public DateTime SheetDate { get; set; }
        public string InspectorName { get; set; }
        public string SavedFilepath { get; set; }
    }
}
