using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class TimeTableResponses
    {
        public List<WeekDay> weekDays { get; set; }

        public static TimeTableResponses PopulateTimeTableResponses(List<WeekDayDTO> weekDayDTOs)
        {
            var timeTableResponses = new TimeTableResponses();

            timeTableResponses.weekDays = new List<WeekDay>();

            foreach (var weekDayDTO in weekDayDTOs)
            {
                var weekDay = new WeekDay();
                weekDay.Weekday = weekDayDTO.Weekday;

                foreach (var timeTableDTO in weekDayDTO.TimeTables)
                {
                    var timeTable = new TimeTable
                    {
                        className = timeTableDTO.ClassName,
                        subject = timeTableDTO.Subject,
                        weekday = timeTableDTO.Weekday,
                        fromTime = timeTableDTO.FromTime,
                        toTime = timeTableDTO.ToTime
                    };
                    weekDay.TimeTables.Add(timeTable);
                }


                timeTableResponses.weekDays.Add(weekDay);
            }

            return timeTableResponses;
        }
    }

    public class TimeTable
    {
        public string className { get; set; }
        public string subject { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string weekday { get; set; }
    }

    public class WeekDay
    {
        public WeekDay()
        {
            TimeTables = new List<TimeTable>();
        }
        public string Weekday { get; set; }
        public List<TimeTable> TimeTables { get; set; }
    }
}
