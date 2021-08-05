using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class IncidentReportDTO
    {
        public long Id { get; set; }
        public long InstituteId { get; set; }
        public long InstituteUserId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string ResponsibleUserFirstName { get; set; }
        public string ResponsibleUserLastName { get; set; }
    }
}
