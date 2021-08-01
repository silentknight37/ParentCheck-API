using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class ComposeCommunicationRequest
    {
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public List<UserContactRequest> ToUsers { get; set; }
        public List<ReferenceRequest> ToGroups { get; set; }
        public bool IsGroup { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? TemplateId { get; set; }
        public int CommunicationType { get; set; }
    }
}
