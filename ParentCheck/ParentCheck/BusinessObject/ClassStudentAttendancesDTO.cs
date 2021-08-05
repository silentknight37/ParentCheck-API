using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class ClassStudentAttendancesDTO
    {
        public long Id { get; set; }
        public long InstituteId { get; set; }
        public long InstituteUserId { get; set; }
        public long InstituteClassId { get; set; }
        public long InstituteUserClassId { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsAttendance { get; set; }
        public bool IsMarked { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserClassName { get; set; }
        public string ResponsibleUserFirstName { get; set; }
        public string ResponsibleUserLastName { get; set; }
    }
}
