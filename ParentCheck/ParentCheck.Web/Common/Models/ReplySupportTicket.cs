using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class ReplySupportTicket
    {
        public string TicketId { get; set; }
        public string ReplyMessage { get; set; }
    }
}
