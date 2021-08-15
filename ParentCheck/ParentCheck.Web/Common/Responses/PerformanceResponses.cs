using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class PerformanceResponses
    {
        public List<Performance> performances { get; set; }

        public static PerformanceResponses PopulatePerformanceResponses(List<PerformanceDTO> performanceDTOs)
        {
            var performanceResponses = new PerformanceResponses();

            performanceResponses.performances = new List<Performance>();

            foreach (var performanceDTO in performanceDTOs)
            {
                var performance = new Performance
                {
                    notComplete= performanceDTO.NotComplete,
                    completed= performanceDTO.Completed,
                    percentage= performanceDTO.Percentage,
                    performanceType= performanceDTO.PerformanceType
                };

                performanceResponses.performances.Add(performance);
            }

            return performanceResponses;
        }
    }

    public class Performance
    {
        public string performanceType { get; set; }
        public string notComplete { get; set; }
        public string completed { get; set; }
        public string percentage { get; set; }
    }
}
