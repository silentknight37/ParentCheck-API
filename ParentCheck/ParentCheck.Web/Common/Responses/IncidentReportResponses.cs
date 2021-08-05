using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class IncidentReportResponses
    {
        public List<IncidentReport> incidentReports { get; set; }

        public static IncidentReportResponses PopulateIncidentReportResponses(List<IncidentReportDTO> incidentReportDTOs)
        {
            var incidentReportResponses = new IncidentReportResponses();
            incidentReportResponses.incidentReports = new List<IncidentReport>();

            foreach (var incidentReportDTO in incidentReportDTOs)
            {
                var incidentReport = new IncidentReport
                {
                    id = incidentReportDTO.Id,
                    instituteId = incidentReportDTO.InstituteId,
                    recordDate = incidentReportDTO.RecordDate,
                    subject = incidentReportDTO.Subject,
                    message = incidentReportDTO.Message,
                    incidentUserName = $"{incidentReportDTO.UserFirstName} {incidentReportDTO.UserLastName}",
                    responsibleUserName = $"{incidentReportDTO.ResponsibleUserFirstName} {incidentReportDTO.ResponsibleUserLastName}",
                };

                incidentReportResponses.incidentReports.Add(incidentReport);
            }

            return incidentReportResponses;
        }
    }

    public class IncidentReport
    {
        public long id { get; set; }
        public long instituteId { get; set; }
        public DateTime recordDate { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string incidentUserName { get; set; }
        public string responsibleUserName { get; set; }
    }
}
