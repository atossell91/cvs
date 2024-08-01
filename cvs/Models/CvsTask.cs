using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs.Models
{
    public class CvsTask
    {
        public static int[] ShiftOptions { get; set; } = { 1, 2, 3, 4 };
        public static string[] TaskRatingOptions { get; set; } = { "A", "C", "U", "E" };

        public DateTime Date { get; set; }
        public int Shift { 
            get;
            set;
        } // 1 or 2 (could also be an enum)
        public DateTime TimeIn { get; set; } // Don't really need the date, just the time
        public DateTime TimeOut { get; set; } // See above
        public string ActivityCode { get; set; } // Usually 9.1.12 or 9.1.12
        public string TaskRating { get; set; } // Usually 'C'
        public string ActivityConducted { get; set; } // 'Kill did not go past 18:00'
        public string ItemsNeedingCorrection { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public string InspectorNames { get; set; } // Maybe an array, or list?

    }
}
