using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class TimeTableEnvelop
    {
        public TimeTableEnvelop(List<WeekDayDTO> weekDayDTOs)
        {
            this.weekDays = weekDayDTOs;
        }

        public List<WeekDayDTO> weekDays { get; set; }
    }
}
