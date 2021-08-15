using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class TopicContentRequest
    {
        public long id { get; set; }
        public long topicId { get; set; }
        public string contentText { get; set; }
        public int contentTypeId { get; set; }
        public int orderId { get; set; }
        public bool isActive { get; set; }
    }
}
