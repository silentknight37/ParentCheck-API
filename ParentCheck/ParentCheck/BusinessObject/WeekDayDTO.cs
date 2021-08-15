using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class WeekDayDTO
    {
        public WeekDayDTO()
        {
            TimeTables = new List<TimeTableDTO>();
        }
        public string Weekday { get; set; }
        public List<TimeTableDTO> TimeTables { get; set; }
    }
}
