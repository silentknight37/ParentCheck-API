using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class PerformanceEnvelop
    {
        public PerformanceEnvelop(List<PerformanceDTO> performanceDTOs)
        {
            this.performances = performanceDTOs;
        }

        public List<PerformanceDTO>performances { get; set; }
    }
}
