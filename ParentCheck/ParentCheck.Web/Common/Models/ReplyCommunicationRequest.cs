using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class ReplyCommunicationRequest
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public long ToUserId { get; set; }
    }
}
