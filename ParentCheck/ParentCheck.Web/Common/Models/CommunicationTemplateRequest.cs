using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class CommunicationTemplateRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsSenderTemplate { get; set; }
        public bool IsActive { get; set; }
    }
}
