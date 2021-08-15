using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class SubjectChapterDTO
    {
        public long InstituteSubjectChapterId { get; set; }
        public string Chapter { get; set; }
        public int TopicCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssignmentAssigned { get; set; }
        public long? InstituteAssignmentId { get; set; }
    }
}
