using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class NewSupportTicket
    {
        public string Subject { get; set; }
        public string IssueText { get; set; }
    }
}
