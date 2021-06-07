using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class CalenderEventResponses
    {
        public List<CalenderEvent> calenderEvents { get; set; }

        public static CalenderEventResponses PopulateCalenderEventResponses(List<CalenderEventDTO> calenderEventResponses)
        {
            var calanderResponses = new CalenderEventResponses();
            calanderResponses.calenderEvents = new List<CalenderEvent>();

            foreach (var calenderEventResponse in calenderEventResponses)
            {
                var calenderEvents = new CalenderEvent
                {
                    id=calenderEventResponse.Id,
                    subject = calenderEventResponse.Subject,
                    description = calenderEventResponse.Description,
                    fromDate = calenderEventResponse.FromDate,
                    toDate = calenderEventResponse.ToDate,
                    type = calenderEventResponse.Type,
                    colorCode= calenderEventResponse.ColorCode
                };

                calanderResponses.calenderEvents.Add(calenderEvents);
            }

            return calanderResponses;
        }
    }

    public class CalenderEvent
    {
        public long id { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public int type { get; set; }
        public string colorCode { get; set; }
    }
}
