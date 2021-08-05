using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class IncidentReportEnvelop
    {
        public IncidentReportEnvelop(List<IncidentReportDTO> incidentReportDTOs)
        {
            this.IncidentReports = incidentReportDTOs;
        }

        public List<IncidentReportDTO> IncidentReports { get; set; }
    }
}
