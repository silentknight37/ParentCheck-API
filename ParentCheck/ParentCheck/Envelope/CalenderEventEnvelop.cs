using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class CalenderEventEnvelop
    {
        public CalenderEventEnvelop(List<CalenderEventDTO> calenderEvents)
        {
            this.CalenderEvents = calenderEvents;
        }

        public List<CalenderEventDTO> CalenderEvents { get; set; }
    }
}
