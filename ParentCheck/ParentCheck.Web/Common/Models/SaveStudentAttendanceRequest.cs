using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class SaveStudentAttendanceRequest
    {
        public long instituteUserId { get; set; }
        public long instituteClassId { get; set; }
        public DateTime recordDate { get; set; }
        public bool isAttendance { get; set; }
        public bool isReset { get; set; }
    }
}
